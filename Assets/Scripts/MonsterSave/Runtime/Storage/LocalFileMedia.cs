using System;
using System.IO;

namespace MonsterSave.Runtime
{
    // public class LocalFileMedia : IStorageMedia
    // {
    //     // 可序列化字典只在这个类中使用
    //     [Serializable]
    //     private class SerializableDictionary<TKey, TValue>
    //     {
    //         [SerializeField] private List<TKey> keys = new();
    //         [SerializeField] private List<TValue> values = new();
    //
    //         public SerializableDictionary(Dictionary<TKey, TValue> dict)
    //         {
    //             foreach (var kvp in dict)
    //             {
    //                 keys.Add(kvp.Key);
    //                 values.Add(kvp.Value);
    //             }
    //         }
    //
    //         public Dictionary<TKey, TValue> ToDictionary()
    //         {
    //             var dict = new Dictionary<TKey, TValue>();
    //             for (var i = 0; i < keys.Count && i < values.Count; i++)
    //                 dict[keys[i]] = values[i];
    //             return dict;
    //         }
    //     }
    //
    //     private readonly StorageCore _storageCore;
    //
    //     public LocalFileMedia(StorageCore storageCore)
    //     {
    //         _storageCore = storageCore;
    //     }
    //
    //     public void Save(Dictionary<string, string> data)
    //     {
    //         string processed;
    //
    //         var directory = Path.GetDirectoryName(_storageCore.Config.storagePath);
    //         if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
    //             Directory.CreateDirectory(directory);
    //
    //
    //         switch (_storageCore.Config.format)
    //         {
    //             case Format.JSON:
    //             {
    //                 var serializableDict = new SerializableDictionary<string, string>(data);
    //                 processed = JsonUtility.ToJson(serializableDict, true);
    //                 break;
    //             }
    //             case Format.XML:
    //             {
    //                 var serializer = new XmlSerializer(data.GetType());
    //                 using var stringWriter = new StringWriter();
    //                 using var xmlWriter = XmlWriter.Create(stringWriter,
    //                     new XmlWriterSettings
    //                     {
    //                         Indent = true
    //                     });
    //                 serializer.Serialize(xmlWriter, data);
    //                 processed = stringWriter.ToString();
    //                 break;
    //             }
    //             case Format.Binary:
    //             default:
    //                 throw new ArgumentOutOfRangeException();
    //         }
    //
    //         File.WriteAllText(_storageCore.Config.storagePath, processed);
    //     }
    //
    //     public void Save(Dictionary<string, byte[]> data)
    //     {
    //         var filePath = _storageCore.Config.storagePath;
    //
    //         var directory = Path.GetDirectoryName(filePath);
    //         if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
    //             Directory.CreateDirectory(directory);
    //
    //         switch (_storageCore.Config.format)
    //         {
    //             case Format.Binary:
    //             {
    //                 using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
    //                 using var writer = new BinaryWriter(stream);
    //                 writer.Write(data.Count);
    //                 foreach (var kvp in data)
    //                 {
    //                     writer.Write(kvp.Key);
    //                     writer.Write(kvp.Value.Length);
    //                     writer.Write(kvp.Value);
    //                 }
    //
    //                 break;
    //             }
    //             case Format.JSON:
    //             case Format.XML:
    //             default:
    //                 throw new ArgumentOutOfRangeException();
    //         }
    //     }
    //
    //     public Dictionary<string, string> LoadString()
    //     {
    //         var filePath = _storageCore.Config.storagePath;
    //         if (!File.Exists(filePath))
    //             return new Dictionary<string, string>();
    //
    //         var content = File.ReadAllText(filePath);
    //         if (string.IsNullOrEmpty(content))
    //             return new Dictionary<string, string>();
    //
    //         switch (_storageCore.Config.format)
    //         {
    //             case Format.JSON:
    //             {
    //                 var serializableDict = JsonUtility.FromJson<SerializableDictionary<string, string>>(content);
    //                 return serializableDict.ToDictionary();
    //             }
    //             case Format.XML:
    //             {
    //                 using var stringReader = new StringReader(content);
    //                 var serializer = new XmlSerializer(typeof(Dictionary<string, string>));
    //                 return (Dictionary<string, string>)serializer.Deserialize(stringReader);
    //             }
    //             case Format.Binary:
    //             default:
    //                 throw new ArgumentOutOfRangeException();
    //         }
    //     }
    //
    //     public Dictionary<string, byte[]> LoadBytes()
    //     {
    //         var filePath = _storageCore.Config.storagePath;
    //         if (!File.Exists(filePath))
    //             return new Dictionary<string, byte[]>();
    //
    //         switch (_storageCore.Config.format)
    //         {
    //             case Format.Binary:
    //             {
    //                 using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
    //                 using var reader = new BinaryReader(stream);
    //
    //                 var count = reader.ReadInt32();
    //                 var data = new Dictionary<string, byte[]>(count);
    //                 for (var i = 0; i < count; i++)
    //                 {
    //                     var key = reader.ReadString();
    //                     var length = reader.ReadInt32();
    //                     var value = reader.ReadBytes(length);
    //                     data[key] = value;
    //                 }
    //
    //                 return data;
    //             }
    //             case Format.JSON:
    //             case Format.XML:
    //             default:
    //                 return new Dictionary<string, byte[]>();
    //         }
    //     }
    // }

    public class LocalFileMedia : IStorageMedia
    {
        private readonly string _path;

        public LocalFileMedia(StorageCore storageCore)
        {
            _path = storageCore.Config.storagePath;
        }

        public void WriteAllText(string content)
        {
            EnsureDirectory();
            File.WriteAllText(_path, content);
        }

        public string ReadAllText() => Exists() ? File.ReadAllText(_path) : null;

        public void WriteAllBytes(byte[] bytes)
        {
            EnsureDirectory();
            File.WriteAllBytes(_path, bytes);
        }

        public byte[] ReadAllBytes() => Exists() ? File.ReadAllBytes(_path) : null;

        public void Delete()
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
        }

        public bool Exists() => File.Exists(_path);

        private void EnsureDirectory()
        {
            var directory = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(directory))
                throw new Exception("Can't get directory path from the given path string.");

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
    }
}