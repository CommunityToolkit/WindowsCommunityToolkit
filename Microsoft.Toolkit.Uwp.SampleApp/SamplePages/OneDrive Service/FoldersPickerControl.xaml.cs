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

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Uwp.Services.OneDrive;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class FoldersPickerControl : UserControl
    {
        private IOneDriveStorageFolder _rootFolder = null;
        private List<IOneDriveStorageFolder> _folders = null;

        private IOneDriveStorageFolder _destinationFolder = null;
        private IOneDriveStorageFolder _currentFolder = null;

        public IOneDriveStorageFolder SelectedFolder
        {
            get
            {
                return _destinationFolder;
            }
        }

        public FoldersPickerControl(List<IOneDriveStorageFolder> folders, IOneDriveStorageFolder rootFolder)
        {
            this.InitializeComponent();
            _folders = folders;
            _currentFolder = _rootFolder = rootFolder;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LstFolder.ItemsSource = _folders;
        }

        private void LstFolder_ItemClick(object sender, ItemClickEventArgs e)
        {
            _destinationFolder = e.ClickedItem as IOneDriveStorageFolder;
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            await NavigateBackAsync();
        }

        private async Task NavigateBackAsync()
        {
            if (_currentFolder != null)
            {
                IOneDriveStorageFolder currentFolder = null;
                progressRing.IsActive = true;
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

                    LstFolder.ItemsSource = await currentFolder.GetFoldersAsync();
                    _currentFolder = currentFolder;
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

        private async Task NavigateToFolderAsync(IOneDriveStorageItem item)
        {
                progressRing.IsActive = true;
                try
                {
                    var currentFolder = await _currentFolder.GetFolderAsync(item.Name);
                    var items = await currentFolder.GetFoldersAsync(100);
                    if (items.Count > 0)
                    {
                        LstFolder.ItemsSource = items;
                        _currentFolder = currentFolder;
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
            await NavigateToFolderAsync((IOneDriveStorageItem)((AppBarButton)e.OriginalSource).DataContext);
        }
    }
}
