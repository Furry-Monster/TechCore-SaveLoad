using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class PlayerPrefBackend : IStorageBackend
    {
        // 将byte[]转为Base64字符串存储
        public void Write(string key, byte[] data)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var base64 = Convert.ToBase64String(data);
            PlayerPrefs.SetString(key, base64);
            PlayerPrefs.Save();
        }

        // 读取Base64字符串并转回byte[]
        public byte[] Read(string key)
        {
            if (!PlayerPrefs.HasKey(key))
                return null;

            var base64 = PlayerPrefs.GetString(key, null);
            if (string.IsNullOrEmpty(base64))
                return null;

            return Convert.FromBase64String(base64);
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }

        public bool HasKey(string key) => PlayerPrefs.HasKey(key);

        public void WriteAll(Dictionary<string, byte[]> allData) =>
            throw new NotSupportedException("PlayerPrefs does not support WriteAll.");

        public Dictionary<string, byte[]> ReadAll() =>
            throw new NotSupportedException("PlayerPrefs does not support ReadAll.");
    }
}