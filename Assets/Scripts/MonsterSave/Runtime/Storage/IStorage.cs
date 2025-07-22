using JetBrains.Annotations;

namespace MonsterSave.Runtime
{
    public interface IStorage
    {
        public IStorageMedia StorageMedia { get; }

        void UpdateConfig(MonsterSaveConfig config = null);
        void SaveText([NotNull] string key, string data);
        void SaveBinary([NotNull] string key, byte[] data);
        string LoadText([NotNull] string key);
        byte[] LoadBinary([NotNull] string key);
        bool Sync();
    }
}