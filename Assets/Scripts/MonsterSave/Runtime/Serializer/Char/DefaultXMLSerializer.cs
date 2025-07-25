using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MonsterSave.Runtime
{
    /// <summary>
    /// 默认XML序列化器，使用System.Xml
    /// </summary>
    public class DefaultXMLSerializer : CharSerializer
    {
        protected override string SerializeHandler(object serializable)
        {
            if (serializable == null || !serializable.GetType().IsSerializable)
                return null;

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

        protected override object DeserializeHandler(Type type, string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return null;

            var serializer = new XmlSerializer(type);
            using var stringReader = new StringReader(xml);
            return serializer.Deserialize(stringReader);
        }
    }
}