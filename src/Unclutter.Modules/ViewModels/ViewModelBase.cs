using System;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace Unclutter.Modules.ViewModels
{
    public abstract class ViewModelBase : BindableBase, IConfirmNavigationRequest, IRegionMemberLifetime
    {
        /* Fields */
        private string _title;
        private bool _keepAlive;

        /* Properties */
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public bool KeepAlive
        {
            get => _keepAlive;
            set => SetProperty(ref _keepAlive, value);
        }

        /* Services */
        public IRegionManager RegionManager { get; }
        public IEventAggregator EventAggregator { get; }

        /* Commands */
        public DelegateCommand<string> NavigateCommand => new DelegateCommand<string>(Navigate);

        /* Constructors */
        protected ViewModelBase()
        {
            RegionManager = ContainerLocator.Container.Resolve<IRegionManager>();
            EventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
            Title = string.Empty;
            KeepAlive = true;
        }

        /* Methods */
        public virtual void Navigate(string uri) { }
        public virtual void OnNavigatedTo(NavigationContext navigationContext) { }
        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }
        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback) => continuationCallback(true);
    }
}
