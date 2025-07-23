using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    [Serializable]
    public struct MonsterVector4
    {
        public float x, y, z, w;

        public MonsterVector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public MonsterVector4(MonsterVector4 other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
            w = other.w;
        }

        public MonsterVector4(Vector4 uVec4)
        {
            x = uVec4.x;
            y = uVec4.y;
            z = uVec4.z;
            w = uVec4.w;
        }
    }

    public class Vec4Adapter : IValueAdapter<Vector4, MonsterVector4>
    {
        public MonsterVector4 ConvertToSerializable(Vector4 native) => new(native.x, native.y, native.z, native.w);

        public Vector4 ConvertFromSerializable(MonsterVector4 serializable) =>
            new(serializable.x, serializable.y, serializable.z, serializable.w);
    }
}