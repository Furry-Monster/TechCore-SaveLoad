using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    [Serializable]
    public struct MonsterVector3
    {
        public float x, y, z;

        public MonsterVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public MonsterVector3(MonsterVector3 other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
        }

        public MonsterVector3(Vector3 uVec3)
        {
            x = uVec3.x;
            y = uVec3.y;
            z = uVec3.z;
        }
    }

    public class Vec3Adapter : ITypeAdapter<Vector3, MonsterVector3>
    {
        public MonsterVector3 ConvertToSerializable(Vector3 native) => new(native);

        public Vector3 ConvertFromSerializable(MonsterVector3 serializable) =>
            new(serializable.x, serializable.y, serializable.z);
    }
}