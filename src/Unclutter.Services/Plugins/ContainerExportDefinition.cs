using System;
using System.ComponentModel.Composition.Primitives;

namespace Unclutter.Services.Plugins
{
    public class ContainerExportDefinition : ExportDefinition
    {
        /* Properties */
        public Type ContractType { get; }
        public string RegistrationName { get; }
        public Func<object> Factory { get; }

        /* Constructor */
        public ContainerExportDefinition(Type contractType, string registrationName, Func<object> factory)
            : base(PluginServices.GetContractName(contractType, registrationName), PluginServices.GetMetadata(contractType))
        {
            ContractType = contractType ?? throw new ArgumentNullException(nameof(contractType));
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
            RegistrationName = registrationName;
        }
    }
}
