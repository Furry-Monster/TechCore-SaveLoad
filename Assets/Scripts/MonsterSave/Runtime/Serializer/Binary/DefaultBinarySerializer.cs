using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MonsterSave.Runtime
{
    public class DefaultBinarySerializer : IBinarySerializer
    {
        public byte[] Serialize(object serializable)
        {
            if (serializable == null)
                return null;

            // 检查属性
            if (!serializable.GetType().IsSerializable)
                throw new InvalidOperationException($"Type {serializable.GetType()} is not marked as [Serializable].");

            var formatter = new BinaryFormatter();
            using var memoryStream = new MemoryStream();
            formatter.Serialize(memoryStream, serializable);

            return memoryStream.ToArray();
        }

        public object Deserialize(Type type, byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            var formatter = new BinaryFormatter();
            using var memoryStream = new MemoryStream(bytes);

            return formatter.Deserialize(memoryStream);
        }
    }
}