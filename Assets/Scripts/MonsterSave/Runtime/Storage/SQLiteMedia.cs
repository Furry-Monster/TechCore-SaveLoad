using System;

namespace MonsterSave.Runtime
{
    public class SQLiteMedia : IKVStoreMedia
    {
        public string GetText(string key)
        {
            throw new NotImplementedException();
        }

        public void SetText(string key, string text)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBytes(string key)
        {
            throw new NotImplementedException();
        }

        public void SetBytes(string key, byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }
    }
}