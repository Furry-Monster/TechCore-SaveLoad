using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public interface IStorageMedia
    {
        void SaveString(Dictionary<string, string> data);
        void SaveBytes(Dictionary<string, byte[]> data);
        Dictionary<string, string> LoadString();
        Dictionary<string, byte[]> LoadBytes();
    }
}