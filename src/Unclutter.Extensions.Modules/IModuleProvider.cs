using System.Collections.Generic;

namespace Unclutter.Extensions.Modules
{
    public interface IModuleProvider
    {
        IEnumerable<IModuleEntry> Modules { get; }
        void AddModule(IModuleEntry module);
        object ResolveView(IModuleView moduleView);
        void RegisterView(IModuleView moduleView);
    }
}
