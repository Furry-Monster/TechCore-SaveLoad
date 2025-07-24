using System;

namespace MonsterSave.Runtime
{
    public class NoCache : ICache
    {
        public object Get(string key)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int Count { get; }
        public int Capacity { get; set; }
    }
}