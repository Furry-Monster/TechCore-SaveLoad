using System;

namespace MonsterSave.Runtime
{
    public interface IJSONSerializer
    {
        string Serialize(object serializable);
        object Deserialize(Type type, string json);
    }
}