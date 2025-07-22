using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public class Storage : IStorage
    {
        public IStorageMedia StorageMedia { get; } = null;

        private MonsterSaveConfig _config;
        private Dictionary<string, string> _textData;
        private Dictionary<string, byte[]> _binaryData;

        public Storage()
        {
            _textData = new Dictionary<string, string>();
            _binaryData = new Dictionary<string, byte[]>();
        }

        public Storage(MonsterSaveConfig config)
        {
            _config = config;

            _textData = new Dictionary<string, string>();
            _binaryData = new Dictionary<string, byte[]>();
        }

        public void UpdateConfig(MonsterSaveConfig config = null)
        {
            if (config == null)
                config = _config;

            _config = config;

            var media = config.media;
            var path = config.storagePath;
        }

        public void SaveText(string key, string data)
        {
        }

        public void SaveBinary(string key, byte[] data)
        {
            throw new NotImplementedException();
        }

        public string LoadText(string key)
        {
            throw new NotImplementedException();
        }

        public byte[] LoadBinary(string key)
        {
            throw new NotImplementedException();
        }

        public bool SyncText()
        {
            throw new NotImplementedException();
        }

        public bool SyncBinary()
        {
            throw new NotImplementedException();
        }

        public bool SyncAll()
        {
            throw new NotImplementedException();
        }
    }
}