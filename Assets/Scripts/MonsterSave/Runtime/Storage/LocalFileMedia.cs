using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class LocalFileMedia : IStorageMedia
    {
        // 可序列化字典只在这个类中使用
        [Serializable]
        private class SerializableDictionary<TKey, TValue>
        {
            [SerializeField] private List<TKey> keys = new();
            [SerializeField] private List<TValue> values = new();

            public SerializableDictionary(Dictionary<TKey, TValue> dict)
            {
                foreach (var kvp in dict)
                {
                    keys.Add(kvp.Key);
                    values.Add(kvp.Value);
                }
            }

            public Dictionary<TKey, TValue> ToDictionary()
            {
                var dict = new Dictionary<TKey, TValue>();
                for (var i = 0; i < keys.Count && i < values.Count; i++)
                    dict[keys[i]] = values[i];
                return dict;
            }
        }

        private readonly Storage _storage;

        public LocalFileMedia(Storage storage)
        {
            _storage = storage;
        }

        public void SaveString(Dictionary<string, string> data)
        {
            string processed;

            var directory = Path.GetDirectoryName(_storage.Config.storagePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);


            switch (_storage.Config.format)
            {
                case Format.JSON:
                {
                    var serializableDict = new SerializableDictionary<string, string>(data);
                    processed = JsonUtility.ToJson(serializableDict, true);
                    break;
                }
                case Format.XML:
                {
                    var serializer = new XmlSerializer(data.GetType());
                    using var stringWriter = new StringWriter();
                    using var xmlWriter = XmlWriter.Create(stringWriter,
                        new XmlWriterSettings
                        {
                            Indent = true
                        });
                    serializer.Serialize(xmlWriter, data);
                    processed = stringWriter.ToString();
                    break;
                }
                case Format.Binary:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            File.WriteAllText(_storage.Config.storagePath, processed);
        }

        public void SaveBytes(Dictionary<string, byte[]> data)
        {
            var filePath = _storage.Config.storagePath;

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            switch (_storage.Config.format)
            {
                case Format.Binary:
                {
                    using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    using var writer = new BinaryWriter(stream);
                    writer.Write(data.Count);
                    foreach (var kvp in data)
                    {
                        writer.Write(kvp.Key);
                        writer.Write(kvp.Value.Length);
                        writer.Write(kvp.Value);
                    }

                    break;
                }
                case Format.JSON:
                case Format.XML:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Dictionary<string, string> LoadString()
        {
            var filePath = _storage.Config.storagePath;
            if (!File.Exists(filePath))
                return new Dictionary<string, string>();

            var content = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(content))
                return new Dictionary<string, string>();

            switch (_storage.Config.format)
            {
                case Format.JSON:
                {
                    var serializableDict = JsonUtility.FromJson<SerializableDictionary<string, string>>(content);
                    return serializableDict.ToDictionary();
                }
                case Format.XML:
                {
                    using var stringReader = new StringReader(content);
                    var serializer = new XmlSerializer(typeof(Dictionary<string, string>));
                    return (Dictionary<string, string>)serializer.Deserialize(stringReader);
                }
                case Format.Binary:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Dictionary<string, byte[]> LoadBytes()
        {
            var filePath = _storage.Config.storagePath;
            if (!File.Exists(filePath))
                return new Dictionary<string, byte[]>();

            switch (_storage.Config.format)
            {
                case Format.Binary:
                {
                    using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    using var reader = new BinaryReader(stream);

                    var count = reader.ReadInt32();
                    var data = new Dictionary<string, byte[]>(count);
                    for (var i = 0; i < count; i++)
                    {
                        var key = reader.ReadString();
                        var length = reader.ReadInt32();
                        var value = reader.ReadBytes(length);
                        data[key] = value;
                    }

                    return data;
                }
                case Format.JSON:
                case Format.XML:
                default:
                    return new Dictionary<string, byte[]>();
            }
        }
    }
}