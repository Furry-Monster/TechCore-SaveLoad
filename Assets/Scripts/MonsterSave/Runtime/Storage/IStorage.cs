namespace MonsterSave.Runtime
{
    public interface IStorage
    {
        void Save(string key, object data);
        object Load(string key);
        T Load<T>(string key);
        bool Sync(bool toDisk = true);
    }
}