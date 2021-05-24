using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using Unclutter.SDK;

namespace Unclutter.AppInstance
{
    public static class AppInstanceManager
    {
        /* Fields */
        private static Mutex _instanceMutex;
        private static int WM_COPYDATA = 0x004A;
        private static IntPtr WM_SUCCESS = new IntPtr(400);

        /* Properties */
        public static event Action<string[]> NewArguments;

        /* Methods */
        public static void VerifyApplicationInstance()
        {
            _instanceMutex = new Mutex(true, $"{Constants.Unclutter}_{{C9502B33-9B04-43EF-ADE0-1C9D7E5925C9}}");

            if (!_instanceMutex.WaitOne(TimeSpan.FromSeconds(0), false))
            {
                SwitchToApplicationInstance();
                Environment.Exit(0);
            }
        }

        public static void SetupApplicationInstance()
        {
            CheckMainWindow();
            Application.Current.MainWindow!.SourceInitialized += OnMainWindowSourceInitialized;
        }

        /* Helpers */
        private static void OnMainWindowSourceInitialized(object sender, EventArgs e)
        {
            Application.Current.MainWindow!.SourceInitialized -= OnMainWindowSourceInitialized;

            var hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(Application.Current.MainWindow!).Handle);
            hwndSource?.AddHook(HandleMessage);
        }

        private static void SwitchToApplicationInstance()
        {
            Win32.EnumWindows(EnumWindowsHandler, IntPtr.Zero);
        }

        private static bool EnumWindowsHandler(IntPtr hwnd, IntPtr lParam)
        {
            var sb = new StringBuilder(256);
            Win32.GetWindowText(hwnd, sb, sb.Capacity);
            if (sb.ToString().StartsWith(Constants.Unclutter + " ", StringComparison.Ordinal))
            {
                var args = Environment.GetCommandLineArgs();
                args[0] = Constants.Unclutter;
                var message = string.Join(Environment.NewLine, args);
                if (SendMessage(hwnd, message) == WM_SUCCESS)
                {
                    Environment.Exit(0);
                }
            }
            return true;
        }

        public static IntPtr HandleMessage(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (message != WM_COPYDATA)
                return IntPtr.Zero;

            var data = GetMessage(message, lParam);

            if (data == null) return IntPtr.Zero;

            if (Application.Current.MainWindow == null)
                return IntPtr.Zero;

            if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
                Application.Current.MainWindow.WindowState = WindowState.Normal;

            Application.Current.MainWindow.Activate();

            Win32.SetForegroundWindow(new WindowInteropHelper(Application.Current.MainWindow).Handle);

            var args = data.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (args[0] == Constants.Unclutter)
            {
                NewArguments?.Invoke(args.Skip(1).ToArray());
                handled = true;
                return WM_SUCCESS;
            }

            handled = false;
            return IntPtr.Zero;
        }

        private static void CheckMainWindow()
        {
            if (Application.Current.MainWindow == null)
            {
                throw new InvalidOperationException($"{nameof(SetupApplicationInstance)} is called before the {nameof(Application.Current.MainWindow)} is initialized, this should never happen !!!");
            }
        }

        public static string GetMessage(int message, IntPtr lParam)
        {
            if (message == WM_COPYDATA)
            {
                try
                {
                    var data = Marshal.PtrToStructure<Win32.COPYDATASTRUCT>(lParam);
                    var result = Marshal.PtrToStringUni(data.lpData);
                    return result;
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public static IntPtr SendMessage(IntPtr hwnd, string message)
        {
            if (hwnd == IntPtr.Zero)
                throw new ArgumentException($"The window handle cannot be IntPtr.Zero", nameof(hwnd));

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("The message cannot be null or empty", nameof(message));

            var messageBytes = Encoding.Unicode.GetBytes(message);
            var data = new Win32.COPYDATASTRUCT
            {
                dwData = new IntPtr(100),
                lpData = Marshal.StringToHGlobalUni(message),
                cbData = messageBytes.Length + 1
            };

            return Win32.SendMessage(hwnd, WM_COPYDATA, IntPtr.Zero, ref data);
        }
    }
}
