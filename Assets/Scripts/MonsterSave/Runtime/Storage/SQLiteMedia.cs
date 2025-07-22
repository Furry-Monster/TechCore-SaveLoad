using System;

namespace MonsterSave.Runtime
{
    public class SQLiteMedia : IStorageMedia
    {
        public void WriteAllText(string content)
        {
            throw new NotImplementedException();
        }

        public string ReadAllText()
        {
            throw new NotImplementedException();
        }

        public void WriteAllBytes(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadAllBytes()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public bool Exists()
        {
            throw new NotImplementedException();
        }
    }
}