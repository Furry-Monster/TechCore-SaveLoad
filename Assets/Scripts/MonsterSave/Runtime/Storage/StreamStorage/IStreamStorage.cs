namespace MonsterSave.Runtime
{
    public interface IStreamStorage
    {
        void SaveText(string data);
        void SaveBinary(byte[] data);
        string LoadText();
        byte[] LoadBinary();
    }
}