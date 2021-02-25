using System.Collections.Generic;
using Unclutter.SDK.IModels;

namespace Unclutter.SDK.Plugins.StartupAction
{
    public interface IStartupAction
    {
        void Execute(IEnumerable<IUserProfile> profiles);
    }
}
