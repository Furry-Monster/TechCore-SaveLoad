using System;

namespace MonsterSave.Runtime
{
    public interface ITypeAdapter<TSource, TTarget> : ITypeAdapter
    {
        Type ITypeAdapter.SourceType => typeof(TSource);
        Type ITypeAdapter.TargetType => typeof(TTarget);

        object ITypeAdapter.ConvertToSerializable(object native) =>
            ConvertToSerializable((TSource)native);

        object ITypeAdapter.ConvertFromSerializable(object serializable) =>
            ConvertFromSerializable((TTarget)serializable);

        TTarget ConvertToSerializable(TSource native);
        TSource ConvertFromSerializable(TTarget serializable);
    }

    public interface ITypeAdapter
    {
        public Type SourceType { get; }
        public Type TargetType { get; }
        object ConvertToSerializable(object native);
        object ConvertFromSerializable(object serializable);
    }
}