using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public class PlayerPrefBackend : IStorageBackend
    {
        public void Write(string key, byte[] data)
        {
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