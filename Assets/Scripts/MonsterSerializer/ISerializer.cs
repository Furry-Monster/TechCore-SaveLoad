using System;

namespace MonsterSerializer
{
    public interface ISerializer
    {
        byte[] Serialize(object obj);
        object Deserialize(Type type, byte[] bytes);
    }
}