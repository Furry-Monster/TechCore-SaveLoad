using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace MonsterSave.Runtime
{
    public class TypeRegistry
    {
        private static readonly Dictionary<Type, object> Adapters = new();

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