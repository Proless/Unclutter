using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace Unclutter.Services.Plugins
{
    public static class PluginServices
    {
        public static IDictionary<string, object> GetMetadata(Type type)
        {
            return new Dictionary<string, object>
            {
                {CompositionConstants.ExportTypeIdentityMetadataName, AttributedModelServices.GetTypeIdentity(type)},
                {CompositionConstants.PartCreationPolicyMetadataName, CreationPolicy.Any},
                {CompositionConstants.ImportSourceMetadataName, ImportSource.Any}
            };
        }
        public static string GetContractName(Type type, string registrationName)
        {
            return registrationName ?? AttributedModelServices.GetContractName(type);
        }
        public static bool TryGetType(string typeIdentity, out Type type)
        {
            if (typeIdentity == null)
            {
                type = null;
                return false;
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var assemblyType in assembly.GetTypes())
                {
                    var identity = AttributedModelServices.GetTypeIdentity(assemblyType);

                    if (identity != typeIdentity) continue;

                    type = assemblyType;
                    return true;
                }
            }

            type = null;
            return false;
        }
        public static string GetExportTypeIdentity(this ExportDefinition definition)
        {
            if (definition is null)
            {
                return string.Empty;
            }

            if (definition.Metadata.TryGetValue(CompositionConstants.ExportTypeIdentityMetadataName, out var typeIdentity) && typeIdentity is string id)
            {
                return id;
            }

            return string.Empty;
        }
        public static ImportDefinition GetImportDefinition(Type type, string name)
        {
            var contractName = GetContractName(type, name);
            var typeIdentity = AttributedModelServices.GetTypeIdentity(type);
            var cardinality = ImportCardinality.ZeroOrOne;
            var creationPolicy = CreationPolicy.Any;

            var definition = new ContractBasedImportDefinition(
                contractName,
                typeIdentity,
                null,
                cardinality,
                true,
                true,
                creationPolicy,
                GetMetadata(type));

            return definition;
        }
    }
}
