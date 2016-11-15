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
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Uwp.Services.MicrosoftGraph;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class OneDrivePage : Page
    {
        private OneDriveViewModel _viewModel;

        public OneDrivePage()
        {
            this.InitializeComponent();
            UserBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Collapsed;
            FilesBox.Visibility = Visibility.Collapsed;
            menuButton.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Collapsed;

            _viewModel = new OneDriveViewModel();
            _viewModel.Dispatcher = this.Dispatcher;
            this.DataContext = _viewModel;
        }

        private void UserBoxExpandButton_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityStatusPanel(UserPanel, (Button)sender);
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }
            ClientId.Text = "75f78111-d2b3-4d46-8567-0f641201d236";
            if (string.IsNullOrEmpty(ClientId.Text))
            {
                return;
            }
            
            Shell.Current.DisplayWaitRing = true;
            try
            {
                await _viewModel.SigninAsync(ClientId.Text);
                UserPanel.DataContext = _viewModel.User;
            }
            catch (ServiceException ex)
            {
                await DisplayAuthorizationErrorMessageAsync(ex);
            }
            finally
            {
                Shell.Current.DisplayWaitRing = false;
            }

            FilesBox.Visibility = Visibility.Visible;
            UserBox.Visibility = Visibility.Visible;
            ClientIdBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Visible;
            ConnectButton.Visibility = Visibility.Collapsed;
        }

        private Task DisplayAuthorizationErrorMessageAsync(ServiceException ex)
        {
            MessageDialog error = null;
            if (ex.Error.Code.Equals("ErrorAccessDenied"))
            {
                error = new MessageDialog($"{ex.Error.Code}\nCheck the scope");
            }
            else
            {
                error = new MessageDialog(ex.Error.Message);
            }

            return error.ShowAsync().AsTask();
        }

        private async void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            await MicrosoftGraphService.Instance.Logout();
            OneDriveItemsList.Visibility = Visibility.Collapsed;
            FilesBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Collapsed;
            UserBox.Visibility = Visibility.Collapsed;
            ClientIdBox.Visibility = Visibility.Visible;
            ConnectButton.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Collapsed;
            menuButton.Visibility = Visibility.Collapsed;
        }

        private void ClientIdExpandButton_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityStatusPanel(ClientIdBox, (Button)sender);
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

        private async void GetFoldersAndFilesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            string txtTop = TopText.Text;
            int top = 0;
            if (string.IsNullOrEmpty(txtTop))
            {
                top = 20;
            }
            else
            {
                top = Convert.ToInt32(txtTop);
            }

            Shell.Current.DisplayWaitRing = true;
            try
            {
                await _viewModel.GetItemsAsync(top);
            }
            catch (ServiceException ex)
            {
                await DisplayAuthorizationErrorMessageAsync(ex);
            }
            finally
            {
                Shell.Current.DisplayWaitRing = false;
                menuButton.Visibility = Visibility.Visible;
                backButton.Visibility = Visibility.Visible;
            }
        }

        private void FilesBoxExpandButton_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityStatusPanel(FilesPanel, (Button)sender);
            SetVisibilityStatusPanel(OneDriveItemsList, (Button)sender);
            menuButton.Visibility = FilesPanel.Visibility;
            backButton.Visibility = FilesPanel.Visibility;
        }

        private async void OneDriveItemsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            await _viewModel.NavigateToFolder(e.ClickedItem as OneDriveStorageItem);
        }
    }
}
