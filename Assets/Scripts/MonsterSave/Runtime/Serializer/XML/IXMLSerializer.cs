using System;

namespace MonsterSave.Runtime
{
    public interface IXMLSerializer
    {
        string Serialize(object serializable);
        object Deserialize(Type type, string xml);
    }
}