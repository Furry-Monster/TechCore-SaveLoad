using System;

namespace MonsterSave.Runtime
{
    public class SQLiteMedia : IStorageMedia
    {
        public void SaveText(string key, string data)
        {
            throw new NotImplementedException();
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
    }
}