using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MonsterSave.Runtime
{
    /// <summary>
    /// 默认XML序列化器，使用System.Xml
    /// </summary>
    public class DefaultXMLSerializer : IXMLSerializer
    {
        public string Serialize(object serializable)
        {
            var serializer = new XmlSerializer(serializable.GetType());
            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter,
                new XmlWriterSettings
                {
                    Indent = true
                });
            serializer.Serialize(xmlWriter, serializable);

            return stringWriter.ToString();
        }

        public object Deserialize(Type type, string xml)
        {
            var serializer = new XmlSerializer(type);
            using var stringReader = new StringReader(xml);
            return serializer.Deserialize(stringReader);
        }
    }
}