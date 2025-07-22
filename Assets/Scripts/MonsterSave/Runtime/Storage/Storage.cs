using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class Storage : IStorage
    {
        public IStorageMedia StorageMedia { get; private set; }
        public MonsterSaveConfig Config { get; private set; }

        private readonly Dictionary<string, string> _textData;
        private readonly Dictionary<string, byte[]> _binaryData;

        public Storage(MonsterSaveConfig config)
        {
            UpdateConfig(config);

            _textData = new Dictionary<string, string>();
            _binaryData = new Dictionary<string, byte[]>();

            if (Config.format == Format.Binary)
            {
                switch (Config.media)
                {
                    case Media.LocalFile:
                        _binaryData = StorageMedia.LoadBytes();
                        break;
                    case Media.PlayerPrefs:
                    case Media.MemoryOnly:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                switch (Config.media)
                {
                    case Media.LocalFile:
                        _textData = StorageMedia.LoadString();
                        break;
                    case Media.PlayerPrefs:
                    case Media.MemoryOnly:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void UpdateConfig(MonsterSaveConfig config = null)
        {
            Config = config ?? Config;

            if (Config == null)
                throw new Exception("Config in 'Storage.cs' is null");

            var media = Config.media;

            StorageMedia = media switch
            {
                Media.LocalFile => new LocalFileMedia(this),
                Media.PlayerPrefs => null,
                Media.MemoryOnly => null,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void SaveText(string key, string data)
        {
            if (string.IsNullOrEmpty(data))
                return;
            if (string.IsNullOrEmpty(key))
                return;

            if (Config.media == Media.PlayerPrefs)
                PlayerPrefs.SetString(key, data);
            else
                _textData[key] = data;
        }

        public void SaveBinary(string key, byte[] data)
        {
            if (data == null || data.Length == 0)
                return;
            if (string.IsNullOrEmpty(key))
                return;

            if (Config.media == Media.PlayerPrefs)
                PlayerPrefs.SetString(key, Convert.ToBase64String(data));
            else
                _binaryData[key] = data;
        }

        public string LoadText(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            if (Config.media == Media.PlayerPrefs)
                return PlayerPrefs.GetString(key);
            else
                return _textData.GetValueOrDefault(key);
        }

        public byte[] LoadBinary(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            if (Config.media == Media.PlayerPrefs)
                return Convert.FromBase64String(PlayerPrefs.GetString(key));
            else
                return _binaryData.GetValueOrDefault(key);
        }

        public bool Sync()
        {
            // 仅内存存储时不进行持久化
            if (Config.media == Media.MemoryOnly)
                return true;

            // 遵循PlayerPrefs设计
            if (Config.media == Media.PlayerPrefs)
            {
                PlayerPrefs.Save();
                return true;
            }

            // 其他存储：
            // 确保配置和注入的存储介质一致
            UpdateConfig();
            if (StorageMedia == null)
                throw new Exception("StorageMedia not initialized");

            switch (Config.format)
            {
                case Format.Binary:
                {
                    if (_binaryData == null || _binaryData.Count == 0)
                        return false;

                    StorageMedia.SaveBytes(_binaryData);
                    break;
                }
                case Format.JSON or Format.XML:
                {
                    if (_textData == null || _textData.Count == 0)
                        return false;

                    StorageMedia.SaveString(_textData);
                    break;
                }
            }

            return true;
        }
    }
}