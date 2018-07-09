// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Defines the properties for the <see cref="ProfileCard"/> control.
    /// </summary>
    public partial class ProfileCard : Control
    {
        /// <summary>
        /// Gets required delegated permissions for the <see cref="ProfileCard"/> control
        /// </summary>
        public static string[] RequiredDelegatedPermissions
        {
            get
            {
                return new string[] { "User.Read", "User.ReadBasic.All" };
            }
        }

        /// <summary>
        /// Identifies the <see cref="UserId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UserIdProperty = DependencyProperty.Register(
            nameof(UserId),
            typeof(string),
            typeof(ProfileCard),
            new PropertyMetadata(string.Empty, OnUserIdPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="DisplayMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register(
            nameof(DisplayMode),
            typeof(ViewType),
            typeof(ProfileCard),
            new PropertyMetadata(ViewType.PictureOnly, OnDisplayModePropertyChanged));

        /// <summary>
        /// Identifies the <see cref="DefaultImage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultImageProperty = DependencyProperty.Register(
            nameof(DefaultImage),
            typeof(BitmapImage),
            typeof(ProfileCard),
            new PropertyMetadata(null, OnDefaultValuePropertyChanged));

        /// <summary>
        /// Identifies the <see cref="LargeProfileTitleDefaultText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LargeProfileTitleDefaultTextProperty = DependencyProperty.Register(
            nameof(LargeProfileTitleDefaultText),
            typeof(string),
            typeof(ProfileCard),
            new PropertyMetadata(string.Empty, OnDefaultValuePropertyChanged));

        /// <summary>
        /// Identifies the <see cref="LargeProfileMailDefaultText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LargeProfileMailDefaultTextProperty = DependencyProperty.Register(
            nameof(LargeProfileMailDefaultText),
            typeof(string),
            typeof(ProfileCard),
            new PropertyMetadata(string.Empty, OnDefaultValuePropertyChanged));

        /// <summary>
        /// Identifies the <see cref="NormalMailDefaultText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NormalMailDefaultTextProperty = DependencyProperty.Register(
            nameof(NormalMailDefaultText),
            typeof(string),
            typeof(ProfileCard),
            new PropertyMetadata(string.Empty, OnDefaultValuePropertyChanged));

        internal static readonly DependencyProperty CurrentProfileItemProperty = DependencyProperty.Register(
            nameof(CurrentProfileItem),
            typeof(ProfileCardItem),
            typeof(ProfileCard),
            new PropertyMetadata(new ProfileCardItem()));

        /// <summary>
        /// Gets or sets user unique identifier.
        /// </summary>
        public string UserId
        {
            get { return ((string)GetValue(UserIdProperty))?.Trim(); }
            set { SetValue(UserIdProperty, value?.Trim()); }
        }

        /// <summary>
        /// Gets or sets the visual layout of the control. Default is PictureOnly.
        /// </summary>
        public ViewType DisplayMode
        {
            get { return (ViewType)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the default image when no user is signed in.
        /// </summary>
        public BitmapImage DefaultImage
        {
            get { return (BitmapImage)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the default title text in LargeProfilePhotoLeft mode or LargeProfilePhotoRight mode when no user is signed in.
        /// </summary>
        public string LargeProfileTitleDefaultText
        {
            get { return (string)GetValue(LargeProfileTitleDefaultTextProperty); }
            set { SetValue(LargeProfileTitleDefaultTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the default secondary mail text in LargeProfilePhotoLeft mode or LargeProfilePhotoRight mode when no user is signed in.
        /// </summary>
        public string LargeProfileMailDefaultText
        {
            get { return (string)GetValue(LargeProfileMailDefaultTextProperty); }
            set { SetValue(LargeProfileMailDefaultTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the default mail text in EmailOnly mode when no user is signed in.
        /// </summary>
        public string NormalMailDefaultText
        {
            get { return (string)GetValue(NormalMailDefaultTextProperty); }
            set { SetValue(NormalMailDefaultTextProperty, value); }
        }

        internal ProfileCardItem CurrentProfileItem
        {
            get { return (ProfileCardItem)GetValue(CurrentProfileItemProperty); }
            set { SetValue(CurrentProfileItemProperty, value); }
        }
    }
}
