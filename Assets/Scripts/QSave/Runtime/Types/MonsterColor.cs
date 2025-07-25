using System;
using UnityEngine;

namespace QSave.Runtime
{
    [Serializable]
    public struct MonsterColor
    {
        public float r, g, b, a;

        public MonsterColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public MonsterColor(MonsterColor other)
        {
            r = other.r;
            g = other.g;
            b = other.b;
            a = other.a;
        }

        public MonsterColor(Color uColor)
        {
            r = uColor.r;
            g = uColor.g;
            b = uColor.b;
            a = uColor.a;
        }
    }

    public class ColorAdapter : IValueAdapter<Color, MonsterColor>
    {
        public MonsterColor ConvertToSerializable(Color native) => new(native);

        public Color ConvertFromSerializable(MonsterColor serializable) =>
            new(serializable.r, serializable.g, serializable.b, serializable.a);
    }
}