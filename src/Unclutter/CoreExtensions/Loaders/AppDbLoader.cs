using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Unclutter.SDK.Plugins;
using Unclutter.Services.Data;

namespace Unclutter.CoreExtensions.Loaders
{
    [ExportLoader]
    public sealed class AppDbLoader : BaseLoader
    {
        /* Fields */
        private readonly IAppDbProvider _appDbProvider;

        [ImportingConstructor]
        public AppDbLoader(IAppDbProvider appDbProvider)
        {
            _appDbProvider = appDbProvider;
            _appDbProvider.ProgressChanged += OnProgressChanged;
            Order = _appDbProvider.Order;
            LoaderOptions = _appDbProvider.LoaderOptions;

        }

        /* Methods */
        public override Task Load()
        {
            return _appDbProvider.Load();
        }
    }
}
