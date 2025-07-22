namespace MonsterSave.Runtime
{
    public interface IStorage
    {
        void Save(object data);
        object Load();
        T Load<T>();
        bool Sync();
    }
}