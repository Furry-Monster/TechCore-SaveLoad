using System;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

namespace MonsterSave.Runtime
{
    public static class TypeRegistry
    {
        private static readonly Dictionary<Type, ITypeAdapter> Adapters = new();

        public static void Initialize()
        {
            // Register default adapters
            Register(new BoundsAdapter());
            Register(new ColorAdapter());
            Register(new Matrix4x4Adapter());
            Register(new QuaternionAdapter());
            Register(new RectAdapter());
            Register(new Vec2Adapter());
            Register(new Vec3Adapter());
            Register(new Vec4Adapter());

            // Register default collection adapters
            Register(new ListAdapter());
            Register(new DictionaryAdapter());
        }

        public static void Register(ITypeAdapter adapter)
        {
            if (!adapter.TargetType.IsSerializable)
                throw new NotSupportedException($"Target type {adapter.TargetType.FullName} must be serializable");

            if (Adapters.ContainsKey(adapter.SourceType))
                Adapters[adapter.SourceType] = adapter;
            else
                Adapters.Add(adapter.SourceType, adapter);
        }

        public static void Register<TSource, TTarget>(IValueAdapter<TSource, TTarget> valueAdapter)
        {
            if (!typeof(TTarget).IsSerializable)
                throw new NotSupportedException($"TTarget<{typeof(TTarget).FullName}> must be serializable");

            if (Adapters.ContainsKey(typeof(TSource)))
                Adapters[typeof(TSource)] = valueAdapter;
            else
                Adapters.Add(typeof(TSource), valueAdapter);
        }

        public static ITypeAdapter GetAdapter(Type type)
        {
            // 首先直接查找具体类型
            if (Adapters.TryGetValue(type, out var adapter))
                return (ITypeAdapter)adapter;

            // 泛型类型，尝试用开放泛型类型查找
            if (type.IsGenericType)
            {
                var genericTypeDef = type.GetGenericTypeDefinition();
                if (Adapters.TryGetValue(genericTypeDef, out adapter))
                    return (ITypeAdapter)adapter;
            }

            // 支持IEnumerable<T>等接口
            foreach (var iface in type.GetInterfaces())
            {
                if (iface.IsGenericType)
                {
                    var genericTypeDef = iface.GetGenericTypeDefinition();
                    if (Adapters.TryGetValue(genericTypeDef, out adapter))
                        return (ITypeAdapter)adapter;
                }
            }

            return null;
        }

        public static IValueAdapter<TSource, TTarget> GetAdapter<TSource, TTarget>()
        {
            // 1. 直接查找具体类型
            if (Adapters.TryGetValue(typeof(TSource), out var adapter))
                return adapter as IValueAdapter<TSource, TTarget>;

            // 2. 泛型类型，尝试用开放泛型类型查找
            var sourceType = typeof(TSource);
            if (sourceType.IsGenericType)
            {
                var genericTypeDef = sourceType.GetGenericTypeDefinition();
                if (Adapters.TryGetValue(genericTypeDef, out adapter))
                    return adapter as IValueAdapter<TSource, TTarget>;
            }

            // 3. 支持IEnumerable<T>等接口
            foreach (var iface in sourceType.GetInterfaces())
            {
                if (iface.IsGenericType)
                {
                    var genericTypeDef = iface.GetGenericTypeDefinition();
                    if (Adapters.TryGetValue(genericTypeDef, out adapter))
                        return adapter as IValueAdapter<TSource, TTarget>;
                }
            }

            return null;
        }

        public static bool HasAdapter(Type sourceType)
        {
            // 直接查找具体类型
            if (Adapters.ContainsKey(sourceType))
                return true;

            // 泛型类型，尝试用开放泛型类型查找
            if (sourceType.IsGenericType)
            {
                var genericTypeDef = sourceType.GetGenericTypeDefinition();
                if (Adapters.ContainsKey(genericTypeDef))
                    return true;
            }

            // 支持IEnumerable<T>等接口
            foreach (var iface in sourceType.GetInterfaces())
            {
                if (iface.IsGenericType)
                {
                    var genericTypeDef = iface.GetGenericTypeDefinition();
                    if (Adapters.ContainsKey(genericTypeDef))
                        return true;
                }
            }

            return false;
        }

        public static bool HasAdapter<TSource, TTarget>()
        {
            // 1. 直接查找具体类型
            if (Adapters.ContainsKey(typeof(TSource)))
                return true;

            // 2. 泛型类型，尝试用开放泛型类型查找
            var sourceType = typeof(TSource);
            if (sourceType.IsGenericType)
            {
                var genericTypeDef = sourceType.GetGenericTypeDefinition();
                if (Adapters.ContainsKey(genericTypeDef))
                    return true;
            }

            // 3. 支持IEnumerable<T>等接口
            foreach (var iface in sourceType.GetInterfaces())
            {
                if (iface.IsGenericType)
                {
                    var genericTypeDef = iface.GetGenericTypeDefinition();
                    if (Adapters.ContainsKey(genericTypeDef))
                        return true;
                }
            }
            return false;
        }
    }
}