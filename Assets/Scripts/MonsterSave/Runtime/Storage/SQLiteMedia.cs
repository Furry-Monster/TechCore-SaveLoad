using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public class SQLiteMedia : IStorageMedia
    {
        public Storage Storage { private get; set; }

        public SQLiteMedia(Storage storage)
        {
            Storage = storage;
        }

        public void SaveString(Dictionary<string, string> data)
        {
            throw new NotImplementedException();
        }

        public void SaveBytes(Dictionary<string, byte[]> data)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> LoadString()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, byte[]> LoadBytes()
        {
            throw new NotImplementedException();
        }
    }
}