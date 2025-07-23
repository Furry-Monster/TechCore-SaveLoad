using System;
using System.Text;

namespace MonsterSave.Runtime
{
    public abstract class CharSerializer : ISerializer
    {
        public bool IsBinary => false;

        // interfaces impl
        byte[] ISerializer.Serialize(object obj)
        {
            if (obj == null)
                return null;
            if (!obj.GetType().IsSerializable)
                throw new InvalidCastException($"{obj.GetType().FullName} is not [Serializable].");

            var serializable = TypeRegistry.AdaptToSerializable(obj);
            var content = Serialize(serializable);
            return Encoding.UTF8.GetBytes(content, 0, content.Length);
        }

        object ISerializer.Deserialize(Type type, byte[] data)
        {
            if (type == null)
                return null;
            if (!type.IsSerializable)
                throw new InvalidCastException($"{type.FullName} is not [Serializable].");
            if (data == null || data.Length == 0)
                return null;

            var content = Encoding.UTF8.GetString(data, 0, data.Length);
            var serializable = Deserialize(type, content);
            return TypeRegistry.AdaptFromSerializable(serializable);
        }


        protected abstract string Serialize(object serializable);
        protected abstract object Deserialize(Type type, string content);
    }
}