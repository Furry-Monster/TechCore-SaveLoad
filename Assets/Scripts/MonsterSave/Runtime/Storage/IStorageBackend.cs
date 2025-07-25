using System.Collections.Generic;

namespace MonsterSave.Runtime
{
    public interface IStorageBackend
    {
        // 低层的读写接口
        void Write(string key, byte[] data);
        byte[] Read(string key);
        void Delete(string key);
        bool HasKey(string key);

        // 可选：全量读写
        void WriteAll(Dictionary<string, byte[]> allData);
        Dictionary<string, byte[]> ReadAll();
    }
}