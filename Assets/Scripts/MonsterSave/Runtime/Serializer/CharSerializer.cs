using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonsterSave.Runtime
{
    public abstract class CharSerializer : ISerializer
    {
        public bool IsBinary => false;

        byte[] ISerializer.Serialize(object obj)
        {
            if (obj == null)
                return null;

            var content = RecursiveSerialize(obj);
            if (content == null)
                return null;

            return Encoding.UTF8.GetBytes(content);
        }

        private string RecursiveSerialize(object obj)
        {
            if (obj == null)
                return string.Empty;

            var type = obj.GetType();

            // 1.如果是字符串或者值类型，直接返回(叶子节点)
            if (type == typeof(string) || type.IsValueType)
                return SerializeHandler(obj);

            // 1.5. 集合类型特殊处理
            // Dictionary<,>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var dict = (IDictionary)obj;
                var dictContent = new Dictionary<string, string>();
                foreach (var key in dict.Keys)
                {
                    var keyStr = RecursiveSerialize(key);
                    var valueStr = RecursiveSerialize(dict[key]);
                    dictContent[keyStr] = valueStr;
                }

                // 递归序列化为字符串
                return RecursiveSerialize(dictContent);
            }

            // List<T> 或数组
            if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
            {
                // 排除字符串本身
                var list = (IEnumerable)obj;
                var items = new List<string>();
                foreach (var item in list)
                    items.Add(RecursiveSerialize(item));

                // 递归序列化为字符串（可自定义格式）
                return RecursiveSerialize(items);
            }

            // 2.如果不是可序列化类型
            if (!type.IsSerializable)
            {
                if (TypeRegistry.HasAdapter(type))
                {
                    var adaptedValue = TypeRegistry
                        .GetAdapter(type)
                        .ConvertToSerializable(obj);
                    return RecursiveSerialize(adaptedValue);
                }
            }

            // 3.如果是可序列化类型
            if (type.IsSerializable)
            {
                if (type.IsClass)
                {
                    var properties = type.GetProperties();
                    var fields = type.GetFields();
                    var dictionary = new Dictionary<string, string>();

                    foreach (var property in properties)
                    {
                        if (property.CanRead && property.CanWrite)
                        {
                            var value = property.GetValue(obj);
                            var serializedValue = RecursiveSerialize(value);
                            if (serializedValue != null)
                                dictionary[property.Name] = serializedValue;
                        }
                    }

                    foreach (var field in fields)
                    {
                        if (field.IsPublic || field.IsStatic)
                        {
                            var value = field.GetValue(obj);
                            var serializedValue = RecursiveSerialize(value);
                            if (serializedValue != null)
                                dictionary[field.Name] = serializedValue;
                        }
                    }

                    return RecursiveSerialize(dictionary);
                }
            }

            return string.Empty;
        }

        object ISerializer.Deserialize(Type type, byte[] data)
        {
            if (type == null)
                return null;
            if (data == null || data.Length == 0)
                return null;

            var content = Encoding.UTF8.GetString(data, 0, data.Length);
            return RecursiveDeserialize(type, content);
        }


        private object RecursiveDeserialize(Type type, string content)
        {
            if (string.IsNullOrEmpty(content) || type == null)
                return null;

            // 1. 字符串或值类型，直接转换(叶子节点)
            if (type == typeof(string) || type.IsValueType)
                return DeserializeHandler(type, content);

            // 1.5. 集合类型处理
            // Dictionary<,>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var dictContent =
                    (Dictionary<string, string>)RecursiveDeserialize(typeof(Dictionary<string, string>), content);

                var keyType = type.GetGenericArguments()[0];
                var valueType = type.GetGenericArguments()[1];
                var dictType = type.GetGenericTypeDefinition().MakeGenericType(keyType, valueType);
                var dict = Activator.CreateInstance(dictType) as IDictionary;
                if (dict == null)
                    return null;

                foreach (var entry in dictContent)
                {
                    var key = RecursiveDeserialize(keyType, entry.Key);
                    var value = RecursiveDeserialize(valueType, entry.Value);
                    dict[key] = value;
                }

                return dict;
            }

            // Array<>或者 List<>
            if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
            {
                var items = (List<string>)RecursiveDeserialize(typeof(List<string>), content);

                var itemType = type.GetGenericArguments()[0];
                var listType = typeof(List<>).MakeGenericType(itemType);
                var list = Activator.CreateInstance(listType) as IList;
                if (list == null)
                    return null;

                foreach (var item in items)
                    list.Add(RecursiveDeserialize(itemType, item));

                return list;
            }

            // 2. 不可序列化类型
            if (!type.IsSerializable)
            {
                // 首先尝试转型
                if (TypeRegistry.HasAdapter(type))
                {
                    var adaptedType = TypeRegistry.GetAdapter(type).TargetType;
                    var adaptedValue = RecursiveDeserialize(adaptedType, content);
                    return TypeRegistry
                        .GetAdapter(type)
                        .ConvertFromSerializable(adaptedValue);
                }
            }

            // 3. 可序列化类型
            if (type.IsSerializable)
            {
                if (type.IsClass)
                {
                    var adaptedValue =
                        (Dictionary<string, string>)RecursiveDeserialize(typeof(Dictionary<string, string>), content);
                    if (adaptedValue == null)
                        return null;

                    var instance = Activator.CreateInstance(type);
                    var properties = type.GetProperties();
                    var fields = type.GetFields();

                    foreach (var property in properties)
                    {
                        if (property.CanRead && property.CanWrite)
                        {
                            if (adaptedValue.TryGetValue(property.Name, out var serializedValue))
                            {
                                var value = RecursiveDeserialize(property.GetType(), serializedValue);
                                property.SetValue(instance, value);
                            }
                        }
                    }

                    foreach (var field in fields)
                    {
                        if (field.IsPublic || field.IsStatic)
                        {
                            if (adaptedValue.TryGetValue(field.Name, out var serializedValue))
                            {
                                var value = RecursiveDeserialize(field.GetType(), serializedValue);
                                field.SetValue(instance, value);
                            }
                        }
                    }

                    return instance;
                }
            }

            return null;
        }

        public byte[] Serialize<T>(T obj)
        {
            if (obj == null)
                return null;

            var content = RecursiveSerialize(obj);
            if (content == null)
                return null;

            return Encoding.UTF8.GetBytes(content);
        }

        public T Deserialize<T>(byte[] data)
        {
            if (data == null || data.Length == 0)
                return default;

            var content = Encoding.UTF8.GetString(data, 0, data.Length);
            return (T)RecursiveDeserialize(typeof(T), content);
        }

        protected abstract string SerializeHandler(object serializable);
        protected abstract object DeserializeHandler(Type type, string content);
    }
}