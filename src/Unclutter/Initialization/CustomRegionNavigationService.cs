using Prism.Regions;
using System;
using System.Windows.Threading;
using Unclutter.Services;

namespace Unclutter.Initialization
{
    public class CustomRegionNavigationService : IRegionNavigationService, INavigateAsync
    {
        /* Fields */
        private readonly RegionNavigationService _regionNavigationServiceImpl;

        /* Constructor */
        public CustomRegionNavigationService(RegionNavigationService regionNavigationServiceImpl)
        {
            _regionNavigationServiceImpl = regionNavigationServiceImpl;
        }

        /* Methods */
        public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback)
        {
            UIDispatcher.BeginOnUIThread(() => _regionNavigationServiceImpl.RequestNavigate(target, navigationCallback), DispatcherPriority.Send);
        }

        public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters)
        {
            UIDispatcher.BeginOnUIThread(() => _regionNavigationServiceImpl.RequestNavigate(target, navigationCallback, navigationParameters), DispatcherPriority.Send);
        }

        public IRegion Region
        {
            get => _regionNavigationServiceImpl.Region;
            set => _regionNavigationServiceImpl.Region = value;
        }

        public IRegionNavigationJournal Journal => _regionNavigationServiceImpl.Journal;

        public event EventHandler<RegionNavigationEventArgs> Navigating
        {
            add => _regionNavigationServiceImpl.Navigating += value;
            remove => _regionNavigationServiceImpl.Navigating -= value;
        }

        public event EventHandler<RegionNavigationEventArgs> Navigated
        {
            add => _regionNavigationServiceImpl.Navigated += value;
            remove => _regionNavigationServiceImpl.Navigated -= value;
        }

        public event EventHandler<RegionNavigationFailedEventArgs> NavigationFailed
        {
            add => _regionNavigationServiceImpl.NavigationFailed += value;
            remove => _regionNavigationServiceImpl.NavigationFailed -= value;
        }
    }
}
