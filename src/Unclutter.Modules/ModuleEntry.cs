using System.Collections.Generic;

namespace Unclutter.Modules
{
    public class ModuleEntry : IModuleEntry
    {
        public string ModuleLocation { get; set; }
        public IModuleMetadata Metadata { get; set; }
        public IEnumerable<IModuleView> Views { get; set; }
    }
}
