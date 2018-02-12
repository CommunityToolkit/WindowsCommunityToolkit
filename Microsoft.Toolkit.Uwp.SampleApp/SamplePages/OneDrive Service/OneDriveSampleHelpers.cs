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
#pragma warning disable CS0618 // Type or member is obsolete

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
        public static async Task NewFolderAsync(Toolkit.Services.OneDrive.OneDriveStorageFolder folder)
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
        /// Create a new folder in the current folder
        /// </summary>
        /// <param name="folder">Destination folder where to create the new folder</param>
        /// <returns>Task to support await of async call.</returns>
        public static async Task NewFolderAsync(Services.OneDrive.OneDriveStorageFolder folder)
        {
            if (folder != null)
            {
                SampleController.Current.DisplayWaitRing = true;
                try
                {
                    string newFolderName = await OneDriveSampleHelpers.InputTextDialogAsync("New Folder Name");
                    if (!string.IsNullOrEmpty(newFolderName))
                    {
                        await folder.CreateFolderAsync(newFolderName);
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
        public static async Task DownloadAsync(Toolkit.Services.OneDrive.OneDriveStorageItem item)
        {
            try
            {
                SampleController.Current.DisplayWaitRing = true;
                var oneDriveFile = (Toolkit.Services.OneDrive.OneDriveStorageFile)item;
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
        /// Download a file
        /// </summary>
        /// <param name="item">File to download from OneDrive</param>
        /// <returns>Task to support await of async call.</returns>
        public static async Task DownloadAsync(Services.OneDrive.OneDriveStorageItem item)
        {
            try
            {
                SampleController.Current.DisplayWaitRing = true;
                var oneDriveFile = (Services.OneDrive.OneDriveStorageFile)item;
                using (var remoteStream = await oneDriveFile.OpenAsync())
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
        public static async Task UploadSimpleFileAsync(Toolkit.Services.OneDrive.OneDriveStorageFolder folder)
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
        /// Upload simple file.
        /// </summary>
        /// <param name="folder">The destination folder</param>
        /// <returns>Task to support await of async call.</returns>
        public static async Task UploadSimpleFileAsync(Services.OneDrive.OneDriveStorageFolder folder)
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
                            var fileCreated = await folder.CreateFileAsync(selectedFile.Name, CreationCollisionOption.GenerateUniqueName, localStream);
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
        public static async Task UploadLargeFileAsync(Toolkit.Services.OneDrive.OneDriveStorageFolder folder)
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

        /// <summary>
        /// Upload large file.
        /// </summary>
        /// <param name="folder">The destination folder</param>
        /// <returns>Task to support await of async call.</returns>
        public static async Task UploadLargeFileAsync(Services.OneDrive.OneDriveStorageFolder folder)
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
                            var largeFileCreated = await folder.UploadFileAsync(selectedFile.Name, localStream, CreationCollisionOption.GenerateUniqueName, 320 * 1024);
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

        public static async Task RenameAsync(Toolkit.Services.OneDrive.OneDriveStorageItem itemToRename)
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

        public static async Task RenameAsync(Services.OneDrive.OneDriveStorageItem itemToRename)
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

        public static async Task CopyToAsync(Toolkit.Services.OneDrive.OneDriveStorageItem item, Toolkit.Services.OneDrive.OneDriveStorageFolder rootFolder)
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

        public static async Task CopyToAsync(Services.OneDrive.OneDriveStorageItem item, Services.OneDrive.OneDriveStorageFolder rootFolder)
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

        public static async Task MoveToAsync(Toolkit.Services.OneDrive.OneDriveStorageItem item, Toolkit.Services.OneDrive.OneDriveStorageFolder rootFolder)
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

        public static async Task MoveToAsync(Services.OneDrive.OneDriveStorageItem item, Services.OneDrive.OneDriveStorageFolder rootFolder)
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

        public static async Task<Toolkit.Services.OneDrive.OneDriveStorageFolder> OpenFolderPicker(string title, Toolkit.Services.OneDrive.OneDriveStorageFolder rootFolder)
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

        public static async Task<Services.OneDrive.OneDriveStorageFolder> OpenFolderPicker(string title, Services.OneDrive.OneDriveStorageFolder rootFolder)
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
                return folderPicker.SelectedFolder;
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

#pragma warning restore CS0618 // Type or member is obsolete
    }
}