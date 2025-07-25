using System;
using System.Collections;
using System.Collections.Generic;

namespace QSave.Runtime
{
    public class ListAdapter : IEnumerableAdapter
    {
        public Type SourceType => typeof(List<>);
        public Type TargetType => typeof(List<>);

        public object ConvertToSerializable(object native)
        {
            if (native == null)
                return null;
            var type = native.GetType();
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(List<>))
                throw new ArgumentException("Only List<T> is supported.");

            var elemType = type.GetGenericArguments()[0];
            var adaptedElemType = TypeMgr.GetAdapter(elemType)?.TargetType ?? elemType; // 获取适配器的目标类型

            var serializableListType = typeof(List<>).MakeGenericType(adaptedElemType);
            var serializableList = (IList)Activator.CreateInstance(serializableListType);

            var adapter = TypeMgr.HasAdapter(elemType)
                ? TypeMgr.GetAdapter(elemType)
                : null;

            if (adapter == null && !elemType.IsSerializable)
                throw new NotSupportedException($"Element type {elemType.FullName} is not serializable.");

            foreach (var item in (IEnumerable)native)
            {
                var value = adapter != null
                    ? adapter.ConvertToSerializable(item)
                    : item; // 如果有适配器则使用适配器转换,否则直接使用原始类型
                serializableList.Add(value);
            }

            return serializableList;
        }

        public object ConvertFromSerializable(object serializable)
        {
            if (serializable == null)
                return null;
            var type = serializable.GetType();
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(List<>))
                throw new ArgumentException("Only List<T> is supported.");

            var elemType = type.GetGenericArguments()[0];
            var adaptedElemType = TypeMgr.GetAdapter(elemType)?.TargetType ?? elemType; // 获取适配器的目标类型

            var serializableListType = typeof(List<>).MakeGenericType(elemType);
            var nativeList = (IList)Activator.CreateInstance(serializableListType);

            var adapter = TypeMgr.HasAdapter(elemType)
                ? TypeMgr.GetAdapter(elemType)
                : null;

            if (adapter == null && !elemType.IsSerializable)
                throw new NotSupportedException($"Element type {elemType.FullName} is not serializable.");

            foreach (var item in (IEnumerable)serializable)
            {
                var value = adapter != null
                    ? adapter.ConvertFromSerializable(item)
                    : item; // 如果有适配器则使用适配器转换,否则直接使用原始类型
                nativeList.Add(value);
            }

            return nativeList;
        }
    }
}