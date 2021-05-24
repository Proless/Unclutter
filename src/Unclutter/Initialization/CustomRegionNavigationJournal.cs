using Prism.Regions;
using System.Windows.Threading;
using Unclutter.Services;

namespace Unclutter.Initialization
{
    public class CustomRegionNavigationJournal : IRegionNavigationJournal
    {
        /* Fields */
        private readonly RegionNavigationJournal _regionNavigationJournal;

        /* Constructor */
        public CustomRegionNavigationJournal(RegionNavigationJournal regionNavigationJournal)
        {
            _regionNavigationJournal = regionNavigationJournal;
        }

        /* Methods */
        public void GoBack()
        {
            UIDispatcher.BeginOnUIThread(() => _regionNavigationJournal.GoBack(), DispatcherPriority.Send);
        }

        public void GoForward()
        {
            UIDispatcher.BeginOnUIThread(() => _regionNavigationJournal.GoForward(), DispatcherPriority.Send);
        }

        public void RecordNavigation(IRegionNavigationJournalEntry entry, bool persistInHistory)
        {
            _regionNavigationJournal.RecordNavigation(entry, persistInHistory);
        }

        public void Clear()
        {
            _regionNavigationJournal.Clear();
        }

        public bool CanGoBack => _regionNavigationJournal.CanGoBack;

        public bool CanGoForward => _regionNavigationJournal.CanGoForward;

        public IRegionNavigationJournalEntry CurrentEntry => _regionNavigationJournal.CurrentEntry;

        public INavigateAsync NavigationTarget
        {
            get => _regionNavigationJournal.NavigationTarget;
            set => _regionNavigationJournal.NavigationTarget = value;
        }
    }
}
