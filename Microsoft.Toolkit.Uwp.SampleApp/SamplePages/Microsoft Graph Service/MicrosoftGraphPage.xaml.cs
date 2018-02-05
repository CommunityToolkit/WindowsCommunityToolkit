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
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Uwp.Services.MicrosoftGraph;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to signin to office 365, retrieve user's profile and user's emails
    /// </summary>
    public sealed partial class MicrosoftGraphPage : Page
    {
        public MicrosoftGraphPage()
        {
            InitializeComponent();
            EventsBox.Visibility = Visibility.Collapsed;
            MessagesBox.Visibility = Visibility.Collapsed;
            UserBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Collapsed;
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            if (string.IsNullOrEmpty(ClientId.Text))
            {
                return;
            }

            // Initialize the service
            MicrosoftGraphService.Instance.Initialize(ClientId.Text);

            // Login via Azure Active Directory
            if (!await MicrosoftGraphService.Instance.LoginAsync())
            {
                var error = new MessageDialog("Unable to sign in to Office 365");
                await error.ShowAsync();
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            try
            {
                // Retrieve user's info from Azure Active Directory
                var user = await MicrosoftGraphService.Instance.User.GetProfileAsync();
                UserPanel.DataContext = user;

                using (IRandomAccessStream photoStream = await MicrosoftGraphService.Instance.User.GetPhotoAsync())
                {
                    BitmapImage photo = new BitmapImage();
                    if (photoStream != null)
                    {
                        await photo.SetSourceAsync(photoStream);
                    }
                    else
                    {
                        photo.UriSource = new Uri("ms-appx:///SamplePages/MicrosoftGraph Service/user.png");
                    }

                    Photo.Source = photo;
                }
            }
            catch (ServiceException ex)
            {
                await DisplayAuthorizationErrorMessageAsync(ex, "Sign in and read user profile");
            }
            finally
            {
                Shell.Current.DisplayWaitRing = false;
            }

            EventsBox.Visibility = Visibility.Visible;
            MessagesBox.Visibility = Visibility.Visible;
            UserBox.Visibility = Visibility.Visible;
            ClientIdBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Visible;
            ConnectButton.Visibility = Visibility.Collapsed;
        }

        private async void GetEventsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            int top = 10;

            var collection = new IncrementalLoadingCollection<MicrosoftGraphSource<Event>, Event>(
                top,
                async () =>
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Shell.Current.DisplayWaitRing = true; });
                },
                async () =>
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Shell.Current.DisplayWaitRing = false; });
                },
                async ex =>
                {
                    if (!Dispatcher.HasThreadAccess)
                    {
                        if (ex is ServiceException)
                        {
                            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => { await DisplayAuthorizationErrorMessageAsync(ex as ServiceException, "Read user event"); });
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                });

            EventsList.ItemsSource = collection;
        }

        private async void GetMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            string txtTop = TopText.Text;
            int top = 0;
            if (string.IsNullOrEmpty(txtTop))
            {
                top = 10;
            }
            else
            {
                top = Convert.ToInt32(txtTop);
            }

            var collection = new IncrementalLoadingCollection<MicrosoftGraphSource<Message>, Message>(
                top,
                async () =>
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Shell.Current.DisplayWaitRing = true; });
                },
                async () =>
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Shell.Current.DisplayWaitRing = false; });
                },
                async ex =>
                {
                    if (!Dispatcher.HasThreadAccess)
                    {
                        if (ex is ServiceException)
                        {
                            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => { await DisplayAuthorizationErrorMessageAsync(ex as ServiceException, "Read user mail"); });
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                });

            MessagesList.ItemsSource = collection;
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessageContentDialog sendMessageDialog = new SendMessageContentDialog();
            await sendMessageDialog.ShowAsync();
        }

        private Task DisplayAuthorizationErrorMessageAsync(ServiceException ex, string additionalMessage)
        {
            MessageDialog error;

            if (ex.Error.Code.Equals("ErrorAccessDenied"))
            {
                error = new MessageDialog($"{ex.Error.Code}\nCheck in Azure Active Directory portal the '{additionalMessage}' Delegated Permissions");
            }
            else
            {
                error = new MessageDialog(ex.Error.Message);
            }

            return error.ShowAsync().AsTask();
        }

        private void ClientIdExpandButton_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityStatusPanel(ClientIdBox, (Button)sender);
        }

        private void UserBoxExpandButton_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityStatusPanel(UserPanel, (Button)sender);
        }

        private void MessageBoxExpandButton_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityStatusPanel(MessagesPanel, (Button)sender);
            SetVisibilityStatusPanel(MessagesList, (Button)sender);
        }

        private void EventBoxExpandButton_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityStatusPanel(EventsPanel, (Button)sender);
            SetVisibilityStatusPanel(EventsList, (Button)sender);
        }

        private void SetVisibilityStatusPanel(FrameworkElement box, Button switchButton)
        {
            if (box.Visibility == Visibility.Visible)
            {
                switchButton.Content = "";
                box.Visibility = Visibility.Collapsed;
            }
            else
            {
                switchButton.Content = "";
                box.Visibility = Visibility.Visible;
            }
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            MessagesList.LoadMoreItemsAsync().Cancel();
            MicrosoftGraphService.Instance.Logout();
            EventsList.Visibility = Visibility.Collapsed;
            EventsBox.Visibility = Visibility.Collapsed;
            MessagesList.Visibility = Visibility.Collapsed;
            MessagesBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Collapsed;
            UserBox.Visibility = Visibility.Collapsed;
            ClientIdBox.Visibility = Visibility.Visible;
            ConnectButton.Visibility = Visibility.Visible;
        }
    }
}
