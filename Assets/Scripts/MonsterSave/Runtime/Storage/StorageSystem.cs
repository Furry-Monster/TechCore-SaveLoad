using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class StorageSystem : IStorage
    {
        private MonsterSaveConfig _config;
        private IStorageMedia _media;
        private ISerializer _serializer;

        private readonly Dictionary<string, object> _cache = new();
        private readonly LinkedList<string> _lruList = new();
        private int _cacheSize = 1024;

        public StorageSystem()
        {
            MonsterSaveMgr.Instance.OnConfigUpdated += () =>
            {
                _config = MonsterSaveMgr.Instance.Config;

                var mediaEnum = _config.media;
                var formatEnum = _config.format;

                _media = mediaEnum switch
                {
                    Media.LocalFile => new LocalFileMedia(),
                    Media.PlayerPrefs => null,
                    Media.MemoryOnly => null,
                    _ => throw new ArgumentOutOfRangeException()
                };

                _serializer = formatEnum switch
                {
                    Format.JSON => new DefaultJSONSerializer(),
                    Format.XML => new DefaultXMLSerializer(),
                    Format.Binary => new DefaultBinarySerializer(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            };
        }

        public void Save(string key, object data)
        {
            // 优先使用缓存
            if (_cache.ContainsKey(key))
            {
                _cache[key] = data;
                _lruList.Remove(key);
            }
            else
            {
                if (_cache.Count >= _cacheSize)
                {
                    // 同步
                    Sync();

                    // LRU 淘汰
                    var lruKey = _lruList.Last.Value;
                    _lruList.RemoveLast();
                    _cache.Remove(lruKey);
                }

                _cache.Add(key, data);
            }

            _lruList.AddFirst(key);
        }

        public object Load(string key)
        {
            if (_cache.TryGetValue(key, out var value))
            {
                // 命中缓存，更新 LRU
                _lruList.Remove(key);
                _lruList.AddFirst(key);
                return value;
            }

            // 未命中缓存，从底层存储加载


            return null;
        }

        public T Load<T>(string key)
        {
            if (_cache.TryGetValue(key, out var value))
            {
                // 命中缓存，更新 LRU
                _lruList.Remove(key);
                _lruList.AddFirst(key);
                return (T)value;
            }

            return default;
        }

        public bool Sync()
        {
            // 仅内存存储时不进行持久化
            if (_config.media == Media.MemoryOnly)
                return true;

            // 兼容PlayerPrefs
            if (_config.media == Media.PlayerPrefs)
            {
                PlayerPrefs.Save();
                return true;
            }

            // 其他存储


            return true;
        }
    }
}