using System;
using Unclutter.SDK.Metadata;

namespace Unclutter.Extensions.Modules
{
    public interface IModuleView : IGroupItem
    {
        string Label { get; }
        /// <summary>
        /// A MaterialDesign Icon, you only need to pass the corresponding name from https://materialdesignicons.com/
        /// </summary>
        string IconId { get; }
        Type ViewType { get; }
    }
}
