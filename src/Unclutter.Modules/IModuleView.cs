using System;

namespace Unclutter.Modules
{
    public interface IModuleView
    {
        public Type ViewType { get; }
        string Label { get; }
        string IconRef { get; }
        double Priority { get; }
    }

    public class ModuleView : IModuleView
    {
        public Type ViewType { get; set; }
        public string Label { get; set; }
        public string IconRef { get; set; }
        public double Priority { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ExportModuleViewAttribute : Attribute, IModuleView
    {
        public Type ViewType { get; }
        public string Label { get; }
        public string IconRef { get; }
        public double Priority { get; }

        public ExportModuleViewAttribute(Type view, string label, string iconRef, double priority = double.MaxValue)
        {
            ViewType = view;
            Label = label;
            IconRef = iconRef;
            Priority = priority;
        }
    }
}
