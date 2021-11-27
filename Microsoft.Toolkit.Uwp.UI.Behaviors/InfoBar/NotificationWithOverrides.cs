using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// An extended notification request.
    /// It allows the user to override different parts of the targeted <see cref="Microsoft.UI.Xaml.Controls.InfoBar"/>.
    /// Only the explicitly set properties will be applied to the InfoBar.
    /// </summary>
    public class NotificationWithOverrides : Notification
    {
        private NotificationOverrides _overrides;
        private bool _isIconVisible;
        private object _content;
        private DataTemplate _contentTemplate;
        private ButtonBase _actionButton;

        /// <summary>
        /// Gets or sets a value indicating whether the icon is visible or not.
        /// True if the icon is visible; otherwise, false. The default is true.
        /// </summary>
        public bool IsIconVisible
        {
            get => _isIconVisible;
            set
            {
                _isIconVisible = value;
                _overrides |= NotificationOverrides.Icon;
            }
        }

        /// <summary>
        /// Gets or sets the XAML Content that is displayed below the title and message in
        ///  the InfoBar.
        /// </summary>
        public object Content
        {
            get => _content;
            set
            {
                _content = value;
                _overrides |= NotificationOverrides.Content;
            }
        }

        /// <summary>
        /// Gets or sets the data template for the <see cref="Content"/>.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get => _contentTemplate;
            set
            {
                _contentTemplate = value;
                _overrides |= NotificationOverrides.ContentTemplate;
            }
        }

        /// <summary>
        /// Gets or sets the action button of the InfoBar.
        /// </summary>
        public ButtonBase ActionButton
        {
            get => _actionButton;
            set
            {
                _actionButton = value;
                _overrides |= NotificationOverrides.ActionButton;
            }
        }

        internal NotificationOverrides Overrides => _overrides;
    }

    /// <summary>
    /// The overrides which should be set on the notification.
    /// </summary>
    [Flags]
    internal enum NotificationOverrides
    {
        None,
        Icon,
        Content,
        ContentTemplate,
        ActionButton,

    }
}
