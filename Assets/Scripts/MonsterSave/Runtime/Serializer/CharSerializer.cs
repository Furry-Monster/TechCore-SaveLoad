using System;
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

            if (!obj.GetType().IsSerializable)
                throw new InvalidCastException($"{obj.GetType().FullName} is not [Serializable].");

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
            if (obj is string || type.IsValueType)
                return SerializeHandler(obj);

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

                // 处理未注册的数据结构
                if (type.IsGenericEnumerable())
                {
                    var args = type.GetGenericArguments();
                    var invalid = new List<Type>();
                    foreach (var arg in args)
                    {
                        if (arg.IsGenericEnumerable())
                            throw new NotSupportedException(
                                $"Generic Type <{type.FullName}> has non-serializable arguments :{arg.FullName}");
                        if (!arg.IsSerializable)
                            invalid.Add(arg);
                    }

                    if (invalid.Count > 0)
                        throw new NotSupportedException(
                            $"Generic Type <{type.FullName}> has non-serializable arguments :{invalid}");

                    // TODO:如果所有参数都可序列化，用默认方式处理数据结构
                    throw new NotImplementedException();
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
            if (!type.IsSerializable)
                throw new InvalidCastException($"{type.FullName} is not [Serializable].");
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

                // 然后再采用默认处理
                if (type.IsGenericEnumerable())
                {
                    var args = type.GetGenericArguments();
                    var invalid = new List<Type>();
                    foreach (var arg in args)
                    {
                        if (arg.IsGenericEnumerable())
                            throw new NotSupportedException(
                                $"Generic Type <{type.FullName}> has non-serializable arguments :{arg.FullName}");
                        if (!arg.IsSerializable)
                            invalid.Add(arg);
                    }

                    if (invalid.Count > 0)
                        throw new NotSupportedException(
                            $"Generic Type <{type.FullName}> has non-serializable arguments :{invalid}");

                    // TODO:如果所有参数都可序列化，用默认方式处理数据结构
                    throw new NotImplementedException();
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

            if (typeof(T).IsSerializable)
                throw new InvalidCastException($"{typeof(T).FullName} is not [Serializable].");

            var content = RecursiveSerialize(obj);
            if (content == null)
                return null;

            return Encoding.UTF8.GetBytes(content);
        }

        public T Deserialize<T>(byte[] data)
        {
            if (data == null || data.Length == 0)
                return default;
            if (!typeof(T).IsSerializable)
                throw new InvalidCastException($"{typeof(T).FullName} is not [Serializable].");


            var content = Encoding.UTF8.GetString(data, 0, data.Length);
            return (T)RecursiveDeserialize(typeof(T), content);
        }

        protected abstract string SerializeHandler(object serializable);
        protected abstract object DeserializeHandler(Type type, string content);
    }
}