using System;
using System.IO;

namespace MonsterSave.Runtime
{
    public class LocalFileMedia : IFullStoreMedia
    {
        private string _path;

        public LocalFileMedia()
        {
            MonsterSaveMgr.Instance.OnConfigUpdated += () =>
                _path = MonsterSaveMgr.Instance.Config.storagePath;
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
    }
}