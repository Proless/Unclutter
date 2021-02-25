using System;

namespace Unclutter.Extensions.Modules.Attributes
{
    /// <summary>
    /// This attribute is a marker for modules to register their settings view.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ModuleSettingsViewAttribute : Attribute, IModuleView
    {
        /// <summary>
        /// The text to display in the Navigation control of the Settings view.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// A MaterialDesign Icon, you only need to pass the corresponding name from https://materialdesignicons.com/
        /// </summary>
        public string IconId { get; }

        /// <summary>
        /// The type of the settings View, this View and its ViewModel will be resolved using the IoC Container.
        /// </summary>
        public Type ViewType { get; }

        public string GroupName { get; }
        public double ItemPriority { get; }

        /// <summary>
        /// A Marker attribute for modules to register their settings view.
        /// </summary>
        /// <param name="view">The type of the settings View</param>
        /// <param name="icon">A MaterialDesign Icon, you only need to pass the corresponding name from https://materialdesignicons.com/</param>
        /// <param name="label">The text to display in the navigation control for this settings view</param>
        /// <param name="groupName"></param>
        /// <param name="priority"></param>
        public ModuleSettingsViewAttribute(Type view, string label, string icon = "", string groupName = "", double priority = double.MaxValue)
        {
            ViewType = view;
            Label = label;
            GroupName = groupName;
            ItemPriority = priority;
            IconId = icon;
        }


    }
}
