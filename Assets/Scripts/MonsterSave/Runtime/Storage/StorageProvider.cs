using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public class StorageProvider : IStorageProvider
    {
        private MonsterSaveConfig _config;
        private ISerializer _serializer;
        private IStorageBackend _backend;

        public StorageProvider()
        {
            MonsterSaveMgr.OnConfigUpdated += () =>
            {
                _config = MonsterSaveMgr.Config;

                var mediaEnum = _config.backend;
                var formatEnum = _config.format;

                _backend = mediaEnum switch
                {
                    Backend.LocalFile => new LocalFileBackend(),
                    Backend.PlayerPrefs => null,
                    Backend.MemoryOnly => null,
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
            throw new NotImplementedException();
        }

        public T Get<T>(string key, T defaultValue = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public bool Exist(string key)
        {
            throw new NotImplementedException();
        }

        public void SyncAll(Dictionary<string, object> allData)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> LoadAll()
        {
            throw new NotImplementedException();
        }
    }
}