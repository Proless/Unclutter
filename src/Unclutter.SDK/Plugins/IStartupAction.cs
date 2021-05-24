using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace Unclutter.SDK.Plugins
{
    public interface IStartupAction
    {
        string Label { get; }
        string IconRef { get; }
        string Hint { get; }
        double Priority { get; }
        ICommand Action { get; }
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class)]
    public sealed class ExportStartupActionAttribute : ExportAttribute
    {
        public ExportStartupActionAttribute() : base(typeof(IStartupAction)) { }
    }
}
