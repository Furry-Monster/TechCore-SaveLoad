using System.Collections.Generic;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class CloudBackend : IStorageBackend
    {
        public void Write(string key, byte[] data)
        {
            Debug.Log($"[CloudBackend] Upload {key} (size={data?.Length})");
            // TODO: 接入实际云存储SDK
        }

        public byte[] Read(string key)
        {
            Debug.Log($"[CloudBackend] Download {key}");
            // TODO: 接入实际云存储SDK
            return null;
        }

        public void Delete(string key)
        {
            Debug.Log($"[CloudBackend] Delete {key}");
            // TODO: 接入实际云存储SDK
        }

        public bool HasKey(string key)
        {
            Debug.Log($"[CloudBackend] Check Exists {key}");
            // TODO: 接入实际云存储SDK
            return false;
        }

        public void WriteAll(Dictionary<string, byte[]> allData)
        {
            foreach (var kv in allData)
                Write(kv.Key, kv.Value);
        }

        public Dictionary<string, byte[]> ReadAll()
        {
            Debug.Log("[CloudBackend] ReadAll not implemented.");
            // TODO: 接入实际云存储SDK
            return new Dictionary<string, byte[]>();
        }
    }
}