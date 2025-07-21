using JetBrains.Annotations;
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

        public static void Register<TSource, TTarget>([NotNull] ITypeAdapter<TSource, TTarget> adapter)
        {
            if (Adapters.ContainsKey(typeof(TSource)))
                Adapters[typeof(TSource)] = adapter;
            else
                Adapters.Add(typeof(TSource), adapter);
        }

        public static ITypeAdapter<TSource, TTarget> GetAdapter<TSource, TTarget>()
        {
            return Adapters.TryGetValue(typeof(TSource), out var adapter)
                ? (ITypeAdapter<TSource, TTarget>)adapter
                : null;
        }
    }
}