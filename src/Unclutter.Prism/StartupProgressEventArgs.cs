using System;

namespace Unclutter.Prism
{
    public class StartupProgressEventArgs : EventArgs
    {
        public string Message { get; }
        public double Progress { get; }

        public StartupProgressEventArgs(string message, double progress)
        {
            Message = message;
            Progress = progress;
        }
    }
}
