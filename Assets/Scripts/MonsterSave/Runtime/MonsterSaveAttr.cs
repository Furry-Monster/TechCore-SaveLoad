using System;
using System.Collections;
using UnityEngine;

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