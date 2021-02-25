using System;
using System.Collections.Specialized;
using System.Windows;
using Prism.Regions;
using Unclutter.Extensions.Prism.Interfaces;

namespace Unclutter.Extensions.Prism.Behaviors
{
	public class RegionManagerAwareBehavior : RegionBehavior
	{
		public const string BehaviorKey = "RegionManagerAwareBehavior";

		protected override void OnAttach()
		{
			Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
		}

		private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					{
						foreach (var item in e.NewItems)
						{
							var regionManager = Region.RegionManager;

							if (item is FrameworkElement element)
							{
								if (element.GetValue(RegionManager.RegionManagerProperty) is IRegionManager scopedRegionManager)
									regionManager = scopedRegionManager;
							}

							InvokeOnRegionManagerAwareElement(item, x => x.ScopeRegionManager = regionManager);
						}

						break;
					}
				case NotifyCollectionChangedAction.Remove:
					{
						foreach (var item in e.OldItems)
						{
							InvokeOnRegionManagerAwareElement(item, x => x.ScopeRegionManager = null);
						}

						break;
					}
			}
		}

		private static void InvokeOnRegionManagerAwareElement(object item, Action<IRegionScope> invocation)
		{
			if (item is IRegionScope rmAwareItem)
				invocation(rmAwareItem);

			if (item is FrameworkElement frameworkElement)
			{
				if (frameworkElement.DataContext is IRegionScope rmAwareDataContext)
				{
					if (frameworkElement.Parent is FrameworkElement frameworkElementParent)
					{
						if (frameworkElementParent.DataContext is IRegionScope rmAwareDataContextParent)
						{
							if (rmAwareDataContext == rmAwareDataContextParent)
							{
								return;
							}
						}
					}

					invocation(rmAwareDataContext);
				}
			}
		}
	}
}
