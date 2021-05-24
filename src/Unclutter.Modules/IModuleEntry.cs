using System.Collections.Generic;

namespace Unclutter.Modules
{
    public interface IModuleEntry
    {
        string ModuleLocation { get; set; }
        IModuleMetadata Metadata { get; }
        IEnumerable<IModuleView> Views { get; }
    }
}