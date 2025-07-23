using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    [Serializable]
    public class MonsterDict<TKey, TValue>
    {
        [Serializable]
        public struct Entry
        {
            public TKey key;
            public TValue value;

            public Entry(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }

        public MonsterList<Entry> entries = new();

        public MonsterDict()
        {
        }

        public MonsterDict(Dictionary<TKey, TValue> dict)
        {
            foreach (var kvp in dict)
            {
                entries.Add(new Entry(kvp.Key, kvp.Value));
            }
        }
    }

    public class DictionaryAdapter<TKey, TValue> : ITypeAdapter<Dictionary<TKey, TValue>, MonsterDict<TKey, TValue>>
    {
        public MonsterDict<TKey, TValue> ConvertToSerializable(Dictionary<TKey, TValue> native) => new(native);

        public Dictionary<TKey, TValue> ConvertFromSerializable(MonsterDict<TKey, TValue> serializable)
        {
            var dict = new Dictionary<TKey, TValue>();
            var count = serializable.entries.count;

            // 轻量化的ArrayList没有迭代器接口，这里我们直接遍历
            for (var i = 0; i < count; i++)
                dict.Add(serializable.entries[i].key, serializable.entries[i].value);

            return dict;
        }
    }
}