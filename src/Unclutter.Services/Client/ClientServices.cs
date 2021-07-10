using Ookii.Dialogs.Wpf;
using System;
using System.Diagnostics;
using Unclutter.SDK.Common;
using Unclutter.SDK.Services;

namespace Unclutter.Services.Client
{
    public class ClientServices : IClientServices
    {
        /* Fields */
        private readonly ILogger _logger;

        /* Constructor */
        public ClientServices(ILogger logger)
        {
            _logger = logger;
        }

        /* Methods */
        public void OpenInDefaultApp(string name)
        {
            var info = new ProcessStartInfo { UseShellExecute = true, FileName = name };
            try
            {
                Process.Start(info);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error when opening {name}", name);
            }
        }

        public string OpenFolderSelectionDialog(string description = "", Environment.SpecialFolder startFolder = Environment.SpecialFolder.MyComputer)
        {
            var browseDlg = new VistaFolderBrowserDialog
            {
                Description = description,
                ShowNewFolderButton = true,
                UseDescriptionForTitle = true,
                RootFolder = startFolder
            };

            browseDlg.ShowDialog();

            return browseDlg.SelectedPath;
        }
    }
}
