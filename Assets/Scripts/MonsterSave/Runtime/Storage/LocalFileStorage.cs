using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Device;

namespace MonsterSave.Runtime
{
    public class LocalFileStorage : IStorage
    {
        public string StoragePath { get; private set; }

        public LocalFileStorage(string fileName = "save.monster")
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or whitespace.", nameof(fileName));
            StoragePath = Path.Combine(Application.persistentDataPath, fileName);
        }

        public void SelectPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or whitespace.", nameof(path));

            StoragePath = Path.IsPathRooted(path)
                ? path
                : Path.Combine(Application.persistentDataPath, path);
        }

        /// <summary>
        /// 保存文本数据
        /// </summary>
        public void SaveText(string key, string data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                File.WriteAllText(StoragePath, data);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to save text to {StoragePath}", ex);
            }
        }

        /// <summary>
        /// 保存二进制数据
        /// </summary>
        public void SaveBinary(string key, byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                File.WriteAllBytes(StoragePath, data);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to save binary data to {StoragePath}", ex);
            }
        }

        /// <summary>
        /// 异步保存文本数据
        /// </summary>
        public async Task SaveTextAsync(string key, string data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                await File.WriteAllTextAsync(StoragePath, data);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to save text to {StoragePath}", ex);
            }
        }

        /// <summary>
        /// 异步保存二进制数据
        /// </summary>
        public async Task SaveBinaryAsync(string key, byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                await File.WriteAllBytesAsync(StoragePath, data);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to save binary data to {StoragePath}", ex);
            }
        }

        /// <summary>
        /// 加载文本数据
        /// </summary>
        public string LoadText(string key)
        {
            try
            {
                return File.ReadAllText(StoragePath);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to load text from {StoragePath}", ex);
            }
        }

        /// <summary>
        /// 加载二进制数据
        /// </summary>
        public byte[] LoadBinary(string key)
        {
            try
            {
                return File.ReadAllBytes(StoragePath);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to load binary data from {StoragePath}", ex);
            }
        }

        /// <summary>
        /// 异步加载文本数据
        /// </summary>
        public async Task<string> LoadTextAsync(string key)
        {
            try
            {
                return await File.ReadAllTextAsync(StoragePath);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to load text from {StoragePath}", ex);
            }
        }

        /// <summary>
        /// 异步加载二进制数据
        /// </summary>
        public async Task<byte[]> LoadBinaryAsync(string key)
        {
            try
            {
                return await File.ReadAllBytesAsync(StoragePath);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to load binary data from {StoragePath}", ex);
            }
        }
    }
}