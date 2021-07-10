using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Threading.Tasks;
using Unclutter.SDK.Services;

namespace Unclutter.Modules.ViewModels
{
    public abstract class BaseViewModel : BindableBase, IConfirmNavigationRequest, IRegionMemberLifetime
    {
        /* Fields */
        private string _title;
        private bool _keepAlive;

        /* Properties */
        public virtual string Title
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
        public ILocalizationProvider LocalizationProvider { get; }

        /* Commands */
        public DelegateCommand<string> NavigateCommand => new DelegateCommand<string>(Navigate);

        /* Constructors */
        protected BaseViewModel()
        {
            RegionManager = ContainerLocator.Container.Resolve<IRegionManager>();
            EventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
            LocalizationProvider = ContainerLocator.Container.Resolve<ILocalizationProvider>();
            _title = string.Empty;
            _keepAlive = true;
        }

        /* Methods */
        public virtual void Navigate(string uri) { }
        public virtual void OnNavigatedTo(NavigationContext navigationContext) { }
        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }
        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback) => continuationCallback(true);
        public virtual Task OnViewLoaded() => Task.CompletedTask;
    }
}
