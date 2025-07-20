namespace MonsterSave.Runtime
{
    public interface IKVStorage
    {
        void SaveText(string key, string data);
        void SaveBinary(string key, byte[] data);
        string LoadText(string key);
        byte[] LoadBinary(string key);
    }
}