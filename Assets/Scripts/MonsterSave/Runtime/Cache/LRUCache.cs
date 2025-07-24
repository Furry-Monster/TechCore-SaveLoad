using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public class LRUCache : ICache
    {
        private Dictionary<string, object> _cache = new();
        private HashSet<string> _dirtyKeys = new();
        private List<string> _lruList = new();

        public LRUCache()
        {
            MonsterSaveMgr.Instance.OnConfigUpdated += ()
                => Capacity = MonsterSaveMgr.Instance.Config.cacheSize;
        }

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