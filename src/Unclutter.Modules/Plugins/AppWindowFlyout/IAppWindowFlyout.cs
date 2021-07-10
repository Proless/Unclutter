using System;
using System.ComponentModel.Composition;
using System.Windows;
using Unclutter.Modules.Plugins.AppWindowBar;

namespace Unclutter.Modules.Plugins.AppWindowFlyout
{
    public interface IAppWindowFlyout : IAppWindowPlugin
    {
        /// <summary>
        /// Get the name of the flyout.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Determine if this Flyout should be visible or not.
        /// </summary>
        bool IsOpen { get; set; }

        /// <summary>
        /// Determine if this  Flyout is modal.
        /// </summary>
        bool IsModal { get; }

        /// <summary>
        /// Determine if this Flyout stays open when the user clicks somewhere outside of it.
        /// </summary>
        bool IsPinned { get; }

        /// <summary>
        /// Determine if a  close button is displayed.
        /// </summary>
        bool ShowCloseButton { get; }

        /// <summary>
        /// Get the title of the flyout.
        /// </summary>
        object Title { get; }

        /// <summary>
        /// Called after the flyout is opened.
        /// </summary>
        void OnOpened();

        /// <summary>
        /// Called after the flyout is closed.
        /// </summary>
        void OnClosed();

        VerticalAlignment VerticalContentAlignment { get; }
        HorizontalAlignment HorizontalContentAlignment { get; }

        /// <summary>
        /// Get the Position of the flyout.
        /// </summary>
        AppWindowFlyoutPosition Position { get; }
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ExportAppWindowFlyoutAttribute : ExportAttribute
    {
        public ExportAppWindowFlyoutAttribute() : base(typeof(IAppWindowFlyout)) { }
    }
}
