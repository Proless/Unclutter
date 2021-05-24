using System;
using Prism.Commands;
using System.ComponentModel;
using System.Windows.Data;
using Prism.Regions;
using Unclutter.Modules.Helpers;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK.Common;
using Unclutter.SDK.IModels;

namespace Modules.Profiles.ViewModels
{
    public class GameSelectionViewModel : ViewModelBase
    {
        /* Fields */
        private string _searchKeyword;
        private bool _initialized;
        private SynchronizedObservableCollection<IGameDetails> _games;
        private ICollectionView _gamesView;
        private IGameDetails _selectedGame;


        /* Properties */
        public SynchronizedObservableCollection<IGameDetails> Games
        {
            get => _games;
            set => SetProperty(ref _games, value);
        }
        public ICollectionView GamesView
        {
            get => _gamesView;
            set => SetProperty(ref _gamesView, value);
        }
        public IGameDetails SelectedGame
        {
            get => _selectedGame;
            set => SetProperty(ref _selectedGame, value);
        }
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                SetProperty(ref _searchKeyword, value);
                GamesView.Refresh();
            }
        }

        /* Commands */
        public DelegateCommand ConfirmSelectionCommand => new DelegateCommand(ConfirmSelection, CanConfirmSelection)
            .ObservesProperty(() => SelectedGame);

        /* Constructor */
        public GameSelectionViewModel()
        {
            Games = new SynchronizedObservableCollection<IGameDetails>();
            GamesView = CollectionViewSource.GetDefaultView(Games);
            GamesView.Filter = FilterGames;
        }

        /* Methods */
        public bool CanConfirmSelection()
        {
            return SelectedGame != null;
        }
        public void ConfirmSelection()
        {
            var parameters = new NavigationParameters
            {
                { LocalIdentifiers.Parameters.Game, SelectedGame }
            };
            RegionManager.RequestNavigate(CommonIdentifiers.Regions.Startup, LocalIdentifiers.Views.ProfileCreation, parameters);
        }
        public bool FilterGames(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchKeyword))
            {
                return true;
            }

            if (obj is IGameDetails game)
            {
                return game.Name.IndexOf(SearchKeyword, StringComparison.OrdinalIgnoreCase) != -1;
            }
            return false;
        }
    }
}
