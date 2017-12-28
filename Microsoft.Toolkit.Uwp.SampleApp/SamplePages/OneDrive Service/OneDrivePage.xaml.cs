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
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Uwp.Services.OneDrive;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;

#pragma warning disable SA1118
namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class OneDrivePage : Page
    {
        private Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveStorageFolder _rootFolder = null;
        private Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveStorageFolder _currentFolder = null;

        public OneDrivePage()
        {
            this.InitializeComponent();
            UserBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Collapsed;
            FilesBox.Visibility = Visibility.Collapsed;
            menuButton.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Collapsed;
        }

        private async Task SigninAsync(int indexProvider, string appClientId)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            bool succeeded = false;

            try
            {
                // OnlineId
                if (indexProvider == 0)
                {
                    OneDriveService.Instance.Initialize();
                }
                else if (indexProvider == 1)
                {
                    OneDriveService.Instance.Initialize(appClientId, AccountProviderType.Msa, OneDriveScopes.OfflineAccess | OneDriveScopes.ReadWrite);
                }
                else if (indexProvider == 2)
                {
                    OneDriveService.Instance.Initialize(appClientId, AccountProviderType.Adal);
                }

                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }

                _currentFolder = _rootFolder = await OneDriveService.Instance.RootFolderAsync();
                OneDriveItemsList.ItemsSource = _rootFolder.GetItemsAsync();
                succeeded = true;
            }
            catch (ServiceException serviceEx)
            {
                await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(serviceEx);
            }
            catch (Exception ex)
            {
                await OneDriveSampleHelpers.DisplayMessageAsync(ex.Message);
                TrackingManager.TrackException(ex);
            }
            finally
            {
                Shell.Current.DisplayWaitRing = false;
            }

            if (succeeded)
            {
                FilesBox.Visibility = Visibility.Visible;
                UserBox.Visibility = Visibility.Visible;
                ClientIdBox.Visibility = Visibility.Collapsed;
                ClientIdHelper.Visibility = Visibility.Collapsed;
                LogOutButton.Visibility = Visibility.Visible;
                ConnectButton.Visibility = Visibility.Collapsed;
                menuButton.Visibility = Visibility.Visible;
                BackButton.Visibility = Visibility.Visible;
            }
            else
            {
                FilesBox.Visibility = Visibility.Collapsed;
                UserBox.Visibility = Visibility.Collapsed;
                ClientIdBox.Visibility = Visibility.Visible;
                ClientIdHelper.Visibility = Visibility.Visible;
                LogOutButton.Visibility = Visibility.Collapsed;
                ConnectButton.Visibility = Visibility.Visible;
                menuButton.Visibility = Visibility.Collapsed;
                BackButton.Visibility = Visibility.Collapsed;
            }
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SigninAsync(_indexProvider, ClientId.Text);
            }
            catch (Exception ex)
            {
                await OneDriveSampleHelpers.DisplayMessageAsync(ex.Message);
            }
        }

        private async void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveService.Instance.LogoutAsync();
            OneDriveItemsList.Visibility = Visibility.Collapsed;
            FilesBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Collapsed;
            UserBox.Visibility = Visibility.Collapsed;
            ClientIdBox.Visibility = Visibility.Visible;
            ClientIdHelper.Visibility = Visibility.Visible;
            ConnectButton.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Collapsed;
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
                OneDriveItemsList.ItemsSource = await _currentFolder.GetItemsAsync(top);
            }
            catch (ServiceException ex)
            {
                await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(ex);
            }
            finally
            {
                Shell.Current.DisplayWaitRing = false;
                menuButton.Visibility = Visibility.Visible;
                BackButton.Visibility = Visibility.Visible;
            }
        }

        private void FilesBoxExpandButton_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityStatusPanel(FilesPanel, (Button)sender);
            SetVisibilityStatusPanel(OneDriveItemsList, (Button)sender);
            menuButton.Visibility = FilesPanel.Visibility;
            BackButton.Visibility = FilesPanel.Visibility;
        }

        private async void OneDriveItemsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            await NavigateToFolderAsync(e.ClickedItem as OneDriveStorageItem);
        }

        private async Task NavigateToFolderAsync(OneDriveStorageItem item)
        {
            if (item.IsFolder())
            {
                Shell.Current.DisplayWaitRing = true;
                try
                {
                    var currentFolder = await _currentFolder.GetFolderAsync(item.Name);
                    OneDriveItemsList.ItemsSource = currentFolder.GetItemsAsync();
                    _currentFolder = currentFolder;
                }
                catch (ServiceException ex)
                {
                    await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(ex);
                }
                finally
                {
                    Shell.Current.DisplayWaitRing = false;
                }
            }
        }

        private async Task NavigateBackAsync()
        {
            if (_currentFolder != null)
            {
                OneDriveStorageFolder currentFolder = null;
                Shell.Current.DisplayWaitRing = true;
                try
                {
                    if (!string.IsNullOrEmpty(_currentFolder.Path))
                    {
                        currentFolder = await _rootFolder.GetFolderAsync(_currentFolder.Path);
                    }
                    else
                    {
                        currentFolder = _rootFolder;
                    }

                    OneDriveItemsList.ItemsSource = currentFolder.GetItemsAsync();
                    _currentFolder = currentFolder;
                }
                catch (ServiceException ex)
                {
                    await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(ex);
                }
                finally
                {
                    Shell.Current.DisplayWaitRing = false;
                }
            }
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            await NavigateBackAsync();
        }

        private async void NewFolderButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveSampleHelpers.NewFolderAsync(_currentFolder);
            OneDriveItemsList.ItemsSource = _currentFolder.GetItemsAsync();
        }

        private async void UploadSimpleFileButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveSampleHelpers.UploadSimpleFileAsync(_currentFolder);
            OneDriveItemsList.ItemsSource = _currentFolder.GetItemsAsync();
        }

        private async void UploadLargeFileButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveSampleHelpers.UploadLargeFileAsync(_currentFolder);
            OneDriveItemsList.ItemsSource = _currentFolder.GetItemsAsync();
        }

        private async void RenameButton_Click(object sender, RoutedEventArgs e)
        {
           await OneDriveSampleHelpers.RenameAsync((OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext);
           OneDriveItemsList.ItemsSource = _currentFolder.GetItemsAsync();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await DeleteAsync((OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext);
        }

        private async Task DeleteAsync(OneDriveStorageItem itemToDelete)
        {
            MessageDialog messageDialog = new MessageDialog($"Are you sure you want to delete '{itemToDelete.Name}'", "Delete");
            messageDialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(async (cmd) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Shell.Current.DisplayWaitRing = true; });
                try
                {
                    await itemToDelete.DeleteAsync();
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { OneDriveItemsList.ItemsSource = _currentFolder.GetItemsAsync();  });
                }
                catch (ServiceException ex)
                {
                    await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(ex);
                }
                finally
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Shell.Current.DisplayWaitRing = false; });
                }
            })));

            messageDialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler((cmd) => { return; })));

            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            var command = await messageDialog.ShowAsync();
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveSampleHelpers.DownloadAsync((OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext);
        }

        private int _indexProvider = 0;

        private async void CboProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _indexProvider = CboProvider.SelectedIndex;
            var visibility = Visibility.Visible;

            if (_indexProvider == 0)
            {
                await SigninAsync(_indexProvider, null);
                visibility = Visibility.Collapsed;
            }

            ClientIdHelper.Visibility = ConnectButton.Visibility = ClientId.Visibility = visibility;
        }

        private async void CopyToButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveSampleHelpers.CopyToAsync((OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext, _rootFolder);
        }

        private async void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveSampleHelpers.MoveToAsync((OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext, _rootFolder);
        }

        private async void ThumbnailButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Shell.Current.DisplayWaitRing = true;
                var file = (OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext;
                using (var stream = await file.GetThumbnailAsync(ThumbnailSize.Large))
                {
                    await OneDriveSampleHelpers.DisplayThumbnail(stream, "thumbnail");
                }
            }
            catch (ServiceException ex)
            {
                await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(ex);
            }
            finally
            {
                Shell.Current.DisplayWaitRing = false;
            }
        }
    }
}
