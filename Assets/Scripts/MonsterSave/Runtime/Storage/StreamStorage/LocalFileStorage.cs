using System;
using System.IO;
using System.Threading.Tasks;

namespace MonsterSave.Runtime
{
    public class LocalFileStorage : IStreamStorage
    {
        private readonly Storage _storage;

        public LocalFileStorage(Storage storage)
        {
            _storage = storage;
        }

        public string StoragePath => _storage.StoragePath;

        /// <summary>
        /// 保存文本数据
        /// </summary>
        public void SaveText(string data)
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
        public void SaveBinary(byte[] data)
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
        public async Task SaveTextAsync(string data)
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
        public async Task SaveBinaryAsync(byte[] data)
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
        public string LoadText()
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
        public byte[] LoadBinary()
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
        public async Task<string> LoadTextAsync()
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
        public async Task<byte[]> LoadBinaryAsync()
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