using System;

namespace Unclutter.Extensions.Modules.Attributes
{
    /// <summary>
    /// This attribute is a marker for modules to register their view,
    /// which will be integrated into the main application and will be Navigatable "navigable" (not sure if this is a used word in English)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ModuleViewAttribute : Attribute, IModuleView
    {
        /// <summary>
        /// The text to display in the Navigation view of the application.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// A MaterialDesign Icon, you only need to pass the corresponding name from https://materialdesignicons.com/
        /// </summary>
        public string IconId { get; }

        /// <summary>
        /// The type of the View which will be navigated to, this View and its ViewModel will be resolved using the IoC Container.
        /// </summary>
        public Type ViewType { get; }

        public string GroupName { get; }
        public double ItemPriority { get; }

        /// <summary>
        /// A Marker attribute for modules to register their views
        /// </summary>
        /// <param name="view">The type of the View which will be navigated to, this View and its ViewModel will be resolved using prism IoC Container.</param>
        /// <param name="label">The text to display in the Navigation sidebar of the application</param>
        /// <param name="iconId">A MaterialDesign Icon, you only need to pass the corresponding name from https://materialdesignicons.com/ </param>
        /// <param name="groupName"></param>
        /// <param name="priority"></param>
        public ModuleViewAttribute(Type view, string label, string iconId, string groupName = "", double priority = double.MaxValue)
        {
            ViewType = view;
            Label = label;
            IconId = iconId;
            GroupName = groupName;
            ItemPriority = priority;
        }


    }
}
