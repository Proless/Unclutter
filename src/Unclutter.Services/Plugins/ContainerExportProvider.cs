using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Unclutter.Services.Container;

namespace Unclutter.Services.Plugins
{
    public class ContainerExportProvider : ExportProvider, IContainerExportProvider
    {
        /* Fields */
        private readonly IContainerAdapter _containerAdapter;
        private readonly ConcurrentBag<ContainerExportDefinition> _definitions = new ConcurrentBag<ContainerExportDefinition>();

        /* Properties */
        public ExportProvider ExportProvider => this;

        /* Constructor */
        public ContainerExportProvider(IContainerAdapter containerAdapter)
        {
            _containerAdapter = containerAdapter ?? throw new ArgumentNullException(nameof(containerAdapter));
            _containerAdapter.ComponentRegistered += OnComponentRegistered;
        }

        /* Methods */
        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            IEnumerable<Export> exports;
            if (definition.Cardinality == ImportCardinality.ZeroOrMore)
            {
                exports = from exportDefinition in _definitions
                          let contractName = PluginServices.GetContractName(exportDefinition.ContractType, exportDefinition.RegistrationName)
                          where contractName == definition.ContractName
                          select new Export(exportDefinition, exportDefinition.Factory);
            }
            else
            {
                exports = from exportDefinition in _definitions
                          where definition.IsConstraintSatisfiedBy(exportDefinition)
                          select new Export(exportDefinition, exportDefinition.Factory);
            }

            return exports;
        }

        public void RegisterAsExport(Type exportType, string name)
        {
            if (CanRegister(exportType, name))
            {
                var exportDefinition = new ContainerExportDefinition(exportType, name, () => Resolve(exportType, name));
                _definitions.Add(exportDefinition);
            }
        }

        public IContainerExportProvider RegisterExport(Type type, Func<object> factory)
        {
            _containerAdapter.Register(type, factory);
            return this;
        }

        /* Helpers */
        private bool CanRegister(Type exportType, string name)
        {
            return exportType != null && _definitions.Count(d => d.ContractType == exportType && d.RegistrationName == name) == 0;
        }

        private object Resolve(Type type, string name)
        {
            return string.IsNullOrWhiteSpace(name) ? _containerAdapter.Resolve(type) : _containerAdapter.Resolve(type, name);
        }

        private void OnComponentRegistered(RegisteredComponentEventArgs args)
        {
            RegisterAsExport(args.Type, args.Name);
        }
    }
}
