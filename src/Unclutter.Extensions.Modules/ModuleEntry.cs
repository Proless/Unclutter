using System.Collections.Generic;

namespace Unclutter.Extensions.Modules
{
    public class ModuleEntry : IModuleEntry
    {
        /** Metadata **/
        public string Name { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        /****/
        public string ModuleLocation { get; set; }
        public IEnumerable<IModuleView> SettingViews { get; set; }
        public IEnumerable<IModuleView> Views { get; set; }

    }
}
