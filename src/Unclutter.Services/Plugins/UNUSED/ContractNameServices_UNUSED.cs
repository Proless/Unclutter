using System;

namespace Unclutter.Services.Plugins.UNUSED
{
    public static class ContractNameServices_UNUSED
    {
        public static bool TryFindType(ContractNameInfo_UNUSED infoUnused, out Type type)
        {
            Type foundType = null;
            try
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {

                }
            }
            catch
            {
                foundType = null;
            }

            type = foundType;
            return type != null;
        }
    }
}
