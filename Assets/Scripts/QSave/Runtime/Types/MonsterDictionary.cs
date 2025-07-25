using System;
using System.Collections;
using System.Collections.Generic;

namespace QSave.Runtime
{
    public class DictionaryAdapter : IDictionaryAdapter
    {
        public Type SourceType => typeof(Dictionary<,>);
        public Type TargetType => typeof(Dictionary<,>);


        public object ConvertToSerializable(object native)
        {
            if (native == null)
                return null;
            var type = native.GetType();
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Dictionary<,>))
                throw new ArgumentException("Only Dictionary<TKey, TValue> is supported.");

            var keyType = type.GetGenericArguments()[0];
            var keyAdaptedType = TypeMgr.GetAdapter(keyType)?.TargetType ?? keyType; // 获取适配器的目标类型
            var valueType = type.GetGenericArguments()[1];
            var valueAdaptedType = TypeMgr.GetAdapter(valueType)?.TargetType ?? valueType; // 获取适配器的目标类型

            // create a serializable dictionary type
            var serializableDictType = typeof(Dictionary<,>).MakeGenericType(keyAdaptedType, valueAdaptedType);
            var serializableDict = (IDictionary)Activator.CreateInstance(serializableDictType);

            // get adapters for key and value types
            var adapterKey = TypeMgr.HasAdapter(keyType)
                ? TypeMgr.GetAdapter(keyType)
                : null;
            var adapterValue = TypeMgr.HasAdapter(valueType)
                ? TypeMgr.GetAdapter(valueType)
                : null;

            // NOTE: 属于非可序列化类型,并且作为字典的键或值类型,
            // 则必须手动注册适配器,否则抛出异常
            if (adapterKey == null && !keyType.IsSerializable)
                throw new NotSupportedException($"Key type {keyType.FullName} is not serializable.");
            if (adapterValue == null && !valueType.IsSerializable)
                throw new NotSupportedException($"Value type {valueType.FullName} is not serializable.");

            // iterate through the native dictionary and convert keys and values
            foreach (DictionaryEntry entry in (IDictionary)native)
            {
                // 如果键或值类型有适配器,则使用适配器转换,否则直接使用原始类型
                var key = adapterKey != null
                    ? adapterKey.ConvertToSerializable(entry.Key)
                    : entry.Key;
                var value = adapterValue != null
                    ? adapterValue.ConvertToSerializable(entry.Value)
                    : entry.Value;
                serializableDict.Add(key, value);
            }

            return serializableDict;
        }

        public object ConvertFromSerializable(object serializable)
        {
            if (serializable == null)
                return null;
            var type = serializable.GetType();
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Dictionary<,>))
                throw new ArgumentException("Only Dictionary<TKey, TValue> is supported.");

            var keyType = type.GetGenericArguments()[0];
            var keyAdaptedType = TypeMgr.GetAdapter(keyType)?.TargetType ?? keyType;
            var valueType = type.GetGenericArguments()[1];
            var valueAdaptedType = TypeMgr.GetAdapter(valueType)?.TargetType ?? valueType;

            var nativeDictType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            var nativeDict = (IDictionary)Activator.CreateInstance(nativeDictType);

            var adapterKey = TypeMgr.HasAdapter(keyType)
                ? TypeMgr.GetAdapter(keyType)
                : null;
            var adapterValue = TypeMgr.HasAdapter(valueType)
                ? TypeMgr.GetAdapter(valueType)
                : null;

            // NOTE: 属于非可序列化类型,并且作为字典的键或值类型,
            // 则必须手动注册适配器,否则抛出异常
            if (adapterKey == null && !keyType.IsSerializable)
                throw new NotSupportedException($"Key type {keyType.FullName} is not serializable.");
            if (adapterValue == null && !valueType.IsSerializable)
                throw new NotSupportedException($"Value type {valueType.FullName} is not serializable.");

            foreach (DictionaryEntry entry in (IDictionary)serializable)
            {
                var key = adapterKey != null
                    ? adapterKey.ConvertFromSerializable(entry.Key)
                    : entry.Key;
                var value = adapterValue != null
                    ? adapterValue.ConvertFromSerializable(entry.Value)
                    : entry.Value;
                nativeDict.Add(key, value);
            }

            return nativeDict;
        }
    }
}