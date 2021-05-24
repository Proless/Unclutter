using System;
using System.ComponentModel.Composition;
using Unclutter.SDK.Common;
using Unclutter.SDK.Events;
using Unclutter.SDK.Plugins;
using Unclutter.Services.Profiles;

namespace Unclutter.Initialization.EventSources
{
    [ExportEventSource]
    public class ProfileChangedEventSource : IEventSource
    {
        /* Properties */
        public ThreadOption PublishThread => ThreadOption.BackgroundThread;

        /* Events */
        public event EventHandler<object> EventSourceChanged;

        /* Constructor */
        [ImportingConstructor]
        public ProfileChangedEventSource(IProfilesManager profilesManager)
        {
            profilesManager.ProfileChanged += OnProfileChanged;
        }

        /* Methods */
        public void OnProfileChanged(ProfileChangedArgs args)
        {
            EventSourceChanged?.Invoke(this, new ProfileChangedEvent(args.NewProfile, args.OldProfile));
        }
    }
}
