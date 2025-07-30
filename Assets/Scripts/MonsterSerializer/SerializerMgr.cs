using System;
using System.Runtime.Serialization;

namespace MonsterSerializer
{
    public class SerializerMgr : IDisposable
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<SerializerMgr> _instance = new(() => new SerializerMgr());

        private ISerializer _currentSerializer;
        private bool _disposed;

        protected SerializerMgr()
        {
            _currentSerializer = new JSONSerializer();
        }

        public static SerializerMgr Instance => _instance.Value;

        public void Dispose()
        {
            if (!_disposed)
            {
                _currentSerializer = null;
                _disposed = true;
            }
        }

        public byte[] Serialize(object obj)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SerializerMgr));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (_currentSerializer == null)
                throw new InvalidOperationException("Serializer is not initialized.");

            try
            {
                return _currentSerializer.Serialize(obj);
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
            if (_currentSerializer == null)
                throw new InvalidOperationException("Serializer is not initialized.");

            try
            {
                return _currentSerializer.Deserialize(type, bytes);
            }
            catch (Exception ex)
            {
                throw new SerializationException("Failed to deserialize object.", ex);
            }
        }

        public void SetSerializer(ISerializer serializer)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SerializerMgr));

            _currentSerializer = serializer ??
                                 throw new ArgumentNullException(nameof(serializer));
        }
    }
}