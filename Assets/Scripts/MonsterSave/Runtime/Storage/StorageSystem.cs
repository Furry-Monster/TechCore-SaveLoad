using System;

namespace MonsterSave.Runtime
{
    public class StorageSystem : IStorage
    {
        private MonsterSaveConfig _config;
        private IStorageMedia _media;
        private ICache _cache;
        private ISerializer _serializer;


        public StorageSystem()
        {
            MonsterSaveMgr.Instance.OnConfigUpdated += () =>
            {
                _config = MonsterSaveMgr.Instance.Config;

                var mediaEnum = _config.media;
                var formatEnum = _config.format;
                var cacheEnum = _config.cache;

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

                _cache = cacheEnum switch
                {
                    Cache.None => new NoCache(),
                    Cache.LRU => new LRUCache(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            };
        }

        public void Save(string key, object data)
        {
            throw new NotImplementedException();
        }

        public object Load(string key)
        {
            throw new NotImplementedException();
        }

        public T Load<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool Sync()
        {
            throw new NotImplementedException();
        }
    }
}