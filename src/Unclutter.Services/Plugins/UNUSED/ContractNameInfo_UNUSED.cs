using System;
using System.Collections.Generic;
using System.Linq;

namespace Unclutter.Services.Plugins.UNUSED
{
    public readonly struct ContractNameInfo_UNUSED : IEquatable<ContractNameInfo_UNUSED>, IEqualityComparer<ContractNameInfo_UNUSED>
    {
        #region Static
        public static ContractNameInfo_UNUSED Parse(string contract)
        {
            if (string.IsNullOrWhiteSpace(contract))
                throw new ArgumentNullException(nameof(contract));

            var typeName = contract;
            var genericArgumentsNameInfos = new List<ContractNameInfo_UNUSED>();

            var index = contract.IndexOf('(');
            if (index > 0)
            {
                var genericArgumentsNames = contract[(index + 1)..^1].Split(',');
                typeName = genericArgumentsNames.Length > 0
                    ? $"{contract[..index]}`{genericArgumentsNames.Length}"
                    : contract[..index];

                genericArgumentsNameInfos.AddRange(genericArgumentsNames.Select(Parse));
            }

            if (genericArgumentsNameInfos.Count <= 0)
                genericArgumentsNameInfos = null;

            return new ContractNameInfo_UNUSED(contract, genericArgumentsNameInfos, typeName);

        }
        public static ContractNameInfo_UNUSED Default => new ContractNameInfo_UNUSED(null, null, null);
        #endregion

        /* Properties */
        public string ContractName { get; }
        public string TypeName { get; }
        public ContractNameInfo_UNUSED[] GenericArgumentsNameInfos { get; }
        public bool IsGeneric => GenericArgumentsNameInfos != null && GenericArgumentsNameInfos.Length > 0;

        /* Constructor */
        public ContractNameInfo_UNUSED(string contractName, List<ContractNameInfo_UNUSED> genericArgumentsNameInfos, string typeName)
        {
            ContractName = contractName;
            GenericArgumentsNameInfos = genericArgumentsNameInfos?.ToArray();
            TypeName = typeName;
        }

        #region IEquatable
        public bool Equals(ContractNameInfo_UNUSED other)
        {
            return ContractName == other.ContractName && TypeName == other.TypeName && Equals(GenericArgumentsNameInfos, other.GenericArgumentsNameInfos);
        }
        #endregion

        #region IEqualityComparer
        public bool Equals(ContractNameInfo_UNUSED x, ContractNameInfo_UNUSED y)
        {
            return x.ContractName == y.ContractName && x.TypeName == y.TypeName && Equals(x.GenericArgumentsNameInfos, y.GenericArgumentsNameInfos);
        }
        public int GetHashCode(ContractNameInfo_UNUSED obj)
        {
            return HashCode.Combine(obj.ContractName, obj.TypeName, obj.GenericArgumentsNameInfos);
        }
        #endregion

        #region Overriden
        public override bool Equals(object obj)
        {
            return obj is ContractNameInfo_UNUSED other && Equals(other);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(ContractName, TypeName, GenericArgumentsNameInfos);
        }
        #endregion

        #region Operators
        public static bool operator ==(ContractNameInfo_UNUSED left, ContractNameInfo_UNUSED right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(ContractNameInfo_UNUSED left, ContractNameInfo_UNUSED right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}
