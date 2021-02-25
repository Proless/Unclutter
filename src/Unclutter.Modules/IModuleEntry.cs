using System.Collections.Generic;

namespace Unclutter.Modules
{
    public interface IModuleEntry : IModuleMetadata
    {
        string ModuleLocation { get; set; }
        IEnumerable<IModuleView> SettingViews { get; }
        IEnumerable<IModuleView> Views { get; }
    }
}