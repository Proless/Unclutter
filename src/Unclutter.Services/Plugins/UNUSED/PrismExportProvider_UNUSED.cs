using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Prism.Ioc;

namespace Unclutter.Services.Plugins.UNUSED
{
    /// <summary>
    /// This class is currently not used as it is not yet fully implemented.
    /// </summary>
    public class PrismExportProvider_UNUSED : ExportProvider
    {
        /* Fields */
        private readonly Dictionary<ContractNameInfo_UNUSED, Type> _typeCache = new Dictionary<ContractNameInfo_UNUSED, Type>();
        private readonly List<Export> _exports = new List<Export>();

        /* Services */
        private readonly IContainerExtension _containerExtension;

        /* Constructor */
        public PrismExportProvider_UNUSED(IContainerExtension containerExtension)
        {
            _containerExtension = containerExtension;
        }

        /* Methods */
        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            var exports = _exports.Where(e => definition.IsConstraintSatisfiedBy(e.Definition)).ToArray();

            if (!exports.Any())
            {
                TryCreateExport(definition);
            }

            return exports;
        }

        /* Helpers */
        private void TryCreateExport(ImportDefinition definition)
        {
            var contractName = definition.ContractName;
            var typeName = definition.ContractName;

            if (definition is ContractBasedImportDefinition importDefinition)
            {
                contractName = importDefinition.ContractName;
                typeName = importDefinition.RequiredTypeIdentity;
            }

            if (!TryGetType(typeName, out var type)) return;

            var typeIsContractName = contractName == typeName;
            var isRegistered = typeIsContractName ? IsRegistered(type) : IsRegistered(type, contractName);

            if (isRegistered)
            {
                _exports.Add(CreateExport(type, definition, typeIsContractName));
            }
        }

        private bool TryGetType(string contractName, out Type type)
        {
            // empty, whitespace or null are not valid type names
            if (string.IsNullOrWhiteSpace(contractName))
            {
                type = null;
                return false;
            }

            var nameInfo = ContractNameInfo_UNUSED.Parse(contractName);

            lock (_typeCache)
            {
                // look for the type in the cache
                if (_typeCache.TryGetValue(nameInfo, out type))
                    return true;

                // at this point we should look for the type in all loaded assemblies
                try
                {
                    if (ContractNameServices_UNUSED.TryFindType(nameInfo, out type))
                    {
                        _typeCache[nameInfo] = type;
                        return true;
                    }
                }
                catch
                {
                    type = null;
                }

                return type != null;
            }
        }

        private Export CreateExport(Type type, ImportDefinition importDefinition, bool typeIsContractName)
        {
            var metadata = new Dictionary<string, object>(importDefinition.Metadata)
            {
                {CompositionConstants.ExportTypeIdentityMetadataName, importDefinition.ContractName},
                {CompositionConstants.PartCreationPolicyMetadataName, CreationPolicy.Any},
                {CompositionConstants.ImportSourceMetadataName, ImportSource.Any}
            };

            var export = new Export(importDefinition.ContractName, metadata, typeIsContractName ? () => GetValue(type) : () => GetValue(type, importDefinition.ContractName));

            return export;
        }

        private bool IsRegistered(Type type) => _containerExtension.IsRegistered(type);

        private bool IsRegistered(Type type, string name) => _containerExtension.IsRegistered(type, name);

        private object GetValue(Type type)
        {
            return _containerExtension.Resolve(type);
        }

        private object GetValue(Type type, string name)
        {
            return _containerExtension.Resolve(type, name);
        }
    }
}
