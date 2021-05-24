namespace Unclutter.Services.Notifications
{
    internal class Notification : BaseNotification
    {
        /* Fields */
        private string _iconId;
        private string _leftButtonText;
        private string _rightButtonText;
        private string _optionText;

        /* Properties */
        public string IconId
        {
            get => _iconId;
            set => SetProperty(ref _iconId, value);
        }
        public string LeftButtonText
        {
            get => _leftButtonText;
            set => SetProperty(ref _leftButtonText, value);
        }
        public string RightButtonText
        {
            get => _rightButtonText;
            set => SetProperty(ref _rightButtonText, value);
        }
        public string OptionText
        {
            get => _optionText;
            set => SetProperty(ref _optionText, value);
        }

        /* Constructor */
        public Notification()
        {
            IconId = null;
            LeftButtonText = null;
            RightButtonText = null;
            OptionText = null;
        }
    }
}
