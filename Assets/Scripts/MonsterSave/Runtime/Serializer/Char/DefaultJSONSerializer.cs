using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    /// <summary>
    /// 默认JSON序列化器，使用Unity内置JSONUtility
    /// </summary>
    public class DefaultJSONSerializer : CharSerializer
    {
        protected override string SerializeHandler(object serializable)
        {
            if (serializable == null || !serializable.GetType().IsSerializable)
                return null;

            return JsonUtility.ToJson(serializable);
        }

        protected override object DeserializeHandler(Type type, string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;

            return JsonUtility.FromJson(json, type);
        }
    }
}