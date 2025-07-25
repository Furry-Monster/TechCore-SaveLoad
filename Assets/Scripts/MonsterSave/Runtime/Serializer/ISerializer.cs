using System;

namespace MonsterSave.Runtime
{
    public interface ISerializer
    {
        public bool IsBinary { get; }
        public byte[] Serialize(object obj);
        public object Deserialize(Type type, byte[] data);
        public byte[] Serialize<T>(T obj);
        public T Deserialize<T>(byte[] data);
    }
}