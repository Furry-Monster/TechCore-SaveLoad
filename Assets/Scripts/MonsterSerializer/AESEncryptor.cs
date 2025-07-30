using System;
using System.IO;
using System.Security.Cryptography;

namespace MonsterSerializer
{
    public class AESEncryptor : IEncryptor
    {
        private readonly byte[] _iv;
        private readonly byte[] _key;

        public AESEncryptor()
        {
            // auto generate key & iv , which is not recommended
            using var aes = Aes.Create();
            _key = aes.Key; // 256-bit key
            _iv = aes.IV; // 128-bit initializing vector
        }

        public AESEncryptor(byte[] key, byte[] iv)
        {
            // use ur own key & iv , recommended!
            if (key != null && key.Length != 32) // AES-256 requires 32-bytes(256-bit) key
                throw new ArgumentException("Key must be 32 bytes");
            if (iv != null && iv.Length != 16) // AES-256 requires 16-bytes(128-bit) iv
                throw new ArgumentException("IV must be 16 bytes");

            _key = key;
            _iv = iv;
        }

        public byte[] Encrypt(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            try
            {
                using var aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;

                using var ms = new MemoryStream();
                using var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);

                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw new CryptographicException("AES encryption failed", ex);
            }
        }

        public byte[] Decrypt(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            try
            {
                using var aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;

                using var ms = new MemoryStream();
                using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw new CryptographicException("AES decryption failed", ex);
            }
        }
    }
}