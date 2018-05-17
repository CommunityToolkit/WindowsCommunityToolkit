﻿// ******************************************************************
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
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Toolkit.Uwp.Services.MicrosoftGraph;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using static Microsoft.Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums;

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

        private async Task<bool> CanLoginAsync()
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return false;
            }

            if (string.IsNullOrEmpty(ClientId.Text))
            {
                return false;
            }

            return true;
        }

        private void Initialize(AuthenticationModel endpointVersion)
        {
            MicrosoftGraphService.Instance.AuthenticationModel = endpointVersion;

            // Initialize the service
            switch (endpointVersion)
            {
                case AuthenticationModel.V1:
                    MicrosoftGraphService.Instance.Initialize(ClientId.Text);
                    break;
                case AuthenticationModel.V2:
                    var scopes = DelegatedPermissionScopes.Text.Split(' ');
                    MicrosoftGraphService.Instance.Initialize(ClientId.Text, ServicesToInitialize.Message | ServicesToInitialize.UserProfile | ServicesToInitialize.Event, scopes);
                    break;
                default:
                    break;
            }
        }

        private async void RemoteConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await CanLoginAsync())
            {
                return;
            }

            // Initialize the service
            Initialize(AuthenticationModel.V1);

            try
            {
                // Initialize the device code
                await MicrosoftGraphService.Instance.InitializeDeviceCodeAsync();
            }
            catch (IdentityModel.Clients.ActiveDirectory.AdalException adalException)
            {
                var error = new MessageDialog($"The Client Id is invalid.\n{adalException.Message}");
                await error.ShowAsync();
                return;
            }

            var popup = new ContentDialog
            {
                Content = CreatePopupContent(),
                Title = "Pending authentication...",
                CloseButtonText = "Cancel"
            };

            popup.ShowAsync().GetResults();

            if (await LoginAsync())
            {
                popup.Hide();
                await LoadProfileAsync();
            }
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await CanLoginAsync())
            {
                return;
            }

            var item = VersionEndpointDropdown.SelectedItem as ComboBoxItem;
            var endpointVersion = item.Tag.ToString() == "v2" ? AuthenticationModel.V2 : AuthenticationModel.V1;

            // Initialize the service
            Initialize(endpointVersion);

            if (await LoginAsync())
            {
                await LoadProfileAsync();
            }
        }

        private async Task<bool> LoginAsync()
        {
            // Login via Azure Active Directory
            try
            {
                var upn = LoginHint.Text;

                if (!await MicrosoftGraphService.Instance.LoginAsync(upn))
                {
                    var error = new MessageDialog("Unable to sign in to Office 365");
                    await error.ShowAsync();
                    return false;
                }
            }
            catch (AdalServiceException ase)
            {
                var error = new MessageDialog(ase.Message);
                await error.ShowAsync();
                return false;
            }
            catch (AdalException ae)
            {
                var error = new MessageDialog(ae.Message);
                await error.ShowAsync();
                return false;
            }
            catch (MsalServiceException mse)
            {
                var error = new MessageDialog(mse.Message);
                await error.ShowAsync();
                return false;
            }
            catch (MsalException me)
            {
                var error = new MessageDialog(me.Message);
                await error.ShowAsync();
                return false;
            }

            return true;
        }

        private async Task LoadProfileAsync()
        {
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
            RemoteConnectButton.Visibility = Visibility.Collapsed;
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

        private async void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            MessagesList.LoadMoreItemsAsync().Cancel();
            await MicrosoftGraphService.Instance.Logout();
            EventsList.Visibility = Visibility.Collapsed;
            EventsBox.Visibility = Visibility.Collapsed;
            MessagesList.Visibility = Visibility.Collapsed;
            MessagesBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Collapsed;
            UserBox.Visibility = Visibility.Collapsed;
            ClientIdBox.Visibility = Visibility.Visible;
            ConnectButton.Visibility = Visibility.Visible;
            RemoteConnectButton.Visibility = Visibility.Visible;
        }

        private void VersionEndpointDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = VersionEndpointDropdown.SelectedItem as ComboBoxItem;
            if (DelegatedPermissionScopes != null)
            {
                DelegatedPermissionScopes.Visibility = item.Tag.ToString() == "v2" ? Visibility.Visible : Visibility.Collapsed;
                LoginHint.Visibility = DelegatedPermissionScopes.Visibility;
            }
        }

        private StackPanel CreatePopupContent()
        {
            var textStart = new TextBlock() { Text = "Go to " };
            var textEnd = new TextBlock() { Text = " and enter the following code :" };

            var link = new HyperlinkButton()
            {
                Content = "http://aka.ms/devicelogin",
                NavigateUri = new Uri("http://aka.ms/devicelogin", UriKind.Absolute),
                VerticalAlignment = VerticalAlignment.Top,
                VerticalContentAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 0, 0, 0),
                Padding = new Thickness(0)
            };

            var codeContentTb = new StackPanel { Orientation = Orientation.Horizontal };
            codeContentTb.Children.Add(textStart);
            codeContentTb.Children.Add(link);
            codeContentTb.Children.Add(textEnd);

            var codeTb = new TextBox()
            {
                IsReadOnly = true,
                Text = MicrosoftGraphService.Instance.UserCode,
                HorizontalAlignment = HorizontalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontSize = 25,
                Background = new SolidColorBrush() { Color = Colors.Transparent },
                BorderThickness = new Thickness(0)
            };

            var stack = new StackPanel { HorizontalAlignment = HorizontalAlignment.Stretch };
            stack.Children.Add(codeContentTb);
            stack.Children.Add(codeTb);

            return stack;
        }
    }
}
