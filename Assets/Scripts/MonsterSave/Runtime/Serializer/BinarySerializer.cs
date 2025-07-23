using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public abstract class BinarySerializer : ISerializer
    {
        public bool IsBinary => true;

        // interfaces impl
        byte[] ISerializer.Serialize(object obj)
        {
            if (obj == null)
                return null;
            if (!obj.GetType().IsSerializable)
                throw new InvalidCastException($"{obj.GetType().FullName} is not [Serializable].");

            return Serialize(obj);
        }

        private byte[] RecursiveSerialize(object obj)
        {
            if (obj == null)
                return null;

            var type = obj.GetType();

            // 1. 字符串或值类型，直接序列化
            if (obj is string || type.IsValueType)
                return Serialize(obj);

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
            }

            // 3. 泛型集合，递归序列化每个元素
            if (type.IsGenericEnumerable())
            {
                var args = type.GetGenericArguments();
                var invalid = new List<Type>();
                foreach (var arg in args)
                {
                    if (arg.IsGenericEnumerable())
                        throw new NotSupportedException($"Generic Type <{type.FullName}> has non-serializable arguments :{arg.FullName}");
                    if (!arg.IsSerializable)
                        invalid.Add(arg);
                }
                if (invalid.Count > 0)
                    throw new NotSupportedException($"Generic Type <{type.FullName}> has non-serializable arguments :{string.Join(", ", invalid)}");

                var adapted = TypeRegistry
                    .GetAdapter(type)
                    .ConvertToSerializable(obj);
                return Serialize(adapted);
            }

            // 4. 可序列化类，递归序列化属性和字段
            if (type.IsSerializable && type.IsClass)
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

                return Serialize(dict);
            }

            throw new NotSupportedException($"Type {type.FullName} is not supported for binary serialization.");
        }

        object ISerializer.Deserialize(Type type, byte[] data)
        {
            if (type == null)
                return null;
            if (!type.IsSerializable)
                throw new InvalidCastException($"{type.FullName} is not [Serializable].");
            if (data == null || data.Length == 0)
                return null;

            return Deserialize(type, data);
        }

        protected abstract byte[] Serialize(object serializable);
        protected abstract object Deserialize(Type type, byte[] bytes);
    }
}