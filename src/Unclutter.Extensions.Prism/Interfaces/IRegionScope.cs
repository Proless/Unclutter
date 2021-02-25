using Prism.Regions;

namespace Unclutter.Extensions.Prism.Interfaces
{
	public interface IRegionScope
	{
		IRegionManager ScopeRegionManager { get; set; }
	}
}
