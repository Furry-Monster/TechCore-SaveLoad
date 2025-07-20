using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    public class PlayerPrefsStorage : IKVStorage
    {
        public void SaveText(string key, string data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
            if (string.IsNullOrEmpty(data))
                Debug.LogWarning($"You're trying to save a null or empty string for key {key}");

            PlayerPrefs.SetString(key, data);
            PlayerPrefs.Save();
        }

        public void SaveBinary(string key, byte[] data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
            if (data == null || data.Length == 0)
            {
                Debug.LogWarning($"You're trying to save a null or empty string for key {key}");
                PlayerPrefs.SetString(key, "");
                PlayerPrefs.Save();
                return;
            }

            try
            {
                var base64String = Convert.ToBase64String(data);
                PlayerPrefs.SetString(key, base64String);
                PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save binary for key {key}: {e.Message}");
                throw;
            }
        }

        public string LoadText(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"Key {key} not found in PlayerPrefs storage");
                return null;
            }

            return PlayerPrefs.GetString(key);
        }

        public byte[] LoadBinary(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"Key {key} not found in PlayerPrefs storage");
                return null;
            }

            try
            {
                var base64String = PlayerPrefs.GetString(key);

                return string.IsNullOrEmpty(base64String)
                    ? Array.Empty<byte>()
                    : Convert.FromBase64String(base64String);
            }
            catch (FormatException e)
            {
                Debug.LogError($"Invalid base 64 string for key {key}: {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load binary for key {key}: {e.Message}");
                throw;
            }
        }
    }
}