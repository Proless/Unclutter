using System;
using System.Collections.Generic;

namespace Unclutter.Modules
{
    public interface IModuleProvider
    {
        IEnumerable<IModuleEntry> Modules { get; }
        event Action<IModuleEntry> ModuleAdded;
    }
}
