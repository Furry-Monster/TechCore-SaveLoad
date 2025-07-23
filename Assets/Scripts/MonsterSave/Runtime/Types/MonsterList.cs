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
    public class MonsterList<T>
    {
        public T[] items;
        public int count;

        public MonsterList()
        {
            items = Array.Empty<T>();
            count = 0;
        }

        public MonsterList(IEnumerable<T> collection)
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

    public class ListAdapter<T> :
        ITypeAdapter<List<T>, MonsterList<T>>
    {
        public MonsterList<T> ConvertToSerializable(List<T> native) => new(native);

        public List<T> ConvertFromSerializable(MonsterList<T> serializable) =>
            new(serializable.items.Take(serializable.count));
    }
}