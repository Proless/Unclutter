using System.Threading;

namespace Unclutter.SDK.Progress
{
    public class ProgressController : IProgressController
    {
        /* Fields */
        private readonly IProgressModel _model;

        /* Properties */
        public CancellationTokenSource CancellationTokenSource { get; set; }

        /* Constructor */
        public ProgressController(IProgressModel model)
        {
            _model = model;
        }

        /* Methods */
        public void SetCancelable(bool cancelable)
        {
            _model.IsCancelable = cancelable;
        }
        public void SetIndeterminate(bool indeterminate)
        {
            _model.IsIndeterminate = indeterminate;
        }
        public void SetMessage(string text)
        {
            _model.Message = text;
        }
        public void SetProgress(double progress)
        {
            if (progress >= 0d || progress <= 100d)
            {
                SetIndeterminate(false);
                _model.ProgressValue = progress;
            }
        }
        public void SetTitle(string title)
        {
            _model.Title = title;
        }
    }
}
