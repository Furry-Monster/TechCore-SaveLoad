using System;
using JetBrains.Annotations;

namespace MonsterSave.Runtime
{
    public interface ISerializer
    {
        public IBinarySerializer BinarySerializer { get; set; }
        public IJSONSerializer JSONSerializer { get; set; }
        public IXMLSerializer XMLSerializer { get; set; }

        byte[] SerializeToBinary(object serializable);
        string SerializeToJson(object serializable);
        string SerializeToXML(object serializable);
        object DeserializeFromBinary([NotNull] Type type, byte[] binary);
        object DeserializeFromJson([NotNull] Type type, string json);
        object DeserializeFromXML([NotNull] Type type, string xml);
    }
}