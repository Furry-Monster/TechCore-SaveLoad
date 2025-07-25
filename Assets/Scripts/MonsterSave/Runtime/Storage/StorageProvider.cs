using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public class StorageProvider : IStorageProvider
    {
        private ISerializer _serializer;
        private IStorageBackend _backend;

        public StorageProvider()
        {
            MonsterSaveMgr.OnConfigUpdated += () =>
            {
                var config = MonsterSaveMgr.Config;

                var backendEnum = config.backend;
                var formatEnum = config.format;

                _backend = backendEnum switch
                {
                    Backend.LocalFile => new LocalFileBackend(),
                    Backend.PlayerPrefs => new PlayerPrefBackend(),
                    Backend.MemoryOnly => null,
                    Backend.Database => null,
                    Backend.Cloud => new CloudBackend(),
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

        public void Set<T>(string key, T data)
        {
            var serialized = _serializer.Serialize(data);
            _backend.Write(key, serialized);
        }

        public T Get<T>(string key, T defaultValue = default)
        {
            var serialized = _backend.Read(key);
            return serialized == null
                ? defaultValue
                : _serializer.Deserialize<T>(serialized);
        }

        public void Delete(string key) => _backend.Delete(key);

        public bool Exist(string key) => _backend.HasKey(key);

        public void SyncAll(Dictionary<string, object> allData)
        {
            var serialized = new Dictionary<string, byte[]>();
            foreach (var pair in allData)
                serialized[pair.Key] = _serializer.Serialize(pair.Value);
            _backend.WriteAll(serialized);
        }

        public Dictionary<string, object> LoadAll()
        {
            var native = new Dictionary<string, object>();
            var serialized = _backend.ReadAll();
            foreach (var pair in serialized)
                native[pair.Key] = _serializer.Serialize(pair.Value);
            return native;
        }
    }
}