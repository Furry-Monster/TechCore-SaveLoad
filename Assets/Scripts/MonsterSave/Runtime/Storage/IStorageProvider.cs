using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public interface IStorageProvider
    {
        void Set<T>(string key, T data);
        T Get<T>(string key, T defaultValue = default);
        void Delete(string key);
        bool Exist(string key);

        void SyncAll(Dictionary<string, object> allData);
        Dictionary<string, object> LoadAll();
    }
}