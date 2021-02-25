namespace Unclutter.Services.WPF.Dialogs
{
    internal class Dialog : BaseDialog
    {
        /* Fields */
        private string _leftButtonLabel;
        private string _rightButtonLabel;
        private string _optionLabel;
        private string _midButtonLabel;

        /* Properties */
        public string LeftButtonLabel
        {
            get => _leftButtonLabel;
            set => SetProperty(ref _leftButtonLabel, value);
        }
        public string MidButtonLabel
        {
            get => _midButtonLabel;
            set => SetProperty(ref _midButtonLabel, value);
        }
        public string RightButtonLabel
        {
            get => _rightButtonLabel;
            set => SetProperty(ref _rightButtonLabel, value);
        }
        public string OptionLabel
        {
            get => _optionLabel;
            set => SetProperty(ref _optionLabel, value);
        }

        /* Constructor */
        public Dialog()
        {
            LeftButtonLabel = null;
            RightButtonLabel = null;
            OptionLabel = null;
        }
    }
}
