using System;
using UnityEngine;

namespace MonsterSave.Runtime
{
    [Serializable]
    public struct MonsterRect
    {
        public float x, y, width, height;

        public MonsterRect(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public MonsterRect(MonsterRect other)
        {
            x = other.x;
            y = other.y;
            width = other.width;
            height = other.height;
        }

        public MonsterRect(Rect uRect)
        {
            x = uRect.x;
            y = uRect.y;
            width = uRect.width;
            height = uRect.height;
        }
    }

    public class RectAdapter : IValueAdapter<Rect, MonsterRect>
    {
        public MonsterRect ConvertToSerializable(Rect native)
        {
            return new MonsterRect(native);
        }

        public Rect ConvertFromSerializable(MonsterRect serializable)
        {
            return new Rect(serializable.x, serializable.y,
                serializable.width, serializable.height);
        }
    }
}