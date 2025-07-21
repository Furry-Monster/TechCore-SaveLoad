namespace MonsterSave.Runtime
{
    public interface ITypeAdapter<TSource, TTarget>
    {
        TTarget ConvertToSerializable(TSource native);
        TSource ConvertFromSerializable(TTarget serializable);
    }
}