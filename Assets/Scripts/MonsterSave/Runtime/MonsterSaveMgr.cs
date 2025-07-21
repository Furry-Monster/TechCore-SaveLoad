using System;

namespace MonsterSave.Runtime
{
    public class MonsterSaveMgr : Singleton<MonsterSaveMgr>
    {
        private ISerializer _serializer = new Serializer();
        private IStorage _storage = new Storage();

        public void Save(string key, object data)
        {
        }

        public object Load(string key)
        {
            throw new NotImplementedException();
        }

        public T Load<T>(string key)
        {
            throw new NotImplementedException();
        }
    }
}