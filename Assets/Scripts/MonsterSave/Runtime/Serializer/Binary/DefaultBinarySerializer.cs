using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MonsterSave.Runtime
{
    public class DefaultBinarySerializer : IBinarySerializer
    {
        public byte[] Serialize(object serializable)
        {
            var formatter = new BinaryFormatter();
            using var memoryStream = new MemoryStream();
            formatter.Serialize(memoryStream, serializable);

            return memoryStream.ToArray();
        }

        public object Deserialize(Type type, byte[] bytes)
        {
            var formatter = new BinaryFormatter();
            using var memoryStream = new MemoryStream(bytes);

            return formatter.Deserialize(memoryStream);
        }
    }
}