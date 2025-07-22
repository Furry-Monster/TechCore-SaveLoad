using System;

namespace MonsterSave.Runtime
{
    public abstract class CharSerializer : ISerializer
    {
        public bool IsBinary => false;
        public abstract string Serialize(object serializable);
        public abstract object Deserialize(Type type, string content);
        public abstract T Deserialize<T>(string content);
    }
}