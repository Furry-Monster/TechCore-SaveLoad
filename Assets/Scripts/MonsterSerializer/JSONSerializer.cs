using System;
using System.Text;
using UnityEngine;

namespace MonsterSerializer
{
    public class JSONSerializer : ISerializer
    {
        public byte[] Serialize(object obj)
        {
            if (obj == null || !obj.GetType().IsSerializable)
                return null;

            var json = JsonUtility.ToJson(obj);
            var bytes = Encoding.UTF8.GetBytes(json);

            return bytes;
        }

        public object Deserialize(Type type, byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            var json = Encoding.UTF8.GetString(bytes);
            var obj = JsonUtility.FromJson(json, type);
            return obj;
        }
    }
}