using Prism.Services.Dialogs;
using Unclutter.SDK.Models;

namespace Unclutter
{
    public class ProfileLoadedEvent
    {
        public IUserProfile OldProfile { get; }
        public IUserProfile NewProfile { get; }

        internal ProfileLoadedEvent(IUserProfile newProfile, IUserProfile oldProfile)
        {
            NewProfile = newProfile;
            OldProfile = oldProfile;
        }
    }
    public class CloseStartupDialogEvent
    {
        public DialogResult Result { get; }

        internal CloseStartupDialogEvent(DialogResult result)
        {
            Result = result;
        }
    }
    public class ProfileCreatedEvent
    {
        internal ProfileCreatedEvent()
        {

        }
    }
    public class GameSelectedEvent
    {
        public IGameDetails GameDetails { get; }

        internal GameSelectedEvent(IGameDetails gameDetails)
        {
            GameDetails = gameDetails;
        }
    }
    public class UserSelectedEvent
    {
        public IUserDetails UserDetails { get; }

        internal UserSelectedEvent(IUserDetails userDetails)
        {
            UserDetails = userDetails;
        }
    }
}
