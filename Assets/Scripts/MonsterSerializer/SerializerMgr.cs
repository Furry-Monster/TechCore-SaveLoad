using System;

namespace MonsterSerializer
{
    public class SerializerMgr : IDisposable
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<SerializerMgr> _instance = new(() => new SerializerMgr());
        public static SerializerMgr Instance => _instance.Value;

        private ISerializer _currentSerializer;
        private bool _disposed;

        protected SerializerMgr()
        {
            _currentSerializer = new JSONSerializer();
        }

        public byte[] Serialize(object obj)
        {
            var bytes = _currentSerializer.Serialize(obj);
            return Array.Empty<byte>();
        }

        public object Deserialize(Type type, byte[] bytes)
        {
            return null;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _currentSerializer = null;
                _disposed = true;
            }
        }
    }
}