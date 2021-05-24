using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using Prism.Ioc;
using Prism.Regions;
using Unclutter.PrismExtensions.Interfaces;

namespace Unclutter.PrismExtensions.Behaviors
{
    public class DependentViewRegionBehavior : RegionBehavior
    {
        /* Fields */
        private readonly IContainerProvider _containerProvider;
        private readonly Dictionary<object, List<DependentViewInfo>> _dependentViewCache = new Dictionary<object, List<DependentViewInfo>>();

        public DependentViewRegionBehavior(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
        }

        /* Properties */

        public const string BehaviorKey = "DependentViewRegionBehavior";

        /* Methods */
        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += OnActiveViewsCollectionChanged;
        }

        /* Helpers */
        private void OnActiveViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newView in e.NewItems)
                {
                    var viewList = new List<DependentViewInfo>();

                    if (_dependentViewCache.ContainsKey(newView))
                    {
                        viewList = _dependentViewCache[newView];
                    }
                    else
                    {
                        foreach (var atr in newView.GetType().GetCustomAttributes<DependentViewAttribute>())
                        {
                            var info = GetDependentView(atr);

                            if (info.View is IDependentViewDataContext dependentView && newView is IDependentViewDataContext view)
                            {
                                dependentView.DataContext = view.DataContext;
                            }
                            viewList.Add(info);
                        }

                        if (!_dependentViewCache.ContainsKey(newView))
                        {
                            _dependentViewCache.Add(newView, viewList);
                        }
                    }
                    viewList.ForEach(x => Region.RegionManager.Regions[x.TargetRegionName].Add(x.View));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldView in e.OldItems)
                {
                    if (_dependentViewCache.ContainsKey(oldView))
                    {
                        _dependentViewCache[oldView].ForEach(x => Region.RegionManager.Regions[x.TargetRegionName].Remove(x.View));
                        if (!ShouldKeepAlive(oldView))
                        {
                            _dependentViewCache.Remove(oldView);
                        }
                    }
                }
            }
        }
        private bool ShouldKeepAlive(object oldView)
        {
            var lifeTIme = GetItemOrContextLifeTime(oldView);
            if (lifeTIme != null)
            {
                return lifeTIme.KeepAlive;
            }

            var lifetimeAttribute = GetItemOrContextLifeTimeAttribute(oldView);
            if (lifetimeAttribute != null)
            {
                return lifetimeAttribute.KeepAlive;
            }
            return true;
        }
        private RegionMemberLifetimeAttribute GetItemOrContextLifeTimeAttribute(object oldView)
        {
            var lifeTimeAttribute = oldView.GetType().GetCustomAttributes<RegionMemberLifetimeAttribute>().FirstOrDefault();
            if (lifeTimeAttribute != null)
            {
                return lifeTimeAttribute;
            }

            if (oldView is FrameworkElement frameworkElement && frameworkElement.DataContext != null)
            {
                var dataContext = frameworkElement.DataContext;
                var contextLifeTimeAttribute = dataContext.GetType().GetCustomAttributes<RegionMemberLifetimeAttribute>().FirstOrDefault();
                return contextLifeTimeAttribute;
            }
            return null;
        }
        private IRegionMemberLifetime GetItemOrContextLifeTime(object oldView)
        {
            if (oldView is IRegionMemberLifetime regionLifeTime)
            {
                return regionLifeTime;
            }

            if (oldView is FrameworkElement frameworkElement && frameworkElement.DataContext is IRegionMemberLifetime dataContextLifeTime)
            {
                return dataContextLifeTime;
            }

            return null;
        }
        private DependentViewInfo GetDependentView(DependentViewAttribute attribute)
        {
            var info = new DependentViewInfo
            {
                TargetRegionName = attribute.TargetRegionName,
                View = attribute.ViewType != null ? _containerProvider.Resolve(attribute.ViewType) : null
            };
            return info;
        }
        private class DependentViewInfo
        {
            public object View { get; set; }
            public string TargetRegionName { get; set; }
        }
    }
}
