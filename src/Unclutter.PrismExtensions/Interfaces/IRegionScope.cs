using Prism.Regions;

namespace Unclutter.PrismExtensions.Interfaces
{
	public interface IRegionScope
	{
		IRegionManager ScopeRegionManager { get; set; }
	}
}
