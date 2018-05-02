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
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.OneDrive;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public static class OneDriveSampleHelpers
    {
        public static async Task<string> InputTextDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox { AcceptsReturn = false, Height = 32 };

            ContentDialog dialog = new ContentDialog
            {
                Content = inputTextBox,
                Title = title,
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"
            };

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return inputTextBox.Text;
            }
            else
            {
                return null;
            }
        }

        public static async Task DisplayThumbnail(IRandomAccessStream streamTodDisplay, string title)
        {
            ContentDialog dialog = new ContentDialog();
            if (streamTodDisplay != null)
            {
                Windows.UI.Xaml.Controls.Image thumbnail = new Windows.UI.Xaml.Controls.Image();
                BitmapImage bmp = new BitmapImage();
                await bmp.SetSourceAsync(streamTodDisplay);
                thumbnail.Source = bmp;
                dialog.Content = thumbnail;
            }
            else
            {
                TextBlock text = new TextBlock { Text = "No thumbnail for this file" };
                dialog.Content = text;
            }

            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            await dialog.ShowAsync();
        }

        public static Task DisplayMessageAsync(string message)
        {
            MessageDialog error = new MessageDialog(message);

            return error.ShowAsync().AsTask();
        }

        public static Task DisplayOneDriveServiceExceptionAsync(ServiceException ex)
        {
            string message = null;
            if (ex.Error.Code.Equals("ErrorAccessDenied"))
            {
                message = $"{ex.Error.Code}\nCheck the scope";
            }
            else
            {
                message = ex.Error.Message;
            }

            return DisplayMessageAsync(message);
        }

        /// <summary>
        /// Create a new folder in the current folder
        /// </summary>
        /// <param name="folder">Destination folder where to create the new folder</param>
        /// <returns>Task to support await of async call.</returns>
        public static async Task NewFolderAsync(OneDriveStorageFolder folder)
        {
            if (folder != null)
            {
                SampleController.Current.DisplayWaitRing = true;
                try
                {
                    string newFolderName = await OneDriveSampleHelpers.InputTextDialogAsync("New Folder Name");
                    if (!string.IsNullOrEmpty(newFolderName))
                    {
                        await folder.StorageFolderPlatformService.CreateFolderAsync(newFolderName, CreationCollisionOption.GenerateUniqueName);
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

        /// <summary>
        /// Download a file
        /// </summary>
        /// <param name="item">File to download from OneDrive</param>
        /// <returns>Task to support await of async call.</returns>
        public static async Task DownloadAsync(OneDriveStorageItem item)
        {
            try
            {
                SampleController.Current.DisplayWaitRing = true;
                var oneDriveFile = (OneDriveStorageFile)item;
                using (var remoteStream = (await oneDriveFile.StorageFilePlatformService.OpenAsync()) as IRandomAccessStream)
                {
                    await SaveToLocalFolder(remoteStream, oneDriveFile.Name);
                }
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
        }

        /// <summary>
        /// Save the stream to a file in the local folder
        /// </summary>
        /// <param name="remoteStream">Stream to save</param>
        /// <param name="fileName">File's name</param>
        /// <returns>Task to support await of async call.</returns>
        public static async Task SaveToLocalFolder(IRandomAccessStream remoteStream, string fileName)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            byte[] buffer = new byte[remoteStream.Size];
            var localBuffer = await remoteStream.ReadAsync(buffer.AsBuffer(), (uint)remoteStream.Size, InputStreamOptions.ReadAhead);
            var myLocalFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
            using (var localStream = await myLocalFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await localStream.WriteAsync(localBuffer);
                await localStream.FlushAsync();
            }
        }

        /// <summary>
        /// Upload simple file.
        /// </summary>
        /// <param name="folder">The destination folder</param>
        /// <returns>Task to support await of async call.</returns>
        public static async Task UploadSimpleFileAsync(OneDriveStorageFolder folder)
        {
            SampleController.Current.DisplayWaitRing = true;

            try
            {
                if (folder != null)
                {
                    var selectedFile = await OpenLocalFileAsync();
                    if (selectedFile != null)
                    {
                        using (var localStream = await selectedFile.OpenReadAsync())
                        {
                            var fileCreated = await folder.StorageFolderPlatformService.CreateFileAsync(selectedFile.Name, CreationCollisionOption.GenerateUniqueName, localStream);
                        }
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                await OneDriveSampleHelpers.DisplayMessageAsync(ex.Message);
            }
            catch (ServiceException graphEx)
            {
                await OneDriveSampleHelpers.DisplayMessageAsync(graphEx.Error.Message);
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
        }

        /// <summary>
        /// Upload large file.
        /// </summary>
        /// <param name="folder">The destination folder</param>
        /// <returns>Task to support await of async call.</returns>
        public static async Task UploadLargeFileAsync(OneDriveStorageFolder folder)
        {
            try
            {
                if (folder != null)
                {
                    var selectedFile = await OpenLocalFileAsync();
                    if (selectedFile != null)
                    {
                        using (var localStream = await selectedFile.OpenReadAsync())
                        {
                            SampleController.Current.DisplayWaitRing = true;

                            // If the file exceed the Maximum size (ie 4MB)
                            var largeFileCreated = await folder.StorageFolderPlatformService.UploadFileAsync(selectedFile.Name, localStream, CreationCollisionOption.GenerateUniqueName, 320 * 1024);
                        }
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                await OneDriveSampleHelpers.DisplayMessageAsync(ex.Message);
            }
            catch (ServiceException graphEx)
            {
                await OneDriveSampleHelpers.DisplayMessageAsync(graphEx.Error.Message);
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
        }

        public static async Task RenameAsync(OneDriveStorageItem itemToRename)
        {
            try
            {
                SampleController.Current.DisplayWaitRing = true;
                string newName = await OneDriveSampleHelpers.InputTextDialogAsync("New Name");
                if (!string.IsNullOrEmpty(newName))
                {
                    await itemToRename.RenameAsync(newName);
                }
            }
            catch (ServiceException graphEx)
            {
                await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(graphEx);
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
        }

        public static async Task CopyToAsync(OneDriveStorageItem item, OneDriveStorageFolder rootFolder)
        {
            SampleController.Current.DisplayWaitRing = true;
            try
            {
                var folder = await OneDriveSampleHelpers.OpenFolderPicker("Copy to", rootFolder);
                if (folder != null)
                {
                    await item.CopyAsync(folder);
                }
            }
            catch (ServiceException exService)
            {
                await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(exService);
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
        }

        public static async Task MoveToAsync(OneDriveStorageItem item, OneDriveStorageFolder rootFolder)
        {
            SampleController.Current.DisplayWaitRing = true;
            try
            {
                var folder = await OneDriveSampleHelpers.OpenFolderPicker("Move to", rootFolder);
                if (folder != null)
                {
                    await item.MoveAsync(folder);
                }
            }
            catch (ServiceException exService)
            {
                await OneDriveSampleHelpers.DisplayOneDriveServiceExceptionAsync(exService);
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
        }

        public static async Task<OneDriveStorageFolder> OpenFolderPicker(string title, OneDriveStorageFolder rootFolder)
        {
            FoldersPickerControl folderPicker = new FoldersPickerControl(await rootFolder.GetFoldersAsync(100), rootFolder);

            ContentDialog dialog = new ContentDialog
            {
                Content = folderPicker,
                Title = title,
                PrimaryButtonText = "Ok"
            };

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return folderPicker.SelectedGraphFolder;
            }
            else
            {
                return null;
            }
        }

        public static async Task<StorageFolder> OpenFolderAsync()
        {
            FolderPicker folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                ViewMode = PickerViewMode.Thumbnail
            };

            return await folderPicker.PickSingleFolderAsync();
        }

        /// <summary>
        /// FileOpenPicker to open a file to upload
        /// </summary>
        /// <returns>a StorageFile representing the file to upload</returns>
        public static async Task<StorageFile> OpenLocalFileAsync()
        {
            FileOpenPicker picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                ViewMode = PickerViewMode.Thumbnail,
                CommitButtonText = "Upload"
            };

            picker.FileTypeFilter.Add("*");

            return await picker.PickSingleFileAsync();
        }
    }
}