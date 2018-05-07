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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The SharePointFiles Control displays a simple list of SharePoint Files.
    /// </summary>
    public partial class SharePointFileList : Control
    {
        private static async void GraphAccessTokenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SharePointFileList control = d as SharePointFileList;
            control._graphClient = Common.GetAuthenticatedClient(control.GraphAccessToken);
            if (!string.IsNullOrEmpty(control.DriveUrl))
            {
                await control.InitDriveAsync(control.DriveUrl);
            }
        }

        private static async void DriveUrlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SharePointFileList control = d as SharePointFileList;
            if (control._graphClient != null && !string.IsNullOrWhiteSpace(control.DriveUrl))
            {
                if (Uri.IsWellFormedUriString(control.DriveUrl, UriKind.Absolute))
                {
                    await control.InitDriveAsync(control.DriveUrl);
                }
            }
        }

        private static void DetailPanePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SharePointFileList control = d as SharePointFileList;
            if (control.IsDetailPaneVisible)
            {
                control.ShowDetailsPane();
            }
        }

        private async void BackCommandAction()
        {
            if (DetailPane == DetailPaneDisplayMode.Full && _thumbnail.Visibility == Visibility.Visible)
            {
                HideDetailsPane();
            }
            else if (_driveItemPath.Count > 1)
            {
                _driveItemPath.Pop();
                string parentItemId = _driveItemPath.Peek();
                if (_driveItemPath.Count == 1)
                {
                    _back.Visibility = Visibility.Collapsed;
                }

                await LoadFilesAsync(parentItemId);
            }
        }

        private async void UploadCommandAction()
        {
            ErrorMessage = string.Empty;
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add("*");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string driveItemId = _driveItemPath.Peek();
                using (Stream inputStream = await file.OpenStreamForReadAsync())
                {
                    if (inputStream.Length < 1024 * 1024 * 4)
                    {
                        FileUploading++;

                        try
                        {
                            await _graphClient.Drives[_driveId].Items[driveItemId].ItemWithPath(file.Name).Content.Request().PutAsync<DriveItem>(inputStream, _cancelUpload.Token);
                            FileUploading--;
                        }
                        catch (Exception ex)
                        {
                            FileUploading--;
                            ErrorMessage = ex.Message;
                        }

                        await LoadFilesAsync(driveItemId);
                    }
                }
            }
        }

        private async void ShareCommandAction()
        {
            if (_list.SelectedItem is DriveItem driveItem)
            {
                Permission link = await _graphClient.Drives[_driveId].Items[driveItem.Id].CreateLink("view", "organization").Request().PostAsync();
                MessageDialog dialog = new MessageDialog(link.Link.WebUrl, ShareLinkCopiedMessage);
                DataPackage package = new DataPackage();
                package.SetText(link.Link.WebUrl);
                Clipboard.SetContent(package);
                await dialog.ShowAsync();
            }
        }

        private async void DownloadCommandAction()
        {
            if (_list.SelectedItem is DriveItem driveItem)
            {
                FileSavePicker picker = new FileSavePicker();
                picker.FileTypeChoices.Add(AllFilesMessage, new List<string>() { driveItem.Name.Substring(driveItem.Name.LastIndexOf(".")) });
                picker.SuggestedFileName = driveItem.Name;
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                StorageFile file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    using (Stream inputStream = await _graphClient.Drives[_driveId].Items[driveItem.Id].Content.Request().GetAsync())
                    {
                        using (Stream outputStream = await file.OpenStreamForWriteAsync())
                        {
                            await inputStream.CopyToAsync(outputStream);
                        }
                    }
                }
            }
        }

        private async void DeleteCommandAction()
        {
            if (_list.SelectedItem is DriveItem driveItem)
            {
                MessageDialog confirmDialog = new MessageDialog(DeleteConfirmMessage);
                confirmDialog.Commands.Add(new UICommand(DeleteConfirmOkMessage, cmd => { }, commandId: 0));
                confirmDialog.Commands.Add(new UICommand(DeleteConfirmCancelMessage, cmd => { }, commandId: 1));

                confirmDialog.DefaultCommandIndex = 0;
                confirmDialog.CancelCommandIndex = 1;

                IUICommand result = await confirmDialog.ShowAsync();

                if ((int)result.Id == 0)
                {
                    await _graphClient.Drives[_driveId].Items[driveItem.Id].Request().DeleteAsync();
                    string driveItemId = _driveItemPath.Peek();
                    await LoadFilesAsync(driveItemId);
                }
            }
        }

        private async void ShowErrorDetailsCommandAction()
        {
            MessageDialog messageDialog = new MessageDialog(ErrorMessage);
            await messageDialog.ShowAsync();
        }

        private async void LoadMoreCommandAction()
        {
            await LoadNextPageAsync();
        }

        private void CancelCommandAction()
        {
            _cancelUpload.Cancel(false);
            _cancelUpload.Dispose();
            _cancelUpload = new CancellationTokenSource();
        }

        private async void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_list.SelectedItem is DriveItem driveItem)
            {
                _cancelGetDetails.Cancel(false);
                _cancelGetDetails.Dispose();
                _cancelGetDetails = new CancellationTokenSource();
                if (driveItem.File != null)
                {
                    try
                    {
                        SelectedFile = driveItem;
                        FileSize = driveItem.Size ?? 0;
                        LastModified = driveItem.LastModifiedDateTime?.LocalDateTime.ToString() ?? string.Empty;
                        if (FileSelected != null)
                        {
                            FileSelected.Invoke(this, new FileSelectedEventArgs(driveItem));
                        }

                        ThumbnailImageSource = null;
                        _download.Visibility = Visibility.Visible;
                        _share.Visibility = Visibility.Collapsed;
                        _delete.Visibility = Visibility.Collapsed;
                        Task<IDriveItemPermissionsCollectionPage> taskPermissions = _graphClient.Drives[_driveId].Items[driveItem.Id].Permissions.Request().GetAsync(_cancelGetDetails.Token);
                        IDriveItemPermissionsCollectionPage permissions = await taskPermissions;
                        if (!taskPermissions.IsCanceled)
                        {
                            foreach (Permission permission in permissions)
                            {
                                if (permission.Roles.Contains("write") || permission.Roles.Contains("owner"))
                                {
                                    _share.Visibility = Visibility.Visible;
                                    _delete.Visibility = Visibility.Visible;
                                    break;
                                }
                            }

                            IsDetailPaneVisible = true;
                            Task<IDriveItemThumbnailsCollectionPage> taskThumbnails = _graphClient.Drives[_driveId].Items[driveItem.Id].Thumbnails.Request().GetAsync(_cancelGetDetails.Token);
                            IDriveItemThumbnailsCollectionPage thumbnails = await taskThumbnails;
                            if (!taskThumbnails.IsCanceled)
                            {
                                ThumbnailSet thumbnailsSet = thumbnails.FirstOrDefault();
                                if (thumbnailsSet != null)
                                {
                                    ThumbnailImageSource = new BitmapImage(new Uri(thumbnailsSet.Large.Url));
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    SelectedFile = null;
                    FileSize = 0;
                    LastModified = string.Empty;
                    _download.Visibility = Visibility.Collapsed;
                    _share.Visibility = Visibility.Collapsed;
                    _delete.Visibility = Visibility.Collapsed;
                    IsDetailPaneVisible = false;
                }
            }
        }

        private async void List_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is DriveItem driveItem && driveItem.Folder != null)
            {
                _driveItemPath.Push(driveItem.Id);
                _back.Visibility = Visibility.Visible;
                await LoadFilesAsync(driveItem.Id);
            }
        }
    }
}
