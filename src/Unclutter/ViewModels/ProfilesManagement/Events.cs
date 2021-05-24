using Prism.Events;
using Unclutter.SDK.IModels;

namespace Unclutter.ViewModels.ProfilesManagement
{
    internal class GameSelectedEvent : PubSubEvent<IGameDetails> { }
    internal class UserSelectedEvent : PubSubEvent<IUserDetails> { }
    internal class ProfileCreatedEvent : PubSubEvent { }
}
