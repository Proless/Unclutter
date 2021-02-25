using System;

namespace Unclutter.Extensions.Modules.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DependentViewAttribute : Attribute
    {
        public Type ViewType { get; }
        public string TargetRegionName { get; }

        public DependentViewAttribute(Type viewType, string targetRegionName)
        {
            ViewType = viewType;
            TargetRegionName = targetRegionName;
        }
    }
}
