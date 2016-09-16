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
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
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
            MessagesBox.Visibility = Visibility.Collapsed;
            UserBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Collapsed;
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnection())
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
            catch (Microsoft.Graph.ServiceException ex)
            {
                await DisplayAuthorizationErrorMessage(ex, "Sign in and read user profile");
            }
            finally
            {
                Shell.Current.DisplayWaitRing = false;
            }

            MessagesBox.Visibility = Visibility.Visible;
            UserBox.Visibility = Visibility.Visible;
            ClientIdBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Visible;
            ConnectButton.Visibility = Visibility.Collapsed;
        }

        private async void GetMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnection())
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

            bool isFirstCall = true;

            var incrementalCollectionMessages = new IncrementalCollection<Message>((CancellationToken cts, uint count) =>
              {
                  return Task.Run<ObservableCollection<Message>>(async () =>
                    {
                        IUserMessagesCollectionPage messages = null;
                        ObservableCollection<Message> intermediateList = new ObservableCollection<Message>();

                        if (isFirstCall)
                        {
                            try
                            {
                                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() => { Shell.Current.DisplayWaitRing = true; }));

                                messages = await MicrosoftGraphService.Instance.User.Message.GetEmailsAsync(cts, top);
                            }
                            catch (Microsoft.Graph.ServiceException ex)
                            {
                                if (!Dispatcher.HasThreadAccess)
                                {
                                   await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(async () => { await DisplayAuthorizationErrorMessage(ex, "Read user mail"); }));
                                }
                            }
                            finally
                            {
                                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() => { Shell.Current.DisplayWaitRing = false; }));
                            }

                            isFirstCall = false;
                        }
                        else
                        {
                            if (cts.IsCancellationRequested)
                            {
                                return intermediateList;
                            }
                            messages = await MicrosoftGraphService.Instance.User.Message.NextPageEmailsAsync();
                        }

                        if (messages != null)
                        {
                            messages.AddTo(intermediateList);
                        }

                        return intermediateList;
                    });
              });

            MessagesList.ItemsSource = incrementalCollectionMessages;
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessageContentDialog sendMessageDialog = new SendMessageContentDialog();
            await sendMessageDialog.ShowAsync();
        }

        private async Task DisplayAuthorizationErrorMessage(Microsoft.Graph.ServiceException ex, string additionalMessage)
        {
            MessageDialog error = null;
            if (ex.Error.Code.Equals("ErrorAccessDenied"))
            {
                error = new MessageDialog($"{ex.Error.Code}\nCheck in Azure Active Directory portal the '{additionalMessage}' Delegated Permissions");
            }
            else
            {
                error = new MessageDialog(ex.Error.Message);
            }

            await error.ShowAsync();
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
            MessagesList.Visibility = Visibility.Collapsed;
            MessagesBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Collapsed;
            UserBox.Visibility = Visibility.Collapsed;
            ClientIdBox.Visibility = Visibility.Visible;
            ConnectButton.Visibility = Visibility.Visible;
        }
    }
}
