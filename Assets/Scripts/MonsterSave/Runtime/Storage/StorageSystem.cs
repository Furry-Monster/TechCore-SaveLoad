using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class StorageSystem : IStorage
    {
        public readonly MonsterSaveConfig Config;

        private readonly IStorageMedia _media;
        private readonly ISerializer _serializer;

        public StorageSystem(MonsterSaveConfig config)
        {
            Config = config;

            var mediaEnum = Config.media;
            var formatEnum = Config.format;

            _media = mediaEnum switch
            {
                Media.LocalFile => new LocalFileMedia(this),
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
        }

        public void Save(object data)
        {
            var serialized = _serializer.Serialize(data);
            _media.WriteAllBytes(serialized);
        }

        public object Load()
        {
            throw new NotImplementedException();
        }

        public T Load<T>()
        {
            if (!_media.Exists())
                return default;

            var serialized = _media.ReadAllBytes();

            if (serialized == null || serialized.Length == 0)
                return default;

            return (T)_serializer.Deserialize(typeof(T), serialized);
        }

        public bool Sync()
        {
            // 仅内存存储时不进行持久化
            if (Config.media == Media.MemoryOnly)
                return true;

            // 兼容PlayerPrefs
            if (Config.media == Media.PlayerPrefs)
            {
                PlayerPrefs.Save();
                return true;
            }

            // 其他存储


            return true;
        }
    }
}