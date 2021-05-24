using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace Unclutter.Services.Plugins
{
    public class ContainerExportDefinition : ExportDefinition
    {
        #region Static
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
        #endregion

        /* Properties */
        public Type ContractType { get; }
        public string RegistrationName { get; }
        public Func<object> Factory { get; }

        /* Constructor */
        public ContainerExportDefinition(Type contractType, string registrationName, Func<object> factory) : base(GetContractName(contractType, registrationName), GetMetadata(contractType))
        {
            ContractType = contractType ?? throw new ArgumentNullException(nameof(contractType));
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
            RegistrationName = registrationName;
        }
    }
}
