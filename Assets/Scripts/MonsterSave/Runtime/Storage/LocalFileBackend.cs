using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class LocalFileBackend : IStorageBackend
    {
        private readonly string _path;
        private readonly ISerializer _serializer;

        public LocalFileBackend(MonsterSaveConfig config)
        {
            _path = !string.IsNullOrEmpty(config.storagePath)
                ? config.storagePath
                : Application.persistentDataPath + "/save.ms";

            _serializer = config.format switch
            {
                Format.JSON => new DefaultJSONSerializer(),
                Format.XML => new DefaultXMLSerializer(),
                Format.Binary => new DefaultBinarySerializer(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        // 读取整个存档文件为字典
        private Dictionary<string, byte[]> LoadAllInternal()
        {
            if (!File.Exists(_path))
                return new Dictionary<string, byte[]>();
            var bytes = File.ReadAllBytes(_path);
            return _serializer.Deserialize<Dictionary<string, byte[]>>(bytes);
        }

        // 写入整个字典到存档文件
        private void SaveAllInternal(Dictionary<string, byte[]> allData)
        {
            EnsureDirectory();
            var bytes = _serializer.Serialize(allData);
            File.WriteAllBytes(_path, bytes);
        }

        public void Write(string key, byte[] data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (data == null || data.Length == 0)
                throw new ArgumentNullException(nameof(data));

            var allData = LoadAllInternal();
            allData[key] = data;
            SaveAllInternal(allData);
        }

        public byte[] Read(string key)
        {
            var allData = LoadAllInternal();
            return allData.GetValueOrDefault(key);
        }

        public void Delete(string key)
        {
            var allData = LoadAllInternal();
            if (allData.Remove(key))
                SaveAllInternal(allData);
        }

        public bool HasKey(string key)
        {
            var allData = LoadAllInternal();
            return allData.ContainsKey(key);
        }

        public void WriteAll(Dictionary<string, byte[]> allData)
        {
            if (allData == null)
                throw new ArgumentNullException(nameof(allData));
            SaveAllInternal(allData);
        }

        public Dictionary<string, byte[]> ReadAll()
        {
            return LoadAllInternal();
        }

        private void EnsureDirectory()
        {
            var directory = Path.GetDirectoryName(_path);
            if (string.IsNullOrEmpty(directory))
                throw new Exception($"Can't get directory path from {_path}");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
    }
}