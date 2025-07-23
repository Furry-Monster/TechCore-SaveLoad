using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MonsterSave.Runtime
{
    public class DefaultBinarySerializer : BinarySerializer
    {
        protected override byte[] Serialize(object serializable)
        {
            if (serializable == null || !serializable.GetType().IsSerializable)
                return null;

            var formatter = new BinaryFormatter();
            using var memoryStream = new MemoryStream();
            formatter.Serialize(memoryStream, serializable);

            return memoryStream.ToArray();
        }

        protected override object Deserialize(Type type, byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            var formatter = new BinaryFormatter();
            using var memoryStream = new MemoryStream(bytes);

            return formatter.Deserialize(memoryStream);
        }
    }
}