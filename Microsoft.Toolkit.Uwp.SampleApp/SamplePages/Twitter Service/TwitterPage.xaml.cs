// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Microsoft.Toolkit.Uwp.Services.Twitter;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class TwitterPage
    {
        private ObservableCollection<ITwitterResult> _tweets;

        public TwitterPage()
        {
            InitializeComponent();

            ShareBox.Visibility = Visibility.Collapsed;
            SearchBox.Visibility = Visibility.Collapsed;
            LiveFeedBox.Visibility = Visibility.Collapsed;
            HideSearchPanel();
            HideTweetPanel();
            HideLiveFeedPanel();
        }

        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            if (string.IsNullOrEmpty(ConsumerKey.Text) || string.IsNullOrEmpty(ConsumerSecret.Text) || string.IsNullOrEmpty(CallbackUri.Text))
            {
                return;
            }

            SampleController.Current.DisplayWaitRing = true;
            TwitterService.Instance.Initialize(ConsumerKey.Text, ConsumerSecret.Text, CallbackUri.Text);

            if (!await TwitterService.Instance.LoginAsync())
            {
                ShareBox.Visibility = Visibility.Collapsed;
                SearchBox.Visibility = Visibility.Collapsed;
                SampleController.Current.DisplayWaitRing = false;
                var error = new MessageDialog("Unable to log to Twitter");
                await error.ShowAsync();
                return;
            }

            ShareBox.Visibility = Visibility.Visible;
            SearchBox.Visibility = Visibility.Visible;
            LiveFeedBox.Visibility = Visibility.Visible;

            HideCredentialsPanel();
            ShowSearchPanel();
            ShowTweetPanel();
            ShowLiveFeedPanel();

            TwitterUser user;
            try
            {
                user = await TwitterService.Instance.GetUserAsync();
            }
            catch (TwitterException ex)
            {
                if ((ex.Errors?.Errors?.Length > 0) && (ex.Errors.Errors[0].Code == 89))
                {
                    await new MessageDialog("Invalid or expired token. Logging out. Re-connect for new token.").ShowAsync();
                    TwitterService.Instance.Logout();
                    return;
                }
                else
                {
                    throw ex;
                }
            }

            ProfileImage.DataContext = user;
            ProfileImage.Visibility = Visibility.Visible;

            _tweets = new ObservableCollection<ITwitterResult>(await TwitterService.Instance.GetUserTimeLineAsync(user.ScreenName, 50));
            ListView.ItemsSource = _tweets;

            SampleController.Current.DisplayWaitRing = false;
        }

        private async void GetLocation_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var geolocator = new Geolocator();

                var position = await geolocator.GetGeopositionAsync();

                var pos = position.Coordinate.Point.Position;

                Latitude.Text = pos.Latitude.ToString(CultureInfo.InvariantCulture);
                Longitude.Text = pos.Longitude.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                await new MessageDialog($"An error occured finding your location. Message: {ex.Message}").ShowAsync();
                TrackingManager.TrackException(ex);
            }
        }

        private async void ShareButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            var status = new TwitterStatus
            {
                DisplayCoordinates = DisplayCoordinates.IsChecked == true,
                Message = TweetText.Text,
                Latitude = string.IsNullOrEmpty(Latitude.Text) ? (double?)null : Convert.ToDouble(Latitude.Text),
                Longitude = string.IsNullOrEmpty(Longitude.Text) ? (double?)null : Convert.ToDouble(Longitude.Text)
            };

            SampleController.Current.DisplayWaitRing = true;
            await TwitterService.Instance.TweetStatusAsync(status);
            SampleController.Current.DisplayWaitRing = false;
        }

        private async void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            SampleController.Current.DisplayWaitRing = true;
            ListView.ItemsSource = await TwitterService.Instance.SearchAsync(TagText.Text, 50);
            SampleController.Current.DisplayWaitRing = false;
        }

        private async void SharePictureButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile picture = await openPicker.PickSingleFileAsync();
            if (picture != null)
            {
                using (var stream = await picture.OpenReadAsync())
                {
                    await TwitterService.Instance.TweetStatusAsync(TweetText.Text, stream);
                }
            }
        }

        private void LiveFeedPanel_OnToggled(object sender, RoutedEventArgs e)
        {
            if (LiveFeedToggle.IsOn)
            {
                SampleController.Current.DisplayWaitRing = true;
                GetUserStreams();
                SampleController.Current.DisplayWaitRing = false;
            }
            else
            {
                TwitterService.Instance.StopUserStream();
            }
        }

        private async void GetUserStreams()
        {
            await TwitterService.Instance.StartUserStreamAsync(async tweet =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (tweet != null)
                    {
                        if (tweet is TwitterStreamDeletedEvent)
                        {
                            var toRemove = _tweets.Where(t => t is Tweet)
                                .SingleOrDefault(t => ((Tweet)t).Id == ((TwitterStreamDeletedEvent)tweet).Id);

                            if (toRemove != null)
                            {
                                _tweets.Remove(toRemove);
                            }
                        }
                        else
                        {
                            _tweets.Insert(0, tweet);
                        }
                    }
                });
            });
        }

        private void CredentialsBoxExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (CredentialsBox.Visibility == Visibility.Visible)
            {
                HideCredentialsPanel();
            }
            else
            {
                ShowCredentialsPanel();
            }
        }

        private void TweetBoxExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (TweetPanel.Visibility == Visibility.Visible)
            {
                HideTweetPanel();
            }
            else
            {
                ShowTweetPanel();
            }
        }

        private void SearchBoxExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SearchPanel.Visibility == Visibility.Visible)
            {
                HideSearchPanel();
            }
            else
            {
                ShowSearchPanel();
            }
        }

        private void LiveFeedBoxExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (LiveFeedPanel.Visibility == Visibility.Visible)
            {
                HideLiveFeedPanel();
            }
            else
            {
                ShowLiveFeedPanel();
            }
        }

        private void ShowCredentialsPanel()
        {
            CredentialsBoxExpandButton.Content = "";
            CredentialsBox.Visibility = Visibility.Visible;
        }

        private void HideCredentialsPanel()
        {
            CredentialsBoxExpandButton.Content = "";
            CredentialsBox.Visibility = Visibility.Collapsed;
        }

        private void ShowTweetPanel()
        {
            TweetBoxExpandButton.Content = "";
            TweetPanel.Visibility = Visibility.Visible;
        }

        private void HideTweetPanel()
        {
            TweetBoxExpandButton.Content = "";
            TweetPanel.Visibility = Visibility.Collapsed;
        }

        private void ShowSearchPanel()
        {
            SearchBoxExpandButton.Content = "";
            SearchPanel.Visibility = Visibility.Visible;
        }

        private void HideSearchPanel()
        {
            SearchBoxExpandButton.Content = "";
            SearchPanel.Visibility = Visibility.Collapsed;
        }

        private void ShowLiveFeedPanel()
        {
            LiveFeedBoxExpandButton.Content = "";
            LiveFeedPanel.Visibility = Visibility.Visible;
        }

        private void HideLiveFeedPanel()
        {
            LiveFeedBoxExpandButton.Content = "";
            LiveFeedPanel.Visibility = Visibility.Collapsed;
        }
    }
}
