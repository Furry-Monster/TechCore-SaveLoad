using System;
using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public static class TypeRegistry
    {
        private static readonly Dictionary<Type, object> Adapters = new();

        public static void Initialize()
        {
            Register(new Vec2Adapter());
            Register(new Vec3Adapter());
            Register(new Vec4Adapter());
            Register(new QuaternionAdapter());
            Register(new ColorAdapter());
            Register(new RectAdapter());
            Register(new BoundsAdapter());
            Register(new Matrix4x4Adapter());
        }

        public static void Register<TSource, TTarget>(ITypeAdapter<TSource, TTarget> adapter)
        {
            if (!typeof(TTarget).IsSerializable)
                throw new NotSupportedException($"TTarget<{typeof(TTarget).FullName}> must be serializable");

            // 对泛型集合，递归检查泛型参数是否可以序列化
            if (typeof(TTarget).IsGenericEnumerable())
            {
                var uncheck = new Queue<Type>();
                uncheck.Enqueue(typeof(TTarget));
                while (uncheck.Count > 0)
                {
                    var currentType = uncheck.Dequeue();
                    if (!currentType.IsGenericEnumerable())
                        continue;

                    var args = currentType.GetGenericArguments();
                    var invalid = new List<Type>();
                    foreach (var arg in args)
                    {
                        if (arg.IsGenericEnumerable())
                            uncheck.Enqueue(arg);

                        else if (!arg.IsSerializable)
                            invalid.Add(arg);
                    }

                    if (invalid.Count > 0)
                        throw new NotSupportedException(
                            $"Generic Type <{currentType.FullName}> has non-serializable arguments :{invalid}");
                }
            }

            if (Adapters.ContainsKey(typeof(TSource)))
                Adapters[typeof(TSource)] = adapter;
            else
                Adapters.Add(typeof(TSource), adapter);
        }

        public static ITypeAdapter GetAdapter<TSource>(TSource type) =>
            Adapters.TryGetValue(typeof(TSource), out var adapter)
                ? (ITypeAdapter)adapter
                : null;

        public static ITypeAdapter<TSource, TTarget> GetAdapter<TSource, TTarget>() =>
            Adapters.TryGetValue(typeof(TSource), out var adapter)
                ? (ITypeAdapter<TSource, TTarget>)adapter
                : null;

        public static bool HasAdapter(Type sourceType) => Adapters.ContainsKey(sourceType);

        public static object AdaptToSerializable(object obj)
        {
            throw new NotImplementedException();
        }

        public static TTarget AdaptToSerializable<TSource, TTarget>(TSource source)
        {
            var adapter = GetAdapter<TSource, TTarget>();
            if (adapter == null || source == null)
                return default;

            return adapter.ConvertToSerializable(source);
        }

        public static object AdaptFromSerializable(object obj)
        {
            throw new NotImplementedException();
        }

        public static TSource AdaptFromSerializable<TSource, TTarget>(TTarget target)
        {
            var adapter = GetAdapter<TSource, TTarget>();
            if (adapter == null || target == null)
                return default;

            return adapter.ConvertFromSerializable(target);
        }
    }
}