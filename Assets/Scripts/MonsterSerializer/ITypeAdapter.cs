namespace MonsterSerializer
{
    public interface ITypeAdapter
    {
        public object Adapt(object raw);
        public object Restore(object baked);
    }
}