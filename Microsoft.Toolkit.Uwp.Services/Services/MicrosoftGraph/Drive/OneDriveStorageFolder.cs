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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Newtonsoft.Json;
using Windows.Storage.Streams;
using static Microsoft.Toolkit.Uwp.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class using Office 365 Microsoft Graph Drive API and representing local folder
    /// </summary>
    public class OneDriveStorageFolder : OneDriveStorageItem
    {
        private IDriveItemChildrenCollectionRequest _nextPageFoldersRequest;
        private IDriveItemChildrenCollectionRequest _nextPageFilesRequest;
        private IDriveItemChildrenCollectionRequest _nextPageItemsRequest;

        private OneDriveUploadSession _uploadSession;

        /// <summary>
        /// Instance of Http request builder for the current file
        /// </summary>
        private IDriveItemRequestBuilder _builder;

        /// <summary>
        /// Delegate raise when each chunk of file was uploaded
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">args</param>
        public delegate void OnUploadSessionHandler(object sender, OneDriveUploadSessionEventArgs e);

        /// <summary>
        /// Event raise when each chunk of file was uploaded
        /// </summary>
        public event OnUploadSessionHandler OnUploadSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveStorageFolder"/> class.
        /// <para>Permissions : Have full access to user files and files shared with user</para>
        /// </summary>
        /// <param name="graphProvider">Instance of GraphClientService class</param>
        /// <param name="builder">Http request builder.</param>
        /// <param name="driveItem">OneDrive's item</param>
        public OneDriveStorageFolder(GraphServiceClient graphProvider, IDriveItemRequestBuilder builder, DriveItem driveItem = null)
            : base(graphProvider, builder, driveItem)
        {
            _builder = builder;
        }

        /// <summary>
        /// Gets the subfolders in the current folder.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <returns>When this method completes successfully, it returns a list of the subfolders in the current folder.</returns>
        public async Task<List<OneDriveStorageFolder>> GetFoldersAsync(CancellationToken cancellationToken, int top = 20, OrderBy orderBy = OrderBy.None, string filter = null)
        {
            var oneDriveitemsRequest = CreateChildrenRequest(top, orderBy, filter);
            return await RequestOneDriveFolders(oneDriveitemsRequest, cancellationToken);
        }

        /// <summary>
        /// Gets the subfolders in the current folder.
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <returns>When this method completes successfully, it returns a list of the subfolders in the current folder.</returns>
        public Task<List<OneDriveStorageFolder>> GetFoldersAsync(int top = 20, OrderBy orderBy = OrderBy.None, string filter = null)
        {
            return GetFoldersAsync(CancellationToken.None, top, orderBy, filter);
        }

        /// <summary>
        /// Gets the subfolders in the current folder.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <returns>When this method completes successfully, it returns a list of the subfolders in the current folder.</returns>
        public async Task<OneDriveStorageItemsCollection> GetItemsAsync(CancellationToken cancellationToken, int top = 20, OrderBy orderBy = OrderBy.None, string filter = null)
        {
            IDriveItemChildrenCollectionRequest oneDriveitemsRequest = CreateChildrenRequest(top, orderBy, filter);

            return await RequestOneDriveItemstAsync(oneDriveitemsRequest, cancellationToken);
        }

        /// <summary>
        /// Gets the files and subfolders in the current folder.
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <returns>When this method completes successfully, it returns a list of the subfolders in the current folder.</returns>
        public Task<OneDriveStorageItemsCollection> GetItemsAsync(int top = 20, OrderBy orderBy = OrderBy.None, string filter = null)
        {
            return GetItemsAsync(CancellationToken.None, top, orderBy, filter);
        }

        /// <summary>
        /// Gets the files in the current folder.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <returns>When this method completes successfully, it returns a list of the files in the current folder.</returns>
        public async Task<List<OneDriveStorageFile>> GetFilesAsync(CancellationToken cancellationToken, int top = 20, OrderBy orderBy = OrderBy.None, string filter = null)
        {
            var oneDriveitemsRequest = CreateChildrenRequest(top, orderBy, filter);
            return await RequestOneDriveFilesAsync(oneDriveitemsRequest, cancellationToken, top);
        }

        /// <summary>
        /// Gets the files in the current folder.
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <returns>When this method completes successfully, it returns a list of the files in the current folder.</returns>
        public Task<List<OneDriveStorageFile>> GetFilesAsync(int top = 20, OrderBy orderBy = OrderBy.None, string filter = null)
        {
            return GetFilesAsync(CancellationToken.None, top, orderBy, filter);
        }

        /// <summary>
        /// Gets the file or folder with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the file or folder to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns an MicrosoftGraphOneDriveItem that represents the specified file or folder.</returns>
        public async Task<OneDriveStorageItem> GetItemAsync(string name, CancellationToken cancellationToken)
        {
            var oneDriveItem = await RequestChildrenAsync(name, cancellationToken);
            if (oneDriveItem == null)
            {
                return null;
            }

            if (oneDriveItem.Folder != null)
            {
                return InitializeOneDriveFolder(oneDriveItem);
            }

            if (OneDriveItem.File != null)
            {
                return InitializeOneDriveFile(oneDriveItem);
            }

            return InitializeOneDriveItem(oneDriveItem);
        }

        /// <summary>
        /// Gets the file or folder with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the file or folder to get.</param>
        /// <returns>When this method completes successfully, it returns an MicrosoftGraphOneDriveItem that represents the specified file or folder.</returns>
        public Task<OneDriveStorageItem> GetItemAsync(string name)
        {
            return GetItemAsync(name, CancellationToken.None);
        }

        /// <summary>
        /// Gets the folder with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the folder to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a MicrosoftGraphOneDriveFolder that represents the specified file.</returns>
        public async Task<OneDriveStorageFolder> GetFolderAsync(string name, CancellationToken cancellationToken)
        {
            var oneDriveItem = await RequestChildrenAsync(name, cancellationToken);

            // folder does not exist
            if (oneDriveItem == null)
            {
                return null;
            }

            return InitializeOneDriveFolder(oneDriveItem);
        }

        /// <summary>
        /// Gets the folder with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the folder to get.</param>
        /// <returns>When this method completes successfully, it returns a MicrosoftGraphOneDriveFolder that represents the specified file.</returns>
        public Task<OneDriveStorageFolder> GetFolderAsync(string name)
        {
            return GetFolderAsync(name, CancellationToken.None);
        }

        /// <summary>
        /// Gets the file with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the file to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a MicrosoftGraphOneDriveFile that represents the specified file.</returns>
        public async Task<OneDriveStorageFile> GetFileAsync(string name, CancellationToken cancellationToken)
        {
            var oneDriveItem = await RequestChildrenAsync(name, cancellationToken);
            if (oneDriveItem == null)
            {
                return null;
            }

            return InitializeOneDriveFile(oneDriveItem);
        }

        /// <summary>
        /// Gets the file with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the file to get.</param>
        /// <returns>When this method completes successfully, it returns a MicrosoftGraphOneDriveFile that represents the specified file.</returns>
        public Task<OneDriveStorageFile> GetFileAsync(string name)
        {
            return GetFileAsync(name, CancellationToken.None);
        }

        /// <summary>
        /// Renames the current folder.
        /// </summary>
        /// <param name="desiredName">The desired, new name for the current folder.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns an MicrosoftGraphOneDriveFolder that represents the specified folder.</returns>
        public async Task<OneDriveStorageFolder> RenameAsync(string desiredName, CancellationToken cancellationToken)
        {
            if (OneDriveItem.Name == "root")
            {
                throw new Microsoft.Graph.ServiceException(new Error { Message = "Could not rename the root folder" });
            }

            OneDriveItem.Name = desiredName;
            var itemRenamed = await Builder.RenameAsync(OneDriveItem, desiredName, cancellationToken);
            return InitializeOneDriveFolder(itemRenamed);
        }

        /// <summary>
        /// Renames the current folder.
        /// </summary>
        /// <param name="desiredName">The desired, new name for the current folder.</param>
        /// <returns>When this method completes successfully, it returns an MicrosoftGraphOneDriveFolder that represents the specified folder.</returns>
        public Task<OneDriveStorageFolder> RenameAsync(string desiredName)
        {
            return RenameAsync(desiredName, CancellationToken.None);
        }

        /// <summary>
        /// Creates a new subfolder in the current folder.
        /// </summary>
        /// <param name="name">The name of the new subfolder to create in the current folder.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, it returns a MicrosoftGraphOneDriveFolder that represents the new subfolder.</returns>
        public async Task<OneDriveStorageFolder> CreateFolderAsync(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var request = _builder.Children.Request();

            DriveItem folderToCreate = new DriveItem { Name = name, Folder = new Folder { } };
            var folderCreated = await request.AddAsync(folderToCreate, cancellationToken).ConfigureAwait(false);
            return InitializeOneDriveFolder(folderCreated);
        }

        /// <summary>
        /// Creates a new subfolder in the current folder.
        /// </summary>
        /// <param name="name">The name of the new subfolder to create in the current folder.</param>
        /// <returns>When this method completes, it returns a MicrosoftGraphOneDriveFolder that represents the new subfolder.</returns>
        public Task<OneDriveStorageFolder> CreateFolderAsync(string name)
        {
            return CreateFolderAsync(name, CancellationToken.None);
        }

        /// <summary>
        /// Creates a new large file in the current folder.
        /// Use this method when your file is larger than
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <param name="content">The data's stream to push into the file</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="maxChunkSize">Max chunk size must be a multiple of 320 KiB (ie: 320*1024)</param>
        /// <returns>When this method completes, it returns a MicrosoftGraphOneDriveFile that represents the new file.</returns>
        public async Task<OneDriveStorageFile> UploadFileAsync(string desiredName, IRandomAccessStream content, CancellationToken cancellationToken, int maxChunkSize = -1)
        {
            int currentChunkSize = maxChunkSize < 0 ? OneDriveConstants.DefaultMaxChunkSizeForUploadSession : maxChunkSize;

            if (currentChunkSize % OneDriveConstants.RequiredChunkSizeIncrementForUploadSession != 0)
            {
                throw new ArgumentException("Max chunk size must be a multiple of 320 KiB", nameof(maxChunkSize));
            }

            if (string.IsNullOrEmpty(desiredName))
            {
                throw new ArgumentNullException(nameof(desiredName));
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var streamToUpload = content.AsStreamForRead();

            if (!streamToUpload.CanRead || !streamToUpload.CanSeek)
            {
                throw new ArgumentException("Must provide stream that can read and seek");
            }

            _uploadSession = await this.CreateUploadSessionAsync(Provider, desiredName, OneDriveItemNameCollisionOption.Fail);

            return await UploadChunkStreamAsync(_uploadSession, streamToUpload, currentChunkSize, cancellationToken);
        }

        /// <summary>
        /// Creates a new file in the current folder. This method also specifies what to
        /// do if a file with the same name already exists in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <param name="content">The data's stream to push into the file</param>
        /// <param name="option">One of the enumeration values that determines how to handle the collision if a file with the specified desiredName already exists in the current folder.</param>
        /// <remarks>With OneDrive Consumer, the content could not be null</remarks>
        /// One of the enumeration values that determines how to handle the collision if
        /// a file with the specified desiredNewName already exists in the destination folder.
        /// Default : Fail
        /// <returns>When this method completes, it returns a MicrosoftGraphOneDriveFile that represents the new file.</returns>
        public async Task<OneDriveStorageFile> CreateFileAsync(string desiredName, IRandomAccessStream content, OneDriveItemNameCollisionOption option = OneDriveItemNameCollisionOption.Fail)
        {
            if (string.IsNullOrEmpty(desiredName))
            {
                throw new ArgumentNullException(nameof(desiredName));
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (content.Size > OneDriveConstants.SimpleUploadMaxSize)
            {
                throw new ServiceException(new Error { Message = "The file size cannot exceed 4MB, use UploadFileAsync instead ", Code = "MaxSizeExceeded", ThrowSite = "UWP Community Toolkit" });
            }

            DriveItem fileCreated = await Builder.CreateFileAsync(Provider, desiredName, content, option);

            return InitializeOneDriveFile(fileCreated);
        }

        /// <summary>
        /// Creates a new file in the current folder.
        /// </summary>
        /// <param name="name">The name of the new file to create in the current folder.</param>
        /// <returns>When this method completes, it returns a MicrosoftGraphOneDriveFile that represents the new file.</returns>
        public async Task<OneDriveStorageFile> CreateFileAsync(string name)
        {
            // Because OneDrive (Not OneDrive For business) don't allow to create a file with no content
            // Put a single byte, then the caller can call Item.WriteAsync() to put the real content in the file
            InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream();
            byte[] buffer = new byte[1];
            buffer[0] = 0x0;
            await ms.WriteAsync(buffer.AsBuffer());

            return await CreateFileAsync(name, ms, OneDriveItemNameCollisionOption.Fail);
        }

        /// <summary>
        /// Retrieve the next page of files
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The next collection of files or null if there are no more files</returns>
        public async Task<List<OneDriveStorageFile>> NextFilesAsync(CancellationToken cancellationToken)
        {
            if (_nextPageFilesRequest != null)
            {
                return await RequestOneDriveFilesAsync(_nextPageFilesRequest, cancellationToken);
            }

            // no more folders
            return null;
        }

        /// <summary>
        /// Retrieve the next page of files
        /// </summary>
        /// <returns>The next collection of files or null if there are no more files</returns>
        public Task<List<OneDriveStorageFile>> NextFilesAsync()
        {
            return NextFilesAsync(CancellationToken.None);
        }

        /// <summary>
        /// Retrieve the next page of items
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The next collection of items or null if there are no more items (an item could be a folder or  file)</returns>
        public async Task<OneDriveStorageItemsCollection> NextItemsAsync(CancellationToken cancellationToken)
        {
            if (_nextPageItemsRequest != null)
            {
                return await RequestOneDriveItemstAsync(_nextPageItemsRequest, cancellationToken);
            }

            // no more items
            return null;
        }

        /// <summary>
        /// Retrieve the next page of files
        /// </summary>
        /// <returns>The next collection of items or null if there are no more items (an item could be a folder or  file)</returns>
        public Task<OneDriveStorageItemsCollection> NextItemsAsync()
        {
            return NextItemsAsync(CancellationToken.None);
        }

        /// <summary>
        /// Retrieve the next page of folders
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The next collection of folders or null if there are no more folders</returns>
        public async Task<List<OneDriveStorageFolder>> NextFoldersAsync(CancellationToken cancellationToken)
        {
            if (_nextPageFoldersRequest != null)
            {
                return await RequestOneDriveFolders(_nextPageFoldersRequest, cancellationToken);
            }

            // no more folders
            return null;
        }

        /// <summary>
        /// Retrieve the next page of folders
        /// </summary>
        /// <returns>The next collection of folders or null if there are no more files</returns>
        public Task<List<OneDriveStorageFolder>> NextFoldersAsync()
        {
            return NextFoldersAsync(CancellationToken.None);
        }

        /// <summary>
        /// Delete the session.
        /// </summary>
        /// <returns>Once returned task is complete, the session has been deleted.</returns>
        private async Task DeleteSessionAsync()
        {
            await this.DeleteSession(Provider, _uploadSession);
        }

        /// <summary>
        /// Requests from OneDrive the file or folder with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the file or folder to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a DriveItem that represents the specified file or folder.</returns>
        private async Task<DriveItem> RequestChildrenAsync(string name, CancellationToken cancellationToken)
        {
            DriveItem oneDriveItem = null;
            var requestUrl = Builder.RequestUrl + $":/{name}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            await Provider.AuthenticationProvider.AuthenticateRequestAsync(request);
            var response = await Provider.HttpProvider.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                oneDriveItem = JsonConvert.DeserializeObject<DriveItem>(jsonData);
            }

            return oneDriveItem;
        }

        /// <summary>
        /// Request a list of DriveItem from oneDrive and create a list of MicrosoftGraphOneDriveFolder
        /// </summary>
        /// <param name="request">Http request to execute</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a List of MicrosoftGraphOneDriveFolder that represents the specified folders</returns>
        private async Task<List<OneDriveStorageFolder>> RequestOneDriveFolders(IDriveItemChildrenCollectionRequest request, CancellationToken cancellationToken)
        {
            var oneDriveItems = await request.GetAsync(cancellationToken);

            _nextPageFoldersRequest = oneDriveItems.NextPageRequest;

            List<DriveItem> oneDriveFolders = QueryFolders(oneDriveItems);
            List<OneDriveStorageFolder> folders = new List<OneDriveStorageFolder>();

            foreach (var oneDriveFolder in oneDriveFolders)
            {
                folders.Add(InitializeOneDriveFolder(oneDriveFolder));
            }

            return folders;
        }

        /// <summary>
        /// Request a list of DriveItem from oneDrive and create a MicrosoftGraphOneDriveItemCollection Collection
        /// </summary>
        /// <param name="request">Http request to execute</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns MicrosoftGraphOneDriveItemCollection that represents the specified files or folders</returns>
        private async Task<OneDriveStorageItemsCollection> RequestOneDriveItemstAsync(IDriveItemChildrenCollectionRequest request, CancellationToken cancellationToken)
        {
            var oneDriveItems = await request.GetAsync(cancellationToken);

            _nextPageItemsRequest = oneDriveItems.NextPageRequest;

            List<OneDriveStorageItem> items = new List<OneDriveStorageItem>();

            foreach (var oneDriveItem in oneDriveItems)
            {
                items.Add(InitializeOneDriveItem(oneDriveItem));
            }

            return new OneDriveStorageItemsCollection(items);
        }

        /// <summary>
        /// Get the next range to upload
        /// </summary>
        /// <param name="uploadSession">The upload Session</param>
        /// <param name="totalUploadLength">The file's total size</param>
        /// <returns>the next range</returns>
        private List<Tuple<long, long>> GetNextRanges(OneDriveUploadSession uploadSession, long totalUploadLength)
        {
            var newRangesRemaining = new List<Tuple<long, long>>();
            foreach (var range in uploadSession.NextExpectedRanges)
            {
                var rangeSpecifiers = range.Split('-');
                newRangesRemaining.Add(new Tuple<long, long>(
                    long.Parse(rangeSpecifiers[0]), string.IsNullOrEmpty(rangeSpecifiers[1]) ? totalUploadLength - 1 : long.Parse(rangeSpecifiers[1])));
            }

            return newRangesRemaining;
        }

        /// <summary>
        /// Upload the stream
        /// </summary>
        /// <param name="uploadSession">Upload session</param>
        /// <param name="streamToUpload">The stream to uplaod</param>
        /// <param name="maxChunkSize">Size of the chunk</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns OneDriveStorageFile that represents the specified file</returns>
        private async Task<OneDriveStorageFile> UploadChunkStreamAsync(OneDriveUploadSession uploadSession, Stream streamToUpload, int maxChunkSize, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            var totalUploadLength = streamToUpload.Length;
            var newRangesRemaining = GetNextRanges(uploadSession, totalUploadLength);
            var readBuffer = new byte[maxChunkSize];
            foreach (var range in newRangesRemaining)
            {
                var currentRangeBegins = range.Item1;
                int chunckSize = 0;
                while (currentRangeBegins <= range.Item2)
                {
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var sizeBasedOnRange = (int)(range.Item2 - currentRangeBegins) + 1;
                        var nextChunkSize = sizeBasedOnRange > maxChunkSize ? maxChunkSize : sizeBasedOnRange;
                        chunckSize = currentRangeBegins == 0 ? nextChunkSize : chunckSize += nextChunkSize;
                        HttpRequestMessage requestUpload = new HttpRequestMessage(HttpMethod.Put, uploadSession.UploadUrl);
                        streamToUpload.Seek(currentRangeBegins, SeekOrigin.Begin);
                        await streamToUpload.ReadAsync(readBuffer, 0, nextChunkSize).ConfigureAwait(false);
                        using (MemoryStream requestStream = new MemoryStream(nextChunkSize))
                        {
                            await requestStream.WriteAsync(readBuffer, 0, nextChunkSize).ConfigureAwait(false);
                            requestStream.Seek(0, SeekOrigin.Begin);

                            requestUpload.Content = new StreamContent(requestStream);
                            requestUpload.Content.Headers.ContentRange = new ContentRangeHeaderValue(currentRangeBegins, chunckSize - 1, totalUploadLength);
                            requestUpload.Content.Headers.ContentLength = nextChunkSize;
                            response = await Provider.Me.Client.HttpProvider.SendAsync(requestUpload).ConfigureAwait(false);
                            if (!response.IsSuccessStatusCode)
                            {
                                throw new HttpRequestException(response.ReasonPhrase);
                            }

                            OnUploadSession?.Invoke(this, new OneDriveUploadSessionEventArgs(totalUploadLength, maxChunkSize, totalUploadLength - currentRangeBegins, null));
                            currentRangeBegins += nextChunkSize;
                        }
                    }
                    catch (Exception ex) when (ex is HttpRequestException || ex is ServiceException || ex is TaskCanceledException || ex is OperationCanceledException)
                    {
                        await DeleteSessionAsync();
                        throw;
                    }
                }

                string jsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                DriveItem fileCreated = JsonConvert.DeserializeObject<DriveItem>(jsonData);
                var oneDriveFile = InitializeOneDriveFile(fileCreated);
                OnUploadSession?.Invoke(this, new OneDriveUploadSessionEventArgs(totalUploadLength, maxChunkSize, totalUploadLength - currentRangeBegins, oneDriveFile));
                return oneDriveFile;
            }

            return null;
        }

        /// <summary>
        /// Request a list of DriveItem from oneDrive and create a list of MicrosoftGraphOneDriveFile
        /// </summary>
        /// <param name="request">Http request to execute</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <returns>When this method completes successfully, it returns a list of MicrosoftGraphOneDriveFile that represents the specified files</returns>
        private async Task<List<OneDriveStorageFile>> RequestOneDriveFilesAsync(IDriveItemChildrenCollectionRequest request, CancellationToken cancellationToken, int top = 20)
        {
            var oneDriveItems = await request.GetAsync(cancellationToken);

            _nextPageFilesRequest = oneDriveItems.NextPageRequest;

            // TODO: The first items on the list are never a file
            List<DriveItem> oneDriveFiles = QueryFiles(oneDriveItems);

            // TODO: Algo to get only File
            List<OneDriveStorageFile> files = new List<OneDriveStorageFile>();

            foreach (var oneDriveFile in oneDriveFiles)
            {
                files.Add(InitializeOneDriveFile(oneDriveFile));
            }

            return files;
        }

        /// <summary>
        /// Filter the DriveItem of File type
        /// </summary>
        /// <param name="itemFiles">Collection of DriveItem</param>
        /// <returns>A list of DriveItem</returns>
        private List<DriveItem> QueryFiles(IDriveItemChildrenCollectionPage itemFiles)
        {
            var query = from f in itemFiles.CurrentPage where f.File != null select f;
            return query.ToList<DriveItem>();
        }

        /// <summary>
        /// Filter the DriveItem of Folder type
        /// </summary>
        /// <param name="itemFolders">Collection of DriveItem </param>
        /// <returns>Only the DriveItems folders </returns>
        private List<DriveItem> QueryFolders(IDriveItemChildrenCollectionPage itemFolders)
        {
            var query = from f in itemFolders.CurrentPage where f.Folder != null || f.SpecialFolder != null select f;
            return query.ToList<DriveItem>();
        }

        /// <summary>
        /// Create the children http request
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <returns>Returns the http request</returns>
        private IDriveItemChildrenCollectionRequest CreateChildrenRequest(int top, OrderBy orderBy = OrderBy.None, string filter = null)
        {
            IDriveItemChildrenCollectionRequest oneDriveitemsRequest = null;
            if (orderBy == OrderBy.None)
            {
                oneDriveitemsRequest = _builder.Children.Request().Top(top).Filter(filter);
            }
            else
            {
                string order = $"{orderBy} asc";
                oneDriveitemsRequest = _builder.Children.Request().Top(top).OrderBy(order).Filter(filter);
            }

            return oneDriveitemsRequest;
        }
    }
}
