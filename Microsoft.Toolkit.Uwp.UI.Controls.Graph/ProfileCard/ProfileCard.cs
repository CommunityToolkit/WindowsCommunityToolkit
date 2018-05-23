// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The Profile Card control is a simple way to display a user in multiple different formats and mixes of name/image/e-mail.
    /// </summary>
    public partial class ProfileCard : Control
    {
        private static readonly BitmapImage PersonPhoto = new BitmapImage(new Uri("ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls.Graph/Assets/person.png"));
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
            if (string.IsNullOrEmpty(UserId) || UserId.Equals("Invalid UserId"))
            {
                InitUserProfile();
            }
            else
            {
                var graphService = MicrosoftGraphService.Instance;
                if (!(await graphService.TryLoginAsync()))
                {
                    return;
                }

                try
                {
                    var user = await graphService.GraphProvider.Users[UserId].Request().GetAsync();
                    var profileItem = new ProfileCardItem()
                    {
                        NormalMail = user.Mail,
                        LargeProfileTitle = user.DisplayName,
                        LargeProfileMail = user.Mail,
                        DisplayMode = DisplayMode
                    };

                    if (string.IsNullOrEmpty(user.Mail))
                    {
                        profileItem.UserPhoto = DefaultImage ?? PersonPhoto;
                    }
                    else
                    {
                        try
                        {
                            using (Stream photoStream = await graphService.GraphProvider.Users[UserId].Photo.Content.Request().GetAsync())
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

                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        CurrentProfileItem = profileItem;
                    });
                }
                catch (ServiceException ex)
                {
                    // Swallow error in case of no user id found
                    if (!ex.Error.Code.Equals("Request_ResourceNotFound"))
                    {
                        throw;
                    }

                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        UserId = "Invalid UserId";
                    });
                }
            }
        }

        private void InitUserProfile()
        {
            var profileItem = new ProfileCardItem()
            {
                UserPhoto = DefaultImage ?? PersonPhoto,
                NormalMail = NormalMailDefaultText ?? string.Empty,
                LargeProfileTitle = LargeProfileTitleDefaultText ?? string.Empty,
                LargeProfileMail = LargeProfileMailDefaultText ?? string.Empty,
                DisplayMode = DisplayMode
            };

            CurrentProfileItem = profileItem;
        }
    }
}
