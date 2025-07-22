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
            return JsonUtility.ToJson(serializable);
        }

        public object Deserialize(Type type, string json)
        {
            return JsonUtility.FromJson(json, type);
        }
    }
}