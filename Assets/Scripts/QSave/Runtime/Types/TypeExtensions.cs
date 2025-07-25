using System;
using System.Collections.Generic;
using System.Linq;

namespace QSave.Runtime
{
    public static class TypeExtensions
    {
        public static bool IsGenericEnumerable(this Type type)
        {
            if (type == typeof(string))
                return false;

            return type.GetInterfaces()
                .Any(t => t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                                              t.GetGenericTypeDefinition() == typeof(ICollection<>)));
        }
    }
}