using System;
using System.ComponentModel.Composition.Hosting;

namespace Unclutter.Services.Plugins
{
    public interface IContainerExportProvider
    {
        ExportProvider ExportProvider { get; }
        IContainerExportProvider RegisterExport(Type type, string name);
    }
}
