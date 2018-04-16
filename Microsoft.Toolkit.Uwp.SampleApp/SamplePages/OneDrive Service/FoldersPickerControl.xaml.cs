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
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.OneDrive;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class FoldersPickerControl : UserControl
    {
        private OneDriveStorageFolder _graphRootFolder = null;
        private List<OneDriveStorageFolder> _graphFolders = null;

        private OneDriveStorageFolder _graphDestinationFolder = null;
        private OneDriveStorageFolder _graphCurrentFolder = null;

        public OneDriveStorageFolder SelectedGraphFolder
        {
            get
            {
                return _graphDestinationFolder;
            }
        }

        public FoldersPickerControl(List<OneDriveStorageFolder> folders, OneDriveStorageFolder rootFolder)
        {
            this.InitializeComponent();
            _graphFolders = folders;
            _graphCurrentFolder = _graphRootFolder = rootFolder;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LstFolder.ItemsSource = _graphFolders;
        }

        private void LstFolder_ItemClick(object sender, ItemClickEventArgs e)
        {
            _graphDestinationFolder = e.ClickedItem as OneDriveStorageFolder;
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            await NavigateBackGraphAsync();
        }

        private async Task NavigateBackGraphAsync()
        {
            if (_graphCurrentFolder != null)
            {
                OneDriveStorageFolder currentFolder = null;
                progressRing.IsActive = true;
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

                    LstFolder.ItemsSource = await currentFolder.GetFoldersAsync(100);
                    _graphCurrentFolder = currentFolder;
                }
                catch (ServiceException ex)
                {
                    await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(ex);
                }
                finally
                {
                    progressRing.IsActive = false;
                }
            }
        }

        private async Task NavigateToFolderAsync(Toolkit.Services.OneDrive.OneDriveStorageItem item)
        {
            progressRing.IsActive = true;
            try
            {
                var currentFolder = await _graphCurrentFolder.GetFolderAsync(item.Name);
                var items = await currentFolder.GetFoldersAsync(100);
                if (items.Count > 0)
                {
                    LstFolder.ItemsSource = items;
                    _graphCurrentFolder = currentFolder;
                }
            }
            catch (ServiceException ex)
            {
                await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(ex);
            }
            finally
            {
                progressRing.IsActive = false;
            }
        }

        private async void NavigateToButton_Click(object sender, RoutedEventArgs e)
        {
            await NavigateToFolderAsync((Toolkit.Services.OneDrive.OneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext);
        }
    }
}
