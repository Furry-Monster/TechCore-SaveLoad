using System;

namespace MonsterSave.Runtime
{
    public class Serializer : ISerializer
    {
        private MonsterSaveConfig _config;

        public IBinarySerializer BinarySerializer { get; } = new DefaultBinarySerializer();
        public IJSONSerializer JSONSerializer { get; } = new DefaultJSONSerializer();
        public IXMLSerializer XMLSerializer { get; } = new DefaultXMLSerializer();

        public Serializer(MonsterSaveConfig config)
        {
            _config = config;
        }

        public byte[] SerializeToBinary(object serializable) => BinarySerializer.Serialize(serializable);
        public string SerializeToJson(object serializable) => JSONSerializer.Serialize(serializable);
        public string SerializeToXML(object serializable) => XMLSerializer.Serialize(serializable);
        public object DeserializeFromBinary(Type type, byte[] binary) => BinarySerializer.Deserialize(type, binary);
        public object DeserializeFromJson(Type type, string json) => JSONSerializer.Deserialize(type, json);
        public object DeserializeFromXML(Type type, string xml) => XMLSerializer.Deserialize(type, xml);
    }
}