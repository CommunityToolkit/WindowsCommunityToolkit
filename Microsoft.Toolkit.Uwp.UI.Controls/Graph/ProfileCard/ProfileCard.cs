using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The Profile Card control is a simple way to display a user in multiple different formats and mixes of name/image/e-mail.
    /// </summary>
    public partial class ProfileCard : Control
    {
        private static readonly BitmapImage PersonPhoto = new BitmapImage(new Uri("ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls/Assets/person.png"));

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileCard"/> class.
        /// </summary>
        public ProfileCard()
        {
            DefaultStyleKey = typeof(ProfileCard);
        }

        /// <summary>
        /// Override default OnApplyTemplate to initialize child controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitUserProfile();
        }

        private static void OnGraphAccessTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as ProfileCard).FetchUserInfo();

        private static void OnUserIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as ProfileCard).FetchUserInfo();

        private static void OnDefaultImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var profileCardControl = d as ProfileCard;
            if (string.IsNullOrEmpty(profileCardControl.UserId))
                profileCardControl._userPhoto = profileCardControl.DefaultImage;
        }

        private async void FetchUserInfo()
        {
            if (string.IsNullOrEmpty(GraphAccessToken) || string.IsNullOrEmpty(UserId))
            {
                InitUserProfile();
            }
            else
            {
                var graphClient = Common.GetAuthenticatedClient(GraphAccessToken);
                var user = await graphClient.Users[UserId].Request().GetAsync();
                _title = user.DisplayName;
                _mail = user.Mail;
                _secondaryMail = user.Mail;
                if (string.IsNullOrEmpty(_mail))
                {
                    _userPhoto = DefaultImage ?? PersonPhoto;
                }
                else
                {
                    try
                    {
                        using (Stream photoStream = await graphClient.Users[UserId].Photo.Content.Request().GetAsync())
                        using (var ras = photoStream.AsRandomAccessStream())
                        {
                            _userPhoto = new BitmapImage();
                            await _userPhoto.SetSourceAsync(ras);
                        }
                    }
                    catch { _userPhoto = DefaultImage ?? PersonPhoto; }
                }
            }
        }

        private void InitUserProfile()
        {
            _userPhoto = DefaultImage ?? PersonPhoto;
            _title = DefaultTitleText ?? "";
            _mail = DefaultMailText ?? "";
            _secondaryMail = DefaultSecondaryMailText ?? "";
        }
    }
}
