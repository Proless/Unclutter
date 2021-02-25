using System.Collections.Generic;
using System.Reflection;

namespace Unclutter.Services.Plugins
{
    public class AssemblyEqualityComparer : EqualityComparer<Assembly>
    {
        public override bool Equals(Assembly x, Assembly y)
        {
            return x?.FullName == y?.FullName;
        }

        public override int GetHashCode(Assembly obj)
        {
            return obj.GetHashCode();
        }
    }
}
