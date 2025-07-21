using JetBrains.Annotations;

namespace MonsterSave.Runtime
{
    public interface IStorage
    {
        public IKVStorage KVStorage { get; set; }
        public IStreamStorage StreamStorage { get; set; }

        void SaveText(string data);
        void SaveText([NotNull] string key, string data);
        void SaveBinary(byte[] data);
        void SaveBinary([NotNull] string key, byte[] data);

        string LoadText();
        string LoadText([NotNull] string key);
        byte[] LoadBinary();
        byte[] LoadBinary([NotNull] string key);

        void SelectStoragePath(string path);
    }
}