using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    [Serializable]
    public struct MonsterVector2
    {
        public float x, y;

        public MonsterVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public MonsterVector2(MonsterVector2 other)
        {
            x = other.x;
            y = other.y;
        }

        public MonsterVector2(Vector2 uVec2)
        {
            x = uVec2.x;
            y = uVec2.y;
        }
    }

    public class Vec2Adapter : IValueAdapter<Vector2, MonsterVector2>
    {
        public MonsterVector2 ConvertToSerializable(Vector2 native) => new(native);

        public Vector2 ConvertFromSerializable(MonsterVector2 serializable) => new(serializable.x, serializable.y);
    }
}