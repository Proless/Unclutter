using System;

namespace Unclutter.Modules
{
    public class ModuleView : IModuleView
    {
        public string Label { get; }
        /// <summary>
        /// A MaterialDesign Icon, you only need to pass the corresponding name from https://materialdesignicons.com/
        /// </summary>
        public string IconId { get; }
        public Type ViewType { get; }
        public ModuleView(Type viewType, string label, string icon, string groupName = "", double priority = double.MaxValue)
        {
            Label = label;
            IconId = icon;
            GroupName = groupName;
            ItemPriority = priority;
            ViewType = viewType;
        }

        public string GroupName { get; }
        public double ItemPriority { get; }
    }
}
