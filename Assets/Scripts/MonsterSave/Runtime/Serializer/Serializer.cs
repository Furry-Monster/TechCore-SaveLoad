using System;

namespace MonsterSave.Runtime
{
    public class Serializer : ISerializer
    {
        public IBinarySerializer BinarySerializer { get; private set; } = new DefaultBinarySerializer();
        public IJSONSerializer JSONSerializer { get; private set; } = new DefaultJSONSerializer();
        public IXMLSerializer XMLSerializer { get; private set; } = new DefaultXMLSerializer();

        public byte[] SerializeToBinary(object serializable)
        {
            if (serializable == null)
                return null;

            // 检查属性
            if (!serializable.GetType().IsSerializable)
                throw new InvalidOperationException($"Type {serializable.GetType()} is not marked as [Serializable].");

            return BinarySerializer.Serialize(serializable);
        }

        public string SerializeToJson(object serializable)
        {
            if (serializable == null)
                return null;

            // 检查属性
            if (!serializable.GetType().IsSerializable)
                throw new InvalidOperationException($"Type {serializable.GetType()} is not marked as [Serializable].");

            return JSONSerializer.Serialize(serializable);
        }

        public string SerializeToXML(object serializable)
        {
            if (serializable == null)
                return null;

            // 检查属性
            if (!serializable.GetType().IsSerializable)
                throw new InvalidOperationException($"Type {serializable.GetType()} is not marked as [Serializable].");

            return XMLSerializer.Serialize(serializable);
        }

        public object DeserializeFromBinary(Type type, byte[] binary)
        {
            if (binary == null || binary.Length == 0)
                return null;

            return BinarySerializer.Deserialize(type, binary);
        }

        public object DeserializeFromJson(Type type, string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;

            return JSONSerializer.Deserialize(type, json);
        }

        public object DeserializeFromXML(Type type, string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return null;

            return XMLSerializer.Deserialize(type, xml);
        }
    }
}