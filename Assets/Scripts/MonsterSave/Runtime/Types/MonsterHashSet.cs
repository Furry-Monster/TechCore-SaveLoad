using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    [Serializable]
    public class MonsterHashSet<T>
    {
        public MonsterArrayList<T> items = new();

        public MonsterHashSet()
        {
        }

        public MonsterHashSet(HashSet<T> hashSet)
        {
            foreach (var item in hashSet)
            {
                items.Add(item);
            }
        }
    }

    public class HashSetAdapter<T> : ITypeAdapter<HashSet<T>, MonsterHashSet<T>>
    {
        public MonsterHashSet<T> ConvertToSerializable(HashSet<T> native)
        {
            return new MonsterHashSet<T>(native);
        }

        public HashSet<T> ConvertFromSerializable(MonsterHashSet<T> serializable)
        {
            var set = new HashSet<T>();
            var count = serializable.items.count;

            for (var i = 0; i < count; i++)
            {
                set.Add(serializable.items[i]);
            }

            return set;
        }
    }
}