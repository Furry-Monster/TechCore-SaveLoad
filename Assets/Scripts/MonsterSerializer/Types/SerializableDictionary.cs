using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

namespace MonsterSerializer.Types
{
    [Serializable]
    public struct SerializableKVPair<TKey, TValue>
    {
        public TKey key;
        public TValue value;

        public SerializableKVPair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>,
        ISerializationCallbackReceiver, IXmlSerializable
    {
        [SerializeField] public List<SerializableKVPair<TKey, TValue>> pairs = new();

        [NonSerialized] private Dictionary<TKey, TValue> _dictionary = new(); // native dictionary copy for runtime
        [NonSerialized] private readonly object _syncRoot = new();

        public ICollection<TKey> Keys => _dictionary.Keys;
        public ICollection<TValue> Values => _dictionary.Values;
        public int Count => _dictionary.Count;
        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                if (key == null) throw new ArgumentNullException(nameof(key));
                _dictionary[key] = value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.TryGetValue(item.Key, out var value) &&
                   EqualityComparer<TValue>.Default.Equals(value,
                       item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Contains(item) && Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void OnBeforeSerialize()
        {
            lock (_syncRoot)
            {
                pairs.Clear();
                pairs.Capacity = _dictionary.Count;
                foreach (var pair in _dictionary) pairs.Add(new SerializableKVPair<TKey, TValue>(pair.Key, pair.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            lock (_syncRoot)
            {
                _dictionary.Clear();
                foreach (var pair in pairs)
                {
                    if (pair.key == null)
                    {
                        Debug.LogWarning("Null key found in serialized data. Skipping...");
                        continue;
                    }

                    try
                    {
                        _dictionary[pair.key] = pair.value;
                    }
                    catch (ArgumentException ex)
                    {
                        Debug.LogWarning($"Failed to add key {pair.key}: {ex.Message}");
                    }
                }
            }
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return new Dictionary<TKey, TValue>(_dictionary);
        }

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            _dictionary.Clear();
            if (reader.IsEmptyElement)
                return;
            reader.Read();

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("Item");
                var keyStr = reader.ReadElementContentAsString("Key", "");
                var valueStr = reader.ReadElementContentAsString("Value", "");
                var key = (TKey)Convert.ChangeType(keyStr, typeof(TKey));
                var value = (TValue)Convert.ChangeType(valueStr, typeof(TValue));
                _dictionary[key] = value;
                reader.ReadEndElement();
                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var pair in _dictionary)
            {
                writer.WriteStartElement("Item");
                writer.WriteElementString("Key", pair.Key.ToString());
                writer.WriteElementString("Value", pair.Value?.ToString());
                writer.WriteEndElement();
            }
        }
    }
}