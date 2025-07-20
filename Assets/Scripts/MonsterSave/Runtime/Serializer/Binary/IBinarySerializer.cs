using System;

namespace MonsterSave.Runtime
{
    public interface IBinarySerializer
    {
        byte[] Serialize(object serializable);
        object Deserialize(Type type, byte[] bytes);
    }
}