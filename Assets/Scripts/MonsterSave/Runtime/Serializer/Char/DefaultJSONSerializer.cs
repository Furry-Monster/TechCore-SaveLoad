using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    /// <summary>
    /// 默认JSON序列化器，使用Unity内置JSONUtility
    /// </summary>
    public class DefaultJSONSerializer : CharSerializer
    {
        public override string Serialize(object serializable)
        {
            if (serializable == null || !serializable.GetType().IsSerializable)
                return null;

            return JsonUtility.ToJson(serializable);
        }

        public override object Deserialize(Type type, string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;

            return JsonUtility.FromJson(json, type);
        }

        public override T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default;

            return JsonUtility.FromJson<T>(json);
        }
    }
}