using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public class Storage : IStorage
    {
        public IStorageMedia StorageMedia { get; private set; } = null;

        private MonsterSaveConfig _config;
        private readonly Dictionary<string, string> _textData;
        private readonly Dictionary<string, byte[]> _binaryData;

        public Storage()
        {
            _textData = new Dictionary<string, string>();
            _binaryData = new Dictionary<string, byte[]>();
        }

        public Storage(MonsterSaveConfig config)
        {
            _config = config;

            _textData = new Dictionary<string, string>();
            _binaryData = new Dictionary<string, byte[]>();
        }

        public void UpdateConfig(MonsterSaveConfig config = null)
        {
            _config = config ?? _config;

            if (_config == null)
                throw new Exception("Config in 'Storage.cs' is null");

            var media = _config.media;

            StorageMedia = media switch
            {
                Media.LocalFile => new LocalFileMedia(),
                Media.PlayerPrefs => new PlayerPrefsMedia(),
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
            _textData[key] = data;
        }

        public void SaveBinary(string key, byte[] data)
        {
            if (data == null || data.Length == 0)
                return;
            if (string.IsNullOrEmpty(key))
                return;
            _binaryData[key] = data;
        }

        public string LoadText(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            return _textData.GetValueOrDefault(key);
        }

        public byte[] LoadBinary(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            return _binaryData.GetValueOrDefault(key);
        }

        public bool Sync()
        {
            // 确保配置和注入的存储介质一致
            UpdateConfig();
            if (StorageMedia == null)
                throw new Exception("StorageMedia not initialized");

            switch (_config.format)
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