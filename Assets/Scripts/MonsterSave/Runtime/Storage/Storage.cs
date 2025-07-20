using System;
using System.IO;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class Storage : IStorage
    {
        public string StoragePath { get; private set; }

        public IKVStorage KVStorage { get; set; }
        public IStreamStorage StreamStorage { get; set; }

        public Storage()
        {
            StoragePath = Path.Combine(Application.persistentDataPath, "monster.save");

            KVStorage = new PlayerPrefsStorage();
            StreamStorage = new LocalFileStorage(this);
        }

        public void SaveText(string data) => StreamStorage.SaveText(data);
        public void SaveText(string key, string data) => KVStorage.SaveText(key, data);
        public void SaveBinary(byte[] data) => StreamStorage.SaveBinary(data);
        public void SaveBinary(string key, byte[] data) => KVStorage.SaveBinary(key, data);

        public string LoadText() => StreamStorage.LoadText();
        public string LoadText(string key) => KVStorage.LoadText(key);
        public byte[] LoadBinary() => StreamStorage.LoadBinary();
        public byte[] LoadBinary(string key) => KVStorage.LoadBinary(key);

        public void SelectStoragePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or whitespace.", nameof(path));

            StoragePath = Path.IsPathRooted(path)
                ? path
                : Path.Combine(Application.persistentDataPath, path);
        }
    }
}