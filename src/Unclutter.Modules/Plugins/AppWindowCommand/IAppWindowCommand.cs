using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Unclutter.SDK.Common;

namespace Unclutter.Modules.Plugins.AppWindowCommand
{
    public interface IAppWindowCommand : IAppWindowPlugin, IOrderedObject
    {
        string Hint { get; }
        bool IsVisible { get; }
        ICommand Command { get; }
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ExportAppWindowCommandAttribute : ExportAttribute
    {
        public ExportAppWindowCommandAttribute() : base(typeof(IAppWindowCommand)) { }
    }

}
