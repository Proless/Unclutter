using System;

namespace Unclutter.Services.WPF.Dialogs
{
    internal abstract class BaseDialogConfig
    {
        public abstract event Action<BaseDialog> Created;
    }
}
