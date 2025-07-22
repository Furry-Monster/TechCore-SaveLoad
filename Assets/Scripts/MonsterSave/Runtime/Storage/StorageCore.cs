using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class StorageCore : IStorage
    {
        public readonly MonsterSaveConfig Config;

        private readonly IStorageMedia _media;
        private readonly ISerializer _serializer;

        public StorageCore(MonsterSaveConfig config)
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
        }

        public object Load()
        {
            throw new NotImplementedException();
        }

        public T Load<T>()
        {
            throw new NotImplementedException();
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