using System;

namespace Unclutter.SDK.Services
{
    public interface IClientServices
    {
        void OpenInDefaultApp(string name);
        string OpenFolderSelectionDialog(string description = "", Environment.SpecialFolder startFolder = Environment.SpecialFolder.MyComputer);
    }
}
