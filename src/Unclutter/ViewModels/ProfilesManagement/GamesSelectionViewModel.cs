using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Unclutter.Modules.Helpers;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK;
using Unclutter.SDK.Common;
using Unclutter.SDK.IModels;
using Unclutter.SDK.Loader;
using Unclutter.Services.Games;
using Unclutter.Services.Localization;

namespace Unclutter.ViewModels.ProfilesManagement
{
    public class GamesSelectionViewModel : ViewModelBase, ILoader
    {
        /* Services */
        private readonly IGamesProvider _gamesProvider;

        /* Fields */
        private string _searchKeyword;
        private string _gameHint;
        private bool _isLoading;
        private IGameDetails _selectedGame;
        private SynchronizedObservableCollection<IGameDetails> _games;

        /* Properties */
        public SynchronizedObservableCollection<IGameDetails> Games
        {
            get => _games;
            set => SetProperty(ref _games, value);
        }
        public IGameDetails SelectedGame
        {
            get => _selectedGame;
            set
            {
                SetProperty(ref _selectedGame, value);
                EventAggregator.GetEvent<GameSelectedEvent>().Publish(value);
            }
        }
        public string GameHint
        {
            get => _gameHint;
            set => SetProperty(ref _gameHint, value);
        }
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                SetProperty(ref _searchKeyword, value);
                CollectionViewSource.GetDefaultView(Games).Refresh();
            }
        }
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        /* Constructor */
        public GamesSelectionViewModel(IGamesProvider gamesProvider)
        {
            _gamesProvider = gamesProvider;

            Games = new SynchronizedObservableCollection<IGameDetails>();
            GameHint = "Loading...";
            ProgressChanged += OnProgressChanged;
            CollectionViewSource.GetDefaultView(Games).Filter += FilterGame;
        }

        /* Helpers */
        private void OnProgressChanged(ProgressReport report)
        {
            if (report.Status == OperationStatus.Completed)
            {
                GameHint = LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Games_TextBox_Search_Hint);
                IsLoading = false;
            }
            else
            {
                GameHint = report.Message;
                IsLoading = true;
            }
        }
        private bool FilterGame(object obj)
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

        #region ILoader
        public event Action<ProgressReport> ProgressChanged;
        public LoadOptions LoaderOptions => new LoadOptions();
        public async Task Load()
        {
            ProgressChanged?.Invoke(new ProgressReport(LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Games_Loading)));
            var count = 0;
            var games = await Task.Run(() => _gamesProvider.EnumerateGames().OrderByDescending(g => g.Downloads).ToList());
            foreach (var game in games)
            {
                var progress = count / (games.Count * 100d);
                ProgressChanged?.Invoke(new ProgressReport(string.Format(LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Loading_Status), game.Name), progress));
                Games.Add(game);
                count++;
            }
            ProgressChanged?.Invoke(new ProgressReport("", 0d, OperationStatus.Completed));
        }
        #endregion
    }
}
