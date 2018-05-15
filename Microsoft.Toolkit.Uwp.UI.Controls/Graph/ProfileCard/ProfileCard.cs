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
using Microsoft.Graph;
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
        private ContentControl _contentPresenter;

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

            _contentPresenter = GetTemplateChild("ContentPresenter") as ContentControl;
            if (_contentPresenter != null)
            {
                _contentPresenter.ContentTemplateSelector = new ProfileDisplayModeTemplateSelector(_contentPresenter);
            }

            FetchUserInfo();
        }

        private async void FetchUserInfo()
        {
            var profileItem = CurrentProfileItem.Clone();
            profileItem.DisplayMode = DisplayMode;

            if (!AadAuthenticationManager.Instance.IsAuthenticated || string.IsNullOrEmpty(UserId) || UserId.Equals("Invalid UserId"))
            {
                InitUserProfile(profileItem);
            }
            else
            {
                GraphServiceClient graphClient = await AadAuthenticationManager.Instance.GetGraphServiceClientAsync();
                try
                {
                    var user = await graphClient.Users[UserId].Request().GetAsync();
                    profileItem.NormalMail = user.Mail;
                    profileItem.LargeProfileTitle = user.DisplayName;
                    profileItem.LargeProfileMail = user.Mail;

                    if (string.IsNullOrEmpty(user.Mail))
                    {
                        profileItem.UserPhoto = DefaultImage ?? PersonPhoto;
                    }
                    else
                    {
                        try
                        {
                            using (Stream photoStream = await graphClient.Users[UserId].Photo.Content.Request().GetAsync())
                            using (var ras = photoStream.AsRandomAccessStream())
                            {
                                var bitmapImage = new BitmapImage();
                                await bitmapImage.SetSourceAsync(ras);
                                profileItem.UserPhoto = bitmapImage;
                            }
                        }
                        catch
                        {
                            // Swallow error in case of no photo found
                            profileItem.UserPhoto = DefaultImage ?? PersonPhoto;
                        }
                    }
                }
                catch (ServiceException ex)
                {
                    // Swallow error in case of no user id found
                    if (!ex.Error.Code.Equals("Request_ResourceNotFound"))
                    {
                        throw;
                    }

                    UserId = "Invalid UserId";
                }

                CurrentProfileItem = profileItem;
            }
        }

        private void InitUserProfile(ProfileCardItem profileItem)
        {
            profileItem.UserPhoto = DefaultImage ?? PersonPhoto;
            profileItem.NormalMail = NormalMailDefaultText ?? string.Empty;
            profileItem.LargeProfileTitle = LargeProfileTitleDefaultText ?? string.Empty;
            profileItem.LargeProfileMail = LargeProfileMailDefaultText ?? string.Empty;

            CurrentProfileItem = profileItem;
        }
    }
}
