using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;

namespace Unclutter.Modules.Plugins
{
    public interface IShellCommandAction
    {
        double Priority { get; }
        string Hint { get; }
        Control Control { get; }
        ICommand Action { get; }
        void Initialize();
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class)]
    public sealed class ExportShellCommandActionAttribute : InheritedExportAttribute
    {
        public ExportShellCommandActionAttribute() : base(typeof(IShellCommandAction)) { }
    }

}
