using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MonsterSerializer
{
    public class SerializerMgr : IDisposable
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<SerializerMgr> _instance = new(() => new SerializerMgr());
        private bool _disposed;

        private ITypeAdapter _typeAdapter;
        private ISerializer _serializer;
        private IEncryptor _encryptor;

        protected SerializerMgr()
        {
            _typeAdapter = null;
            _serializer = new JSONSerializer();
            _encryptor = null;
        }

        public static SerializerMgr Instance => _instance.Value;

        public void Dispose()
        {
            if (_disposed)
                return;

            _typeAdapter = null;
            _serializer = null;
            _encryptor = null;
            _disposed = true;
        }

        public byte[] Serialize(object obj)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SerializerMgr));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (_serializer == null)
                throw new InvalidOperationException("Serializer is not initialized.");

            try
            {
                var adapted = _typeAdapter?.Adapt(obj);
                var serialized = _serializer.Serialize(adapted);
                var encrypted = _encryptor?.Encrypt(serialized);
                return encrypted;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Failed to serialize object.", ex);
            }
        }

        public object Deserialize(Type type, byte[] bytes)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SerializerMgr));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (_serializer == null)
                throw new InvalidOperationException("Serializer is not initialized.");

            try
            {
                var decrypted = _encryptor?.Decrypt(bytes);
                var deserialized = _serializer.Deserialize(type, decrypted);
                var deadapted = _typeAdapter?.Restore(deserialized);
                return deadapted;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Failed to deserialize object.", ex);
            }
        }

        public void SetTypeAdapter(ITypeAdapter typeAdapter)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SerializerMgr));

            _typeAdapter = typeAdapter;
        }

        public void SetSerializer([NotNull] ISerializer serializer)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SerializerMgr));

            _serializer = serializer
                          ?? throw new ArgumentNullException(nameof(serializer));
        }

        public void SetEncryptor(IEncryptor encryptor)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SerializerMgr));

            _encryptor = encryptor;
        }
    }
}