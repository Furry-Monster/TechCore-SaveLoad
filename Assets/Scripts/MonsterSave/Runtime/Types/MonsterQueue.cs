using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    [Serializable]
    public class MonsterQueue<T>
    {
        public MonsterList<T> items = new();

        public MonsterQueue()
        {
        }

        public MonsterQueue(Queue<T> q)
        {
            foreach (var item in q)
            {
                items.Add(item);
            }
        }
    }

    public class QueueAdapter<T> : ITypeAdapter<Queue<T>, MonsterQueue<T>>
    {
        public MonsterQueue<T> ConvertToSerializable(Queue<T> native) => new(native);

        public Queue<T> ConvertFromSerializable(MonsterQueue<T> serializable)
        {
            var q = new Queue<T>();
            var count = serializable.items.count;

            for (var i = 0; i < count; i++)
            {
                q.Enqueue(serializable.items[i]);
            }

            return q;
        }
    }
}