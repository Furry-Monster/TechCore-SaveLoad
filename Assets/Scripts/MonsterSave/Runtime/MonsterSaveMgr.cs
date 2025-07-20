using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class MonsterSaveMgr : Singleton<MonsterSaveMgr>
    {
        private ISerializer _serializer;
        private IStorage _storage;

        public void Save(string key, object data)
        {
            throw new NotImplementedException();
        }

        public object Load(string key)
        {
            throw new NotImplementedException();
        }
    }
}