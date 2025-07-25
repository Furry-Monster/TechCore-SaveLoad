using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public abstract class BinarySerializer : ISerializer
    {
        public bool IsBinary => true;

        byte[] ISerializer.Serialize(object obj)
        {
            if (obj == null)
                return null;
            if (!obj.GetType().IsSerializable)
                throw new InvalidCastException($"{obj.GetType().FullName} is not [Serializable].");

            var content = RecursiveSerialize(obj);
            return content;
        }

        private byte[] RecursiveSerialize(object obj)
        {
            if (obj == null)
                return Array.Empty<byte>();

            var type = obj.GetType();

            // 1. 字符串或值类型，直接序列化
            if (obj is string || type.IsValueType)
                return SerializeHandler(obj);

            // 2. 非可序列化对象，尝试用TypeRegistry适配
            if (!type.IsSerializable)
            {
                if (TypeRegistry.HasAdapter(type))
                {
                    var adapted = TypeRegistry
                        .GetAdapter(type)
                        .ConvertToSerializable(obj);
                    return RecursiveSerialize(adapted);
                }

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
                            $"Generic Type <{type.FullName}> has non-serializable arguments :{string.Join(", ", invalid)}");

                    // TODO:同
                    throw new NotImplementedException();
                }
            }

            // 3. 可序列化类，递归序列化属性和字段
            if (type.IsSerializable)
            {
                if (type.IsClass)
                {
                    var properties = type.GetProperties();
                    var fields = type.GetFields();
                    var dict = new Dictionary<string, byte[]>();

                    foreach (var property in properties)
                    {
                        if (property.CanRead && property.CanWrite)
                        {
                            var value = property.GetValue(obj);
                            var serializedValue = RecursiveSerialize(value);
                            dict[property.Name] = serializedValue;
                        }
                    }

                    foreach (var field in fields)
                    {
                        if (field.IsPublic && !field.IsStatic)
                        {
                            var value = field.GetValue(obj);
                            var serializedValue = RecursiveSerialize(value);
                            dict[field.Name] = serializedValue;
                        }
                    }

                    return RecursiveSerialize(dict);
                }
            }

            return Array.Empty<byte>();
        }

        object ISerializer.Deserialize(Type type, byte[] data)
        {
            if (type == null)
                return null;
            if (!type.IsSerializable)
                throw new InvalidCastException($"{type.FullName} is not [Serializable].");
            if (data == null || data.Length == 0)
                return null;

            return RecursiveDeserialize(type, data);
        }

        private object RecursiveDeserialize(Type type, byte[] data)
        {
            if (data == null || data.Length == 0 || type == null)
                return null;

            if (type == typeof(string) || type.IsValueType)
                return DeserializeHandler(type, data);

            if (!type.IsSerializable)
            {
                if (TypeRegistry.HasAdapter(type))
                {
                    var adaptedType = TypeRegistry.GetAdapter(type).TargetType;
                    var adaptedValue = RecursiveDeserialize(adaptedType, data);
                    return TypeRegistry
                        .GetAdapter(type)
                        .ConvertFromSerializable(adaptedValue);
                }

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

            if (type.IsSerializable)
            {
                if (type.IsClass)
                {
                    var adaptedValue =
                        (Dictionary<string, byte[]>)RecursiveDeserialize(typeof(Dictionary<string, byte[]>), data);
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
            throw new NotImplementedException();
        }

        public T Deserialize<T>(byte[] data)
        {
            throw new NotImplementedException();
        }

        protected abstract byte[] SerializeHandler(object serializable);
        protected abstract object DeserializeHandler(Type type, byte[] bytes);
    }
}