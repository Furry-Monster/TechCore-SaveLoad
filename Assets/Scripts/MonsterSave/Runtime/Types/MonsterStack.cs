using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    [Serializable]
    public class MonsterStack<T>
    {
        public MonsterList<T> items = new();

        public MonsterStack()
        {
        }

        public MonsterStack(Stack<T> s)
        {
            // 枚举的时候是从栈顶到底
            foreach (var item in s)
            {
                items.Add(item);
            }
        }
    }

    public class StackAdapter<T> : ITypeAdapter<Stack<T>, MonsterStack<T>>
    {
        public MonsterStack<T> ConvertToSerializable(Stack<T> native) => new(native);

        public Stack<T> ConvertFromSerializable(MonsterStack<T> serializable)
        {
            var q = new Stack<T>();
            var count = serializable.items.count;

            // 反序列化的时候顺序要反过来
            for (var i = count - 1; i >= 0; i--)
            {
                q.Push(serializable.items[i]);
            }

            return q;
        }
    }
}