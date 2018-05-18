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
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.OneDrive;
using Microsoft.Toolkit.Services.Services.MicrosoftGraph;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#pragma warning disable SA1118

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class OneDrivePage : Page
    {
        private OneDriveStorageFolder _graphRootFolder = null;
        private OneDriveStorageFolder _graphCurrentFolder = null;

        public OneDrivePage()
        {
            this.InitializeComponent();
            UserBox.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Collapsed;
            FilesBox.Visibility = Visibility.Collapsed;
            menuButton.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Collapsed;

            this.Loaded += OneDrivePage_Loaded;
        }

        private void OneDrivePage_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var p in typeof(MicrosoftGraphScope).GetFields())
            {
                if (string.CompareOrdinal(p.GetValue(null) as string, 0, "Files", 0, 5) == 0)
                {
                    DelegatedPermissions.Items.Add(p.GetValue(null));
                }
            }

            DelegatedPermissions.SelectedIndex = DelegatedPermissions.Items.IndexOf(MicrosoftGraphScope.FilesReadAll);

            DelegatedPermissions.ScrollIntoView(MicrosoftGraphScope.FilesReadAll);
        }

        private async Task SigninAsync(string appClientId)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            SampleController.Current.DisplayWaitRing = true;
            bool succeeded = false;

            try
            {
                // Converged app authentication
                // Get the selected Delegated Permissions
                var scopes = DelegatedPermissions.SelectedItems as string[];

                // If the user hasn't selected a scope then set it to FilesReadAll
                if (scopes == null)
                {
                    scopes = new string[] { MicrosoftGraphScope.FilesReadAll };
                }

                OneDriveService.Instance.Initialize(appClientId, scopes, null, null);

                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }

                _graphCurrentFolder = _graphRootFolder = await OneDriveService.Instance.RootFolderForMeAsync();
                OneDriveItemsList.ItemsSource = await _graphRootFolder.GetItemsAsync(20);
                OneDriveItemsList.Visibility = Visibility.Visible;

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
                SampleController.Current.DisplayWaitRing = false;
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
                await SigninAsync(ClientId.Text);
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

            SampleController.Current.DisplayWaitRing = true;
            try
            {
                OneDriveItemsList.ItemsSource = await _graphCurrentFolder.GetItemsAsync(top);
            }
            catch (ServiceException ex)
            {
                await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(ex);
            }
            finally
            {
                SampleController.Current.DisplayWaitRing = false;
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
                SampleController.Current.DisplayWaitRing = true;
                try
                {
                    var currentFolder = await _graphCurrentFolder.GetFolderAsync(item.Name);
                    OneDriveItemsList.ItemsSource = await currentFolder.GetItemsAsync(20);
                    _graphCurrentFolder = currentFolder;
                }
                catch (ServiceException ex)
                {
                    await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(ex);
                }
                finally
                {
                    SampleController.Current.DisplayWaitRing = false;
                }
            }
        }

        private async Task NavigateBackAsync()
        {
            if (_graphCurrentFolder != null)
            {
                OneDriveStorageFolder currentFolder = null;
                SampleController.Current.DisplayWaitRing = true;
                try
                {
                    if (!string.IsNullOrEmpty(_graphCurrentFolder.Path))
                    {
                        currentFolder = await _graphRootFolder.GetFolderAsync(_graphCurrentFolder.Path);
                    }
                    else
                    {
                        currentFolder = _graphRootFolder;
                    }

                    OneDriveItemsList.ItemsSource = currentFolder.GetItemsAsync(10);
                    _graphCurrentFolder = currentFolder;
                }
                catch (ServiceException ex)
                {
                    await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(ex);
                }
                finally
                {
                    SampleController.Current.DisplayWaitRing = false;
                }
            }
        }

        private async Task NavigateBackGraphAsync()
        {
            if (_graphCurrentFolder != null)
            {
                OneDriveStorageFolder currentFolder = null;
                SampleController.Current.DisplayWaitRing = true;
                try
                {
                    if (!string.IsNullOrEmpty(_graphCurrentFolder.Path))
                    {
                        currentFolder = await _graphRootFolder.GetFolderAsync(_graphCurrentFolder.Path);
                    }
                    else
                    {
                        currentFolder = _graphRootFolder;
                    }

                    OneDriveItemsList.ItemsSource = await currentFolder.GetItemsAsync(20);
                    _graphCurrentFolder = currentFolder;
                }
                catch (ServiceException ex)
                {
                    await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(ex);
                }
                finally
                {
                    SampleController.Current.DisplayWaitRing = false;
                }
            }
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            await NavigateBackGraphAsync();
        }

        private async void NewFolderButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveSampleHelpers.NewFolderAsync(_graphCurrentFolder);
            OneDriveItemsList.ItemsSource = await _graphCurrentFolder.GetItemsAsync(20);
        }

        private async void UploadSimpleFileButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveSampleHelpers.UploadSimpleFileAsync(_graphCurrentFolder);
            OneDriveItemsList.ItemsSource = await _graphCurrentFolder.GetItemsAsync(20);
        }

        private async void UploadLargeFileButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveSampleHelpers.UploadLargeFileAsync(_graphCurrentFolder);
            OneDriveItemsList.ItemsSource = await _graphCurrentFolder.GetItemsAsync(20);
        }

        private async void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveSampleHelpers.RenameAsync((OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext);
            OneDriveItemsList.ItemsSource = await _graphCurrentFolder.GetItemsAsync(20);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await DeleteAsync((OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext);
        }

        private async Task DeleteAsync(Toolkit.Services.OneDrive.OneDriveStorageItem itemToDelete)
        {
            MessageDialog messageDialog = new MessageDialog($"Are you sure you want to delete '{itemToDelete.Name}'", "Delete");
            messageDialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(async (cmd) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { SampleController.Current.DisplayWaitRing = true; });
                try
                {
                    await itemToDelete.DeleteAsync();
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (DispatchedHandler)(async () => { OneDriveItemsList.ItemsSource = await this._graphCurrentFolder.GetItemsAsync(20); }));
                }
                catch (ServiceException ex)
                {
                    await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(ex);
                }
                finally
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { SampleController.Current.DisplayWaitRing = false; });
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

        private async void CopyToButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveSampleHelpers.CopyToAsync((OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext, _graphRootFolder);
        }

        private async void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            await OneDriveSampleHelpers.MoveToAsync((OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext, _graphRootFolder);
        }

        private async void ThumbnailButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SampleController.Current.DisplayWaitRing = true;

                var file = (OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext;
                using (var stream = (await file.StorageItemPlatformService.GetThumbnailAsync(Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ThumbnailSize.Large)) as IRandomAccessStream)
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
                SampleController.Current.DisplayWaitRing = false;
            }
        }
    }
}