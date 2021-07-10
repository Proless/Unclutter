using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Unclutter.Services.Dialogs
{
    public static class DialogServiceExtensions
    {
        public static void ShowDialog(this IDialogService dialogService, string name, IDialogParameters parameters, Action onLoaded, Action<IDialogResult> onClosed)
        {
            dialogService.ShowDialog(name, GetParameters(parameters, onLoaded), onClosed);
        }

        public static void ShowDialog(this IDialogService dialogService, string name, IDialogParameters parameters, Action onLoaded, Action<IDialogResult> onClosed, string windowName)
        {
            dialogService.ShowDialog(name, GetParameters(parameters, onLoaded), onClosed, windowName);
        }

        private static IDialogParameters GetParameters(IDialogParameters parameters, Action onLoaded)
        {
            var dlgParameters = new DialogParameters { { ExtendedDialogService.OnLoadedParameterKey, onLoaded } };
            if (parameters != null)
            {
                foreach (var key in parameters.Keys)
                {
                    dlgParameters.Add(key, parameters.GetValue<object>(key));
                }
            }
            return dlgParameters;
        }
    }

    public class ExtendedDialogService : DialogService
    {
        public const string OnLoadedParameterKey = "ExtendedDialogService.OnLoadedAction";

        /* Fields */
        private readonly Dictionary<IDialogWindow, Action> _onLoadedActions = new Dictionary<IDialogWindow, Action>();

        /* Constructor */
        public ExtendedDialogService(IContainerExtension containerExtension) : base(containerExtension) { }

        /* Methods */
        protected override void ConfigureDialogWindowContent(string dialogName, IDialogWindow window, IDialogParameters parameters)
        {
            base.ConfigureDialogWindowContent(dialogName, window, parameters);
            if (parameters.TryGetValue(OnLoadedParameterKey, out Action onLoaded))
            {
                window.Loaded += OnDialogWindowLoaded;
                _onLoadedActions[window] = onLoaded;
            }
        }

        /* Methods */
        private void OnDialogWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is IDialogWindow window)
            {
                if (_onLoadedActions.TryGetValue(window, out var onLoaded))
                {
                    onLoaded?.Invoke();
                    window.Loaded -= OnDialogWindowLoaded;
                    _onLoadedActions.Remove(window);
                }
            }
        }
    }
}
