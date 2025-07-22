using System;

namespace MonsterSave.Runtime
{
    public abstract class BinarySerializer : ISerializer
    {
        public bool IsBinary => true;
        public abstract byte[] Serialize(object serializable);
        public abstract object Deserialize(Type type, byte[] bytes);
        public abstract T Deserialize<T>(byte[] bytes);
    }
}