using System;
using System.Collections.Generic;
using System.Linq;

namespace Unclutter.Extensions.Modules
{
    public static class Extensions
    {
        public static IEnumerable<T> GetCustomAttributes<T>(this Type type) where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), true).OfType<T>();
        }
    }
}
