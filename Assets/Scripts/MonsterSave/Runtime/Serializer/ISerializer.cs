using System;

namespace MonsterSave.Runtime
{
    public interface ISerializer
    {
        byte[] SerializeToBinary(object serializable);
        string SerializeToJson(object serializable);
        string SerializeToXML(object serializable);
        object DeserializeFromBinary(Type type, byte[] binary);
        object DeserializeFromJson(Type type, string json);
        object DeserializeFromXML(Type type, string xml);
    }
}