using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public class StorageSystem : IStorage
    {
        private MonsterSaveConfig _config;
        private ISerializer _serializer;
        private IStorageMedia _media;

        private Dictionary<string, object> _mediaCache = new();

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
            if (string.IsNullOrEmpty(key) || data == null)
                return;

            _mediaCache.Add(key, data);
        }

        public object Load(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            return _mediaCache.GetValueOrDefault(key);
        }

        public T Load<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default;

            return (T)_mediaCache.GetValueOrDefault(key);
        }

        public bool Sync(bool toDisk = true)
        {
            if (toDisk)
            {
                if (_media is IFullStoreMedia fullStore)
                {
                    var serialized = _serializer.Serialize(_mediaCache);
                    fullStore.WriteAllBytes(serialized);
                    return true;
                }
                else if (_media is IKVStoreMedia kvStore)
                {
                    return false;
                }
            }
            else
            {
                if (_media is IFullStoreMedia fullStore)
                {
                    var serialized = fullStore.ReadAllBytes();
                    _mediaCache =
                        (Dictionary<string, object>)_serializer.Deserialize(typeof(Dictionary<string, object>),
                            serialized);
                    return true;
                }
                else if (_media is IKVStoreMedia kvStore)
                {
                    return false;
                }
            }

            return false;
        }
    }
}