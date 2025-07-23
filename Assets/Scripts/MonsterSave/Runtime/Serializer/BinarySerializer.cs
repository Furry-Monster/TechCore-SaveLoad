using System;

namespace MonsterSave.Runtime
{
    public abstract class BinarySerializer : ISerializer
    {
        public bool IsBinary => true;

        // interfaces impl
        byte[] ISerializer.Serialize(object obj)
        {
            if (obj == null)
                return null;
            if (!obj.GetType().IsSerializable)
                throw new InvalidCastException($"{obj.GetType().FullName} is not [Serializable].");

            return Serialize(obj);
        }

        object ISerializer.Deserialize(Type type, byte[] data)
        {
            if (type == null)
                return null;
            if (!type.IsSerializable)
                throw new InvalidCastException($"{type.FullName} is not [Serializable].");
            if (data == null || data.Length == 0)
                return null;

            return Deserialize(type, data);
        }

        T ISerializer.Deserialize<T>(byte[] data)
        {
            if (!typeof(T).IsSerializable)
                throw new InvalidCastException($"{typeof(T).FullName} is not [Serializable].");
            if (data == null || data.Length == 0)
                return default;

            return Deserialize<T>(data);
        }

        protected abstract byte[] Serialize(object serializable);
        protected abstract object Deserialize(Type type, byte[] bytes);
        protected abstract T Deserialize<T>(byte[] bytes);
    }
}