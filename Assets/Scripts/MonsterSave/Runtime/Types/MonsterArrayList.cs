using System;
using System.Collections.Generic;
using System.Linq;

namespace MonsterSave.Runtime
{
    /// <summary>
    /// 我不确定一个轻量化的List是否必要...
    /// 所以这段代码暂时留在这里
    /// </summary>
    [Serializable]
    public class MonsterArrayList<T>
    {
        public T[] items;
        public int count;

        public MonsterArrayList()
        {
            items = Array.Empty<T>();
            count = 0;
        }

        public MonsterArrayList(IEnumerable<T> collection)
        {
            items = collection.ToArray();
            count = items.Length;
        }

        public void Add(T item)
        {
            items ??= new T[4];

            if (count >= items.Length)
            {
                Array.Resize(ref items, Math.Max(4, items.Length * 2));
            }

            items[count++] = item;
        }

        public T this[int index] => items[index];
    }

    public class ArrayListAdapter<T> :
        ITypeAdapter<List<T>, MonsterArrayList<T>>
    {
        public MonsterArrayList<T> ConvertToSerializable(List<T> native) => new(native);

        public List<T> ConvertFromSerializable(MonsterArrayList<T> serializable) =>
            new(serializable.items.Take(serializable.count));
    }
}