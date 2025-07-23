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

            var adapted = TypeRegistry.AdaptToSerializable(obj);
            return Serialize(adapted);
        }

        object ISerializer.Deserialize(Type type, byte[] data)
        {
            if (type == null)
                return null;
            if (!type.IsSerializable)
                throw new InvalidCastException($"{type.FullName} is not [Serializable].");
            if (data == null || data.Length == 0)
                return null;

            var serializable = Deserialize(type, data);
            return TypeRegistry.AdaptFromSerializable(serializable);
        }

        protected abstract byte[] Serialize(object serializable);
        protected abstract object Deserialize(Type type, byte[] bytes);
    }
}