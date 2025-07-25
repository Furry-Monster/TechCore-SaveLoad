using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class LocalFileBackend : IStorageBackend
    {
        private string _path;

        public LocalFileBackend()
        {
            MonsterSaveMgr.OnConfigUpdated += () =>
            {
                _path = MonsterSaveMgr.Config.storagePath
                        ?? Application.persistentDataPath + "save.ms";
            };
        }

        public void WriteAllText(string content)
        {
            EnsureDirectory();
            File.WriteAllText(_path, content);
        }

        public string ReadAllText() => Exists() ? File.ReadAllText(_path) : null;

        public void WriteAllBytes(byte[] bytes)
        {
            EnsureDirectory();
            File.WriteAllBytes(_path, bytes);
        }

        public byte[] ReadAllBytes() => Exists() ? File.ReadAllBytes(_path) : null;

        public void Delete()
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
        }

        public bool Exists() => File.Exists(_path);

        private void EnsureDirectory()
        {
            var directory = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(directory))
                throw new Exception("Can't get directory path from the given path string.");

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public void Write(string key, byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] Read(string key)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public bool HasKey(string key)
        {
            throw new NotImplementedException();
        }

        public void WriteAll(Dictionary<string, byte[]> allData)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, byte[]> ReadAll()
        {
            throw new NotImplementedException();
        }
    }
}