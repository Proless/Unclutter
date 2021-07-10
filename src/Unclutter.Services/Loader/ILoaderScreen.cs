using System;
using Unclutter.SDK.Images;
using Unclutter.SDK.Progress;

namespace Unclutter.Services.Loader
{
    public interface ILoaderScreen
    {
        event EventHandler Closed;
        public ImageReference Logo { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Footer { get; set; }
        void Show();
        void Hide();
        void Close();
        void ReportProgress(ProgressReport report);
    }
}
