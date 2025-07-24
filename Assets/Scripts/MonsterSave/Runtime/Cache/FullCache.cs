using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public class FullCache : ICache
    {
        private Dictionary<string, object> _cache = new();

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