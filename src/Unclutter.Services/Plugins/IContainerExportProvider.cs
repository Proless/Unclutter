using System;
using System.ComponentModel.Composition.Hosting;

namespace Unclutter.Services.Plugins
{
    /// <summary>
    /// This depends on the IoC Container to support registering type and providing a factory delegate to retrieve the object instance.
    /// </summary>
    public interface IContainerExportProvider
    {
        ExportProvider ExportProvider { get; }

        /// <summary>
        /// Register an exported type from the MEF container in the underlying container to make it available for resolution
        /// </summary>
        IContainerExportProvider RegisterExport(Type type, Func<object> factory);
    }
}
