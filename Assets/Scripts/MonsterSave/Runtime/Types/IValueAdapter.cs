using System;

namespace MonsterSave.Runtime
{
    public interface IValueAdapter<TSource, TTarget> : ITypeAdapter
    {
        Type ITypeAdapter.SourceType => typeof(TSource);
        Type ITypeAdapter.TargetType => typeof(TTarget);

        object ITypeAdapter.ConvertToSerializable(object native)
        {
            return ConvertToSerializable((TSource)native);
        }

        object ITypeAdapter.ConvertFromSerializable(object serializable)
        {
            return ConvertFromSerializable((TTarget)serializable);
        }

        TTarget ConvertToSerializable(TSource native);
        TSource ConvertFromSerializable(TTarget serializable);
    }
}