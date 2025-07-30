using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    [Serializable]
    public struct MonsterQuaternion
    {
        public float x, y, z, w;

        public MonsterQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public MonsterQuaternion(MonsterQuaternion other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
            w = other.w;
        }

        public MonsterQuaternion(Quaternion uQuaternion)
        {
            x = uQuaternion.x;
            y = uQuaternion.y;
            z = uQuaternion.z;
            w = uQuaternion.w;
        }
    }

    public class QuaternionAdapter : IValueAdapter<Quaternion, MonsterQuaternion>
    {
        public MonsterQuaternion ConvertToSerializable(Quaternion native)
        {
            return new MonsterQuaternion(native.x, native.y, native.z, native.w);
        }

        public Quaternion ConvertFromSerializable(MonsterQuaternion serializable)
        {
            return new Quaternion(serializable.x, serializable.y, serializable.z, serializable.w);
        }
    }
}