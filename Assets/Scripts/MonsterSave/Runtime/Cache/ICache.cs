namespace MonsterSave.Runtime
{
    public interface ICache
    {
        object Get(string key);
        void Set(string key, object value);
        bool Remove(string key);
        void Clear();
        int Count { get; }
        int Capacity { get; set; }
    }
}