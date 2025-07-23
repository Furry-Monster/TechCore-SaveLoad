using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterSave.Runtime
{
    public abstract class CharSerializer : ISerializer
    {
        public bool IsBinary => false;

        // interfaces impl
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
                return null;

            var type = obj.GetType();

            // 1.如果是字符串或者值类型，直接返回
            if (obj is string || type.IsValueType)
                return obj.ToString();

            // 2.如果不是可序列化对象，先检查TypeRegistry
            if (!type.IsSerializable)
            {
                if (TypeRegistry.HasAdapter(type))
                {
                    var adapted = TypeRegistry
                        .GetAdapter(type)
                        .ConvertToSerializable(obj);
                    return Serialize(adapted);
                }
            }

            // 3.如果是泛型集合，检查泛型参数是否可序列化
            if (!type.IsSerializable && type.IsGenericEnumerable())
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
                    throw new NotSupportedException($"Generic Type <{type.FullName}> has non-serializable arguments :{invalid}");

                // 如果所有参数都可序列化，直接序列化
                var adapted = TypeRegistry
                    .GetAdapter(type)
                    .ConvertToSerializable(obj);
                return Serialize(adapted);
            }

            // 4.如果是可序列化类，递归属性字段序列化
            if (type.IsSerializable && type.IsClass)
            {
                var properties = type.GetProperties();
                var fields = type.GetFields();
                var dictionary = new Dictionary<string, object>();

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

                return Serialize(dictionary);
            }

            return null;
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
            return Deserialize(type, content);
        }


        protected abstract string Serialize(object serializable);
        protected abstract object Deserialize(Type type, string content);
    }
}