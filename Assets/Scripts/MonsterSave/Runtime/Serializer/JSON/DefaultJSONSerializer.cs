using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    /// <summary>
    /// 默认JSON序列化器，使用Unity内置JSONUtility
    /// </summary>
    public class DefaultJSONSerializer : IJSONSerializer
    {
        public string Serialize(object serializable)
        {
            if (serializable == null)
                return null;
            return JsonUtility.ToJson(serializable);
        }

        public object Deserialize(Type type, string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;
            return JsonUtility.FromJson(json, type);
        }
    }
}