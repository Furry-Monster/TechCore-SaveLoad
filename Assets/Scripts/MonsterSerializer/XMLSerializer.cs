using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MonsterSerializer
{
    public class XMLSerializer : ISerializer
    {
        public byte[] Serialize(object obj)
        {
            if (obj == null || !obj.GetType().IsSerializable)
                return null;

            var serializer = new XmlSerializer(obj.GetType());
            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter,
                new XmlWriterSettings
                {
                    Indent = true
                });
            serializer.Serialize(xmlWriter, obj);

            var xml = stringWriter.ToString();
            var bytes = Encoding.UTF8.GetBytes(xml);
            return bytes;
        }

        public object Deserialize(Type type, byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            var serializer = new XmlSerializer(type);
            var xml = Encoding.UTF8.GetString(bytes);
            using var stringReader = new StringReader(xml);
            return serializer.Deserialize(stringReader);
        }
    }
}