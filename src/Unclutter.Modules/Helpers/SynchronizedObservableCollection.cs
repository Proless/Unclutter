using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace Unclutter.Modules.Helpers
{
    public class SynchronizedObservableCollection<T> : ObservableCollection<T>
    {
        /* Fields */
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        /* Constructors */
        public SynchronizedObservableCollection() { }
        public SynchronizedObservableCollection(List<T> list) : base(list) { }
        public SynchronizedObservableCollection(IEnumerable<T> collection) : base(collection) { }

        /* Methods */
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                RaiseCollectionChanged(e);
            }
            else
            {
                _synchronizationContext.Send(RaiseCollectionChanged, e);
            }
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                RaisePropertyChanged(e);
            }
            else
            {
                _synchronizationContext.Send(RaisePropertyChanged, e);
            }
        }

        /* Helpers */
        private void RaiseCollectionChanged(object param)
        {
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }
        private void RaisePropertyChanged(object param)
        {
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }
}
