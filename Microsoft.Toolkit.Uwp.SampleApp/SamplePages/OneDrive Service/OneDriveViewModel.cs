// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
// ******************************************************************

using Microsoft.Graph;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Microsoft.Toolkit.Uwp.Services;
using Microsoft.Toolkit.Uwp.Services.MicrosoftGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using static Microsoft.Toolkit.Uwp.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Type OneDriveViewModel
    /// </summary>
    public class OneDriveViewModel : BindableBase
    {
        /// <summary>
        /// Use to cancel when uploading a big file
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        public CoreDispatcher Dispatcher { get; set; }

        private double _totalSizeFile;

        /// <summary>
        /// Gets or sets the size of a big file
        /// </summary>
        public double TotalSizeFile { get { return _totalSizeFile; } set { Set(ref _totalSizeFile, value); } }

        private bool _ringActive;

        public bool RingActive { get { return _ringActive; } set { Set(ref _ringActive, value); } }

        private long _uploadProgression = 0;

        /// <summary>
        /// Gets or sets the progression when uploading a big file
        /// </summary>
        public long UploadProgression { get { return _uploadProgression; } set { Set(ref _uploadProgression, value); } }

        private OneDriveStorageFolder _currentFolder = null;

        // Keep an instance to the rootFolder
        private OneDriveStorageFolder _rootFolder = null;

        public User User { get; set; }

        private OneDriveStorageItemsCollection _items;

        public OneDriveStorageItemsCollection Items
        {
            get { return _items; } set { Set<OneDriveStorageItemsCollection>(ref _items, value); }
        }

        public async Task SigninAsync(string appClientId)
        {
            Shell.Current.DisplayWaitRing = true;
            try
            {
                // Initialize the service
                MicrosoftGraphService.Instance.Initialize(appClientId, AuthenticationModel.V2, ServicesToInitialize.OneDrive);

                if (!await MicrosoftGraphService.Instance.LoginAsync())
                {
                    var error = new MessageDialog("Unable to sign in");
                    await error.ShowAsync();
                    return;
                }

                User = await MicrosoftGraphService.Instance.User.GetProfileAsync();
                _rootFolder = await MicrosoftGraphService.Instance.User.OneDrive.RootFolderAsync();
                _currentFolder = _rootFolder;
            }
            catch (ServiceException ex)
            {
                await DisplayMessageAsync(ex.Error.Message);
            }
            finally
            {
                Shell.Current.DisplayWaitRing = false;
            }
        }

        public async Task GetItemsAsync(int top)
        {
            Items = await _currentFolder.GetItemsAsync(top);
        }

        public async Task NavigateToFolder(OneDriveStorageItem item)
        {
            if (item.IsFolder())
            {
                Shell.Current.DisplayWaitRing = true;
                try
                {
                    var currentFolder = await _currentFolder.GetFolderAsync(item.Name);
                    Items = await currentFolder.GetItemsAsync(100);
                    _currentFolder = currentFolder;
                }
                catch (ServiceException ex)
                {
                    await DisplayMessageAsync(ex.Error.Message);
                }
                finally
                {
                    Shell.Current.DisplayWaitRing = false;
                }
            }
        }

        private DelegateCommand<OneDriveStorageItem> _renameItem = default(DelegateCommand<OneDriveStorageItem>);

        /// <summary>
        /// Gets command to rename a File or folder
        /// </summary>
        public DelegateCommand<OneDriveStorageItem> RenameItemCommand => _renameItem ?? (_renameItem = new DelegateCommand<OneDriveStorageItem>(ExecuteRenameItemCommand));

        /// <summary>
        /// Rename a folder or a file
        /// </summary>
        /// <param name="item">OneDrive item to rename</param>
        private async void ExecuteRenameItemCommand(OneDriveStorageItem item)
        {
            Shell.Current.DisplayWaitRing = false;
            try
            {
                OneDriveStorageItem renamedItem = null;
                if (item.IsFile())
                {
                    renamedItem = await ((OneDriveStorageFile)item).RenameAsync("NewName");
                }
                else if (item.IsFolder())
                {
                    renamedItem = await ((OneDriveStorageFolder)item).RenameAsync("NewName");
                }

                Items = await _currentFolder.GetItemsAsync(100);
            }
            catch (ServiceException ex)
            {
                await DisplayMessageAsync(ex.Error.Message);
            }
            finally
            {
                Shell.Current.DisplayWaitRing = false;
            }
        }

        private DelegateCommand _newFolder = default(DelegateCommand);

        /// <summary>
        /// Gets command to new folder
        /// </summary>
        public DelegateCommand NewFolderCommand
        {
            get
            {
                if (_newFolder == null)
                {
                    _newFolder = new DelegateCommand(async () =>
                        {
                            if (_currentFolder != null)
                            {
                                Shell.Current.DisplayWaitRing = true;
                                try
                                {
                                    await _currentFolder.CreateFolderAsync("NewFolder");
                                    await DisplayMessageAsync("Succeeded!");
                                    Items = await _currentFolder.GetItemsAsync(100);
                                }
                                catch (ServiceException ex)
                                {
                                    await DisplayMessageAsync(ex.Error.Message);
                                }
                                finally
                                {
                                    Shell.Current.DisplayWaitRing = false;
                                }
                            }
                        });
                }

                return _newFolder;
            }
        }
        private DelegateCommand _navigateBack = default(DelegateCommand);

        /// <summary>
        /// Gets command to new folder
        /// </summary>
        public DelegateCommand NavigateBackCommand
        {
            get
            {
                if (_navigateBack == null)
                {
                    _navigateBack = new DelegateCommand(async () =>
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

                                Items = await currentFolder.GetItemsAsync(100);
                                _currentFolder = currentFolder;

                            }
                            catch (ServiceException ex)
                            {
                                await DisplayMessageAsync(ex.Error.Message);
                            }
                            finally
                            {
                                Shell.Current.DisplayWaitRing = false;
                            }
                        }
                    });
                }

                return _navigateBack;
            }
        }

        private DelegateCommand<OneDriveStorageItem> _deleteItem = default(DelegateCommand<OneDriveStorageItem>);

        /// <summary>
        /// Gets command to delete a File or folder
        /// </summary>
        public DelegateCommand<OneDriveStorageItem> DeleteItemCommand => _deleteItem ?? (_deleteItem = new DelegateCommand<OneDriveStorageItem>(ExecuteDeleteItemCommand));

        /// <summary>
        /// Delete a folder or a file
        /// </summary>
        /// <param name="item">OneDrive item to delete</param>
        private async void ExecuteDeleteItemCommand(OneDriveStorageItem item)
        {
            MessageDialog messageDialog = new MessageDialog($"Are you sur you want to delete '{item.Name}'", "Delete");
            messageDialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(async (cmd) => 
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Shell.Current.DisplayWaitRing = true; });
                try
                {
                    await item.DeleteAsync();
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => { Items = await _currentFolder.GetItemsAsync(100); });                    
                }
                catch(ServiceException ex)
                {
                    DisplayMessageAsync(ex.Error.Message);
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

        private DelegateCommand<OneDriveStorageItem> _downloadItem = default(DelegateCommand<OneDriveStorageItem>);

        /// <summary>
        /// Gets command to download a File
        /// </summary>
        public DelegateCommand<OneDriveStorageItem> DownloadItemCommand => _downloadItem ?? (_downloadItem = new DelegateCommand<OneDriveStorageItem>(ExecuteDownloadItemCommand));

        /// <summary>
        /// Download a file
        /// </summary>
        /// <param name="item">File to download from OneDrive</param>
        private async void ExecuteDownloadItemCommand(OneDriveStorageItem item)
        {
            try
            {
                var oneDriveFile = (OneDriveStorageFile)item;
                var localFolder = ApplicationData.Current.LocalFolder;
                Shell.Current.DisplayWaitRing = true;
                using (var remoteStream = await oneDriveFile.OpenAsync())
                {
                    byte[] buffer = new byte[remoteStream.Size];
                    var localBuffer = await remoteStream.ReadAsync(buffer.AsBuffer(), (uint)remoteStream.Size, InputStreamOptions.ReadAhead);
                    var myLocalFile = await localFolder.CreateFileAsync($"{oneDriveFile.Name}", CreationCollisionOption.GenerateUniqueName);
                    using (var localStream = await myLocalFile.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        await localStream.WriteAsync(localBuffer);
                        await localStream.FlushAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayMessageAsync(ex.Message);
            }
            finally
            {
                Shell.Current.DisplayWaitRing = false;
            }
        }

        private DelegateCommand _cancelUpload;

        /// <summary>
        /// Gets command to cancel uploading a big File
        /// </summary>
        public DelegateCommand CancelCommand => _cancelUpload ?? (_cancelUpload = new DelegateCommand(() =>
       {
           if (_cancellationTokenSource != null)
            {
               _cancellationTokenSource.Cancel();
            }
       }));

        /// <summary>
        /// FileOpenPicker to open a file to upload
        /// </summary>
        /// <returns>a StorageFile representing the file to upload</returns>
        private async Task<StorageFile> OpenLocalFileAsync()
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.CommitButtonText = "Upload";
            picker.FileTypeFilter.Add("*");

           return await picker.PickSingleFileAsync();
        }

        private DelegateCommand _simpleFile = default(DelegateCommand);

        /// <summary>
        /// Gets command to upload Simple File
        /// </summary>
        public DelegateCommand SimpleItemCommand => _simpleFile ?? (_simpleFile = new DelegateCommand(ExecuteSimpleItemCommand));

        /// <summary>
        /// Upload a simple file to the parent folder
        /// </summary>
        private async void ExecuteSimpleItemCommand()
        {
            Shell.Current.DisplayWaitRing = true;

            try
            {
                if (_currentFolder != null)
                {
                    var selectedFile = await OpenLocalFileAsync();
                    if (selectedFile != null)
                    {
                        using (var localStream = await selectedFile.OpenReadAsync())
                        {

                            // You have several way to upload or create new file
                            // 1. if the file is less than 4MB Create a file
                            // var fileCreated = await currentFolder.CreateFileAsync(selectedFile.Name);
                            // and Push the content from the local file to the remote file
                            // await fileCreated.WriteAsync(localStream);
                            // 2. Pass the stream to the CreateFileAsync method
                            var fileCreated = await _currentFolder.CreateFileAsync(selectedFile.Name, localStream, OneDriveItemNameCollisionOption.Rename);
                        }

                        await DisplayMessageAsync("Succeeded!");
                        Items = await _currentFolder.GetItemsAsync(100);
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                await DisplayMessageAsync(ex.Message);
            }
            catch (ServiceException graphEx)
            {
                await DisplayMessageAsync(graphEx.Error.Message);
            }
            catch (Exception ex)
            {
                await DisplayMessageAsync(ex.Message);
            }
            finally
            {
                Shell.Current.DisplayWaitRing = false;
            }
        }

        private DelegateCommand _uploadBigFile = default(DelegateCommand);

        /// <summary>
        /// Gets command to upload a big File
        /// </summary>
        public DelegateCommand UploadBigFileCommand => _uploadBigFile ?? (_uploadBigFile = new DelegateCommand(ExecuteUploadBigFileCommand));

        /// <summary>
        /// Upload a big file.
        /// </summary>
        /// <param name="item">The destination folder to upload the file.</param>
        private async void ExecuteUploadBigFileCommand()
        {

            try
            {
                if (_currentFolder != null)
                {
                var selectedFile = await OpenLocalFileAsync();
                if (selectedFile != null)
                {
                    using (var localStream = await selectedFile.OpenReadAsync())
                    {

                            _cancellationTokenSource = new CancellationTokenSource();
                            TotalSizeFile = localStream.Size;
                            RingActive = true;
                            // If the file exceed the Maximum size (ie 4MB)
                            _currentFolder.OnUploadSession += CurrentFolder_OnUploadSession;
                            var largeFileCreated = await _currentFolder.UploadFileAsync(selectedFile.Name, localStream, _cancellationTokenSource.Token, 320 * 1024);
                    }

                        await DisplayMessageAsync("Succeeded!");
                        Items = await _currentFolder.GetItemsAsync(100);
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                await DisplayMessageAsync(ex.Message);
            }
            catch (ServiceException graphEx)
            {
                await DisplayMessageAsync(graphEx.Error.Message);
            }
            catch (Exception ex)
            {
                await DisplayMessageAsync(ex.Message);
            }
            finally
            {
                RingActive = false;
            }

        }

        private async void CurrentFolder_OnUploadSession(object sender, OneDriveUploadSessionEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
            {
                UploadProgression = e.UploadSessionTotalSize - e.UploadSessionRemaining;
            }));
        }

        private Task DisplayMessageAsync(string message)
        {
            MessageDialog msg = new MessageDialog(message);
            return msg.ShowAsync().AsTask();
        }
    }
}
