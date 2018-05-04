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
        /// Identifies the <see cref="GraphAccessToken"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GraphAccessTokenProperty = DependencyProperty.Register(
            nameof(GraphAccessToken),
            typeof(string),
            typeof(ProfileCard),
            new PropertyMetadata(string.Empty, OnGraphAccessTokenChanged));

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
            new PropertyMetadata(ViewType.PictureOnly, null));

        /// <summary>
        /// Identifies the <see cref="DefaultImage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultImageProperty = DependencyProperty.Register(
            nameof(DefaultImage),
            typeof(BitmapImage),
            typeof(ProfileCard),
            new PropertyMetadata(null, OnDefaultImageChanged));

        /// <summary>
        /// Identifies the <see cref="DefaultTitleText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultTitleTextProperty = DependencyProperty.Register(
            nameof(DefaultTitleText),
            typeof(string),
            typeof(ProfileCard),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="DefaultSecondaryMailText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultSecondaryMailTextProperty = DependencyProperty.Register(
            nameof(DefaultSecondaryMailText),
            typeof(string),
            typeof(ProfileCard),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="DefaultMailText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultMailTextProperty = DependencyProperty.Register(
            nameof(DefaultMailText),
            typeof(string),
            typeof(ProfileCard),
            new PropertyMetadata(string.Empty));

        internal static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(ProfileCard),
            new PropertyMetadata(string.Empty));

        internal static readonly DependencyProperty SecondaryMailProperty = DependencyProperty.Register(
            nameof(SecondaryMail),
            typeof(string),
            typeof(ProfileCard),
            new PropertyMetadata(string.Empty));

        internal static readonly DependencyProperty MailProperty = DependencyProperty.Register(
            nameof(Mail),
            typeof(string),
            typeof(ProfileCard),
            new PropertyMetadata(string.Empty));

        internal static readonly DependencyProperty UserPhotoProperty = DependencyProperty.Register(
            nameof(UserPhoto),
            typeof(BitmapImage),
            typeof(ProfileCard),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets graph access token.
        /// </summary>
        public string GraphAccessToken
        {
            get { return ((string)GetValue(GraphAccessTokenProperty))?.Trim(); }
            set { SetValue(GraphAccessTokenProperty, value?.Trim()); }
        }

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
        public string DefaultTitleText
        {
            get { return ((string)GetValue(DefaultTitleTextProperty))?.Trim(); }
            set { SetValue(DefaultTitleTextProperty, value?.Trim()); }
        }

        /// <summary>
        /// Gets or sets the default secondary mail text in LargeProfilePhotoLeft mode or LargeProfilePhotoRight mode when no user is signed in.
        /// </summary>
        public string DefaultSecondaryMailText
        {
            get { return ((string)GetValue(DefaultSecondaryMailTextProperty))?.Trim(); }
            set { SetValue(DefaultSecondaryMailTextProperty, value?.Trim()); }
        }

        /// <summary>
        /// Gets or sets the default mail text in EmailOnly mode when no user is signed in.
        /// </summary>
        public string DefaultMailText
        {
            get { return ((string)GetValue(DefaultMailTextProperty))?.Trim(); }
            set { SetValue(DefaultMailTextProperty, value?.Trim()); }
        }

        internal string Title
        {
            get { return (string)GetValue(TitleProperty); }
            private set { SetValue(TitleProperty, value); }
        }

        internal string SecondaryMail
        {
            get { return (string)GetValue(SecondaryMailProperty); }
            private set { SetValue(SecondaryMailProperty, value); }
        }

        internal string Mail
        {
            get { return (string)GetValue(MailProperty); }
            private set { SetValue(MailProperty, value); }
        }

        internal BitmapImage UserPhoto
        {
            get { return (BitmapImage)GetValue(UserPhotoProperty); }
            private set { SetValue(UserPhotoProperty, value); }
        }
    }
}
