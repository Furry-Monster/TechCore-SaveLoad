using System;
using UnityEngine;

namespace QSave.Runtime
{
    [Serializable]
    public struct MonsterBounds
    {
        public MonsterVector3 center;
        public MonsterVector3 size;

        public MonsterBounds(MonsterVector3 center, MonsterVector3 size)
        {
            this.center = center;
            this.size = size;
        }

        public MonsterBounds(MonsterBounds other)
        {
            center = other.center;
            size = other.size;
        }

        public MonsterBounds(Vector3 center, Vector3 size)
        {
            this.center = new MonsterVector3(center);
            this.size = new MonsterVector3(size);
        }

        public MonsterBounds(Bounds uBounds)
        {
            center = new MonsterVector3(uBounds.center);
            size = new MonsterVector3(uBounds.size);
        }
    }

    public class BoundsAdapter : IValueAdapter<Bounds, MonsterBounds>
    {
        public MonsterBounds ConvertToSerializable(Bounds native) => new(native.center, native.size);

        public Bounds ConvertFromSerializable(MonsterBounds serializable) =>
            new(new Vector3(serializable.center.x,
                    serializable.center.y,
                    serializable.center.z),
                new Vector3(serializable.size.x,
                    serializable.size.y,
                    serializable.size.z));
    }
}