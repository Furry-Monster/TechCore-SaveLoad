using System;

namespace MonsterSave.Runtime
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class MSSerializable : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class MSNonSerializable : Attribute
    {
    }
}