// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
        private static readonly BitmapImage PersonPhoto = new BitmapImage(new Uri("ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls/Graph/Assets/person.png"));

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
            {
                profileCardControl.UserPhoto = profileCardControl.DefaultImage;
            }
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
                Title = user.DisplayName;
                Mail = user.Mail;
                SecondaryMail = user.Mail;
                if (string.IsNullOrEmpty(Mail))
                {
                    UserPhoto = DefaultImage ?? PersonPhoto;
                }
                else
                {
                    try
                    {
                        using (Stream photoStream = await graphClient.Users[UserId].Photo.Content.Request().GetAsync())
                        using (var ras = photoStream.AsRandomAccessStream())
                        {
                            UserPhoto = new BitmapImage();
                            await UserPhoto.SetSourceAsync(ras);
                        }
                    }
                    catch
                    {
                        UserPhoto = DefaultImage ?? PersonPhoto;
                    }
                }
            }
        }

        private void InitUserProfile()
        {
            UserPhoto = DefaultImage ?? PersonPhoto;
            Title = DefaultTitleText ?? string.Empty;
            Mail = DefaultMailText ?? string.Empty;
            SecondaryMail = DefaultSecondaryMailText ?? string.Empty;
        }
    }
}
