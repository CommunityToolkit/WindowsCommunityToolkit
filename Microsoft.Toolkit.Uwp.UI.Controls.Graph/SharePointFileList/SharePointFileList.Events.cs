// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The SharePointFiles Control displays a simple list of SharePoint Files.
    /// </summary>
    public partial class SharePointFileList
    {
        private static async void DriveUrlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (MicrosoftGraphService.Instance.IsAuthenticated)
            {
                SharePointFileList control = d as SharePointFileList;
                await MicrosoftGraphService.Instance.TryLoginAsync();
                GraphServiceClient graphServiceClient = MicrosoftGraphService.Instance.GraphProvider;
                if (graphServiceClient != null && !string.IsNullOrWhiteSpace(control.DriveUrl))
                {
                    if (Uri.IsWellFormedUriString(control.DriveUrl, UriKind.Absolute))
                    {
                        await control.InitDriveAsync(control.DriveUrl);
                    }
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

        private async void Back_Click(object sender, RoutedEventArgs e)
        {
            if (DetailPane == DetailPaneDisplayMode.Full && IsDetailPaneVisible)
            {
                IsDetailPaneVisible = false;
                HideDetailsPane();
            }
            else if (_driveItemPath.Count > 1)
            {
                _driveItemPath.Pop();
                string parentItemId = _driveItemPath.Peek().Id;
                if (_driveItemPath.Count == 1)
                {
                    BackButtonVisibility = Visibility.Collapsed;
                }

                UpdateCurrentPath();
                await LoadFilesAsync(parentItemId);
            }
        }

        private async void Upload_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage = string.Empty;
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add("*");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string driveItemId = _driveItemPath.Peek()?.Id;
                using (Stream inputStream = await file.OpenStreamForReadAsync())
                {
                    if (inputStream.Length < 1024 * 1024 * 4)
                    {
                        FileUploading++;
                        StatusMessage = string.Format(UploadingFilesMessageTemplate, FileUploading);
                        VisualStateManager.GoToState(this, UploadStatusUploading, false);
                        try
                        {
                            await GraphService.TryLoginAsync();
                            GraphServiceClient graphServiceClient = GraphService.GraphProvider;
                            await graphServiceClient.Drives[_driveId].Items[driveItemId].ItemWithPath(file.Name).Content.Request().PutAsync<DriveItem>(inputStream, _cancelUpload.Token);
                            VisualStateManager.GoToState(this, UploadStatusNotUploading, false);
                            FileUploading--;
                        }
                        catch (Exception ex)
                        {
                            FileUploading--;
                            ErrorMessage = ex.Message;
                            VisualStateManager.GoToState(this, UploadStatusError, false);
                        }

                        await LoadFilesAsync(driveItemId);
                    }
                }
            }
        }

        private async void Share_Click(object sender, RoutedEventArgs e)
        {
            if (_list.SelectedItem is DriveItem driveItem)
            {
                try
                {
                    await GraphService.TryLoginAsync();
                    GraphServiceClient graphServiceClient = GraphService.GraphProvider;
                    Permission link = await graphServiceClient.Drives[_driveId].Items[driveItem.Id].CreateLink("view", "organization").Request().PostAsync();
                    MessageDialog dialog = new MessageDialog(link.Link.WebUrl, ShareLinkCopiedMessage);
                    DataPackage package = new DataPackage();
                    package.SetText(link.Link.WebUrl);
                    Clipboard.SetContent(package);
                    await dialog.ShowAsync();
                }
                catch (Exception exception)
                {
                    MessageDialog dialog = new MessageDialog(exception.Message);
                    await dialog.ShowAsync();
                }
            }
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            if (_list.SelectedItem is DriveItem driveItem)
            {
                await GraphService.TryLoginAsync();
                GraphServiceClient graphServiceClient = GraphService.GraphProvider;
                FileSavePicker picker = new FileSavePicker();
                picker.FileTypeChoices.Add(AllFilesMessage, new List<string>() { driveItem.Name.Substring(driveItem.Name.LastIndexOf(".")) });
                picker.SuggestedFileName = driveItem.Name;
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                StorageFile file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    using (Stream inputStream = await graphServiceClient.Drives[_driveId].Items[driveItem.Id].Content.Request().GetAsync())
                    {
                        using (Stream outputStream = await file.OpenStreamForWriteAsync())
                        {
                            await inputStream.CopyToAsync(outputStream);
                        }
                    }
                }
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
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
                    await GraphService.TryLoginAsync();
                    GraphServiceClient graphServiceClient = GraphService.GraphProvider;
                    await graphServiceClient.Drives[_driveId].Items[driveItem.Id].Request().DeleteAsync();
                    string driveItemId = _driveItemPath.Peek()?.Id;
                    await LoadFilesAsync(driveItemId);
                }
            }
        }

        private async void ShowErrorDetails_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog(ErrorMessage);
            await messageDialog.ShowAsync();
        }

        private async void LoadMore_Click(object sender, RoutedEventArgs e)
        {
            await LoadNextPageAsync();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _cancelUpload.Cancel(false);
            _cancelUpload.Dispose();
            _cancelUpload = new CancellationTokenSource();
        }

        private async void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _cancelGetDetails.Cancel(false);
            _cancelGetDetails.Dispose();
            _cancelGetDetails = new CancellationTokenSource();
            if (_list.SelectedItem is DriveItem driveItem && driveItem.File != null)
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
                    VisualStateManager.GoToState(this, NavStatesFileReadonly, false);
                    await GraphService.TryLoginAsync();
                    GraphServiceClient graphServiceClient = GraphService.GraphProvider;
                    Task<IDriveItemPermissionsCollectionPage> taskPermissions = graphServiceClient.Drives[_driveId].Items[driveItem.Id].Permissions.Request().GetAsync(_cancelGetDetails.Token);
                    IDriveItemPermissionsCollectionPage permissions = await taskPermissions;
                    if (!taskPermissions.IsCanceled)
                    {
                        foreach (Permission permission in permissions)
                        {
                            if (permission.Roles.Contains("write") || permission.Roles.Contains("owner"))
                            {
                                VisualStateManager.GoToState(this, NavStatesFileEdit, false);
                                break;
                            }
                        }

                        Task<IDriveItemThumbnailsCollectionPage> taskThumbnails = graphServiceClient.Drives[_driveId].Items[driveItem.Id].Thumbnails.Request().GetAsync(_cancelGetDetails.Token);
                        IDriveItemThumbnailsCollectionPage thumbnails = await taskThumbnails;
                        if (!taskThumbnails.IsCanceled)
                        {
                            ThumbnailSet thumbnailsSet = thumbnails.FirstOrDefault();
                            if (thumbnailsSet != null)
                            {
                                Thumbnail thumbnail = thumbnailsSet.Large;
                                if (thumbnail.Url.Contains("inputFormat=svg"))
                                {
                                    SvgImageSource source = new SvgImageSource();
                                    using (Stream inputStream = await graphServiceClient.Drives[_driveId].Items[driveItem.Id].Content.Request().GetAsync())
                                    {
                                        SvgImageSourceLoadStatus status = await source.SetSourceAsync(inputStream.AsRandomAccessStream());
                                        if (status == SvgImageSourceLoadStatus.Success)
                                        {
                                            ThumbnailImageSource = source;
                                        }
                                    }
                                }
                                else
                                {
                                    ThumbnailImageSource = new BitmapImage(new Uri(thumbnail.Url));
                                }
                            }
                        }

                        IsDetailPaneVisible = true;
                        ShowDetailsPane();
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
                VisualStateManager.GoToState(this, _pathVisualState, false);
                IsDetailPaneVisible = false;
                HideDetailsPane();
            }
        }

        private async void List_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is DriveItem driveItem && driveItem.Folder != null)
            {
                _driveItemPath.Push(driveItem);
                UpdateCurrentPath();
                BackButtonVisibility = Visibility.Visible;
                await LoadFilesAsync(driveItem.Id);
            }
        }
    }
}
