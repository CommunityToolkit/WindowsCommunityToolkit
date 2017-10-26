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
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;
using OneDriveSdk = Microsoft.OneDrive.Sdk;
using OneDriveSdkHelper = Microsoft.OneDrive.Sdk.Helpers;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// OneDriveStorageFolder Type
    /// </summary>
    public class OneDriveStorageFolder : OneDriveStorageItem, IOneDriveStorageFolder
    {
        private IItemChildrenCollectionRequest _nextPageItemsRequest;
        private IItemChildrenCollectionRequest _nextPageFoldersRequest;
        private IItemChildrenCollectionRequest _nextPageFilesRequest;

        /// <summary>
        /// Requests from OneDrive the file or folder with the specified name from the current folder.
        /// </summary>
        /// <param name="path">The name (or path relative to the current folder) of the file or folder to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a DriveItem that represents the specified file or folder.</returns>
        private async Task<DriveItem> RequestChildrenAsync(string path, CancellationToken cancellationToken)
        {
            var oneDriveItem = await ((IItemRequestBuilder)RequestBuilder).ItemWithPath(path).Request().GetAsync(cancellationToken).ConfigureAwait(false);
            return oneDriveItem.CopyToDriveItem();
        }

        private OneDriveSdkHelper.ChunkedUploadProvider _uploadProvider;

        private Microsoft.OneDrive.Sdk.UploadSession _uploadSession;

        /// <summary>
        /// Gets the upload provider
        /// </summary>
        private OneDriveSdkHelper.ChunkedUploadProvider UploadProvider
        {
            get { return _uploadProvider; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if a large file upload is completed
        /// </summary>
        public bool IsUploadCompleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveStorageFolder"/> class.
        /// <para>Permissions : Have full access to user files and files shared with user</para>
        /// </summary>
        /// <param name="oneDriveProvider">Instance of OneDriveClient class</param>
        /// <param name="requestBuilder">Http request builder.</param>
        /// <param name="oneDriveItem">OneDrive's item</param>
        public OneDriveStorageFolder(IBaseClient oneDriveProvider, IBaseRequestBuilder requestBuilder, DriveItem oneDriveItem)
            : base(oneDriveProvider, requestBuilder, oneDriveItem)
        {
        }

        /// <inheritdoc/>
        public async new Task<IOneDriveStorageFolder> RenameAsync(string desiredName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var renameItem = await base.RenameAsync(desiredName, cancellationToken);
            return InitializeOneDriveStorageFolder(renameItem.OneDriveItem);
        }

        /// <inheritdoc/>
        public async Task<IOneDriveStorageFile> CreateFileAsync(string desiredName,  CreationCollisionOption options = CreationCollisionOption.FailIfExists, IRandomAccessStream content = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Stream streamContent = null;
            if (string.IsNullOrEmpty(desiredName))
            {
                throw new ArgumentNullException(nameof(desiredName));
            }

            if (content == null)
            {
                // Because OneDrive (Not OneDrive For business) don't allow to create a file with no content
                // Put a single byte, then the caller can call Item.WriteAsync() to put the real content in the file
                byte[] buffer = new byte[1];
                buffer[0] = 0x00;
                streamContent = new MemoryStream(buffer);
            }
            else
            {
                if (content.Size > OneDriveUploadConstants.SimpleUploadMaxSize)
                {
                    throw new ServiceException(new Error { Message = "The file size cannot exceed 4MB, use UploadFileAsync instead ", Code = "MaxSizeExceeded", ThrowSite = "UWP Community Toolkit" });
                }

                streamContent = content.AsStreamForRead();
            }

            var childrenRequest = ((IItemRequestBuilder)RequestBuilder).Children.Request();
            string requestUri = $"{childrenRequest.RequestUrl}/{desiredName}/content?@name.conflictBehavior={OneDriveHelper.TransformCollisionOptionToConflictBehavior(options.ToString())}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, requestUri)
            {
                Content = new StreamContent(streamContent)
            };
            var createdFile = await ((IOneDriveClient)Provider).SendAuthenticatedRequestAsync(request, cancellationToken);

            return InitializeOneDriveStorageFile(createdFile.CopyToDriveItem());
        }

        /// <inheritdoc/>
        public async Task<IOneDriveStorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options = CreationCollisionOption.FailIfExists, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(desiredName))
            {
                throw new ArgumentNullException(nameof(desiredName));
            }

            var childrenRequest = ((IItemRequestBuilder)RequestBuilder).Children.Request();
            var requestUri = childrenRequest.RequestUrl;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            Item item = new Item { Name = desiredName, Folder = new Microsoft.OneDrive.Sdk.Folder() { } };
            item.AdditionalData = new Dictionary<string, object>();
            item.AdditionalData.Add(new KeyValuePair<string, object>("@name.conflictBehavior", OneDriveHelper.TransformCollisionOptionToConflictBehavior(options.ToString())));
            var jsonOptions = JsonConvert.SerializeObject(item);
            request.Content = new StringContent(jsonOptions, System.Text.Encoding.UTF8, "application/json");

            var createdFolder = await ((IOneDriveClient)Provider).SendAuthenticatedRequestAsync(request, cancellationToken).ConfigureAwait(false);

            return InitializeOneDriveStorageFolder(createdFolder.CopyToDriveItem());
        }

        /// <inheritdoc/>
        public async Task<IOneDriveStorageFile> GetFileAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var oneDriveItem = await RequestChildrenAsync(name, cancellationToken);
            if (oneDriveItem == null)
            {
                return null;
            }

            return InitializeOneDriveStorageFile(oneDriveItem);
        }

        /// <inheritdoc/>
        public async Task<List<IOneDriveStorageFile>> GetFilesAsync(int top = 20, OrderBy orderBy = OrderBy.None, string filter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            IItemChildrenCollectionRequest oneDriveItemsRequest = CreateChildrenRequest(top, orderBy, filter);
            return await RequestOneDriveFilesAsync(oneDriveItemsRequest, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IOneDriveStorageFolder> GetFolderAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var oneDriveItem = await RequestChildrenAsync(name, cancellationToken).ConfigureAwait(false);
            return InitializeOneDriveStorageFolder(oneDriveItem);
        }

        /// <inheritdoc/>
        public async Task<List<IOneDriveStorageFolder>> GetFoldersAsync(int top = 100, OrderBy orderBy = OrderBy.None, string filter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            IItemChildrenCollectionRequest oneDriveItemsRequest = CreateChildrenRequest(top, orderBy, filter);
            return await RequestOneDriveFoldersAsync(oneDriveItemsRequest, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IOneDriveStorageItem> GetItemAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var oneDriveItem = await RequestChildrenAsync(name, cancellationToken).ConfigureAwait(false);
            return InitializeOneDriveStorageItem(oneDriveItem);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<IOneDriveStorageItem>> GetItemsAsync(int top, OrderBy orderBy = OrderBy.None, string filter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            IItemChildrenCollectionRequest oneDriveItemsRequest = RequestBuilder.CreateChildrenRequest(top, orderBy, filter);
            return await RequestOneDriveItemsAsync(oneDriveItemsRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public IncrementalLoadingCollection<OneDriveRequestSource<IOneDriveStorageItem>, IOneDriveStorageItem> GetItemsAsync(OrderBy orderBy = OrderBy.None)
        {
            var requestSource = new OneDriveRequestSource<IOneDriveStorageItem>(Provider, RequestBuilder, orderBy, null);
            return new IncrementalLoadingCollection<OneDriveRequestSource<IOneDriveStorageItem>, IOneDriveStorageItem>(requestSource);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<IOneDriveStorageItem>> GetItemsAsync(uint startIndex, uint maxItemsToRetrieve, CancellationToken cancellationToken = default(CancellationToken))
        {
            IItemChildrenCollectionRequest oneDriveitemsRequest = null;
            var request = ((IItemRequestBuilder)RequestBuilder).Children.Request();

            // skip is not working right now
            // oneDriveitemsRequest = request.Top((int)maxItemsToRetrieve).Skip((int)startIndex);
            int maxToRetrieve = (int)(maxItemsToRetrieve + startIndex);
            oneDriveitemsRequest = request.Top(maxToRetrieve);
            var tempo = await oneDriveitemsRequest.GetAsync(cancellationToken).ConfigureAwait(false);

            List<IOneDriveStorageItem> items = new List<IOneDriveStorageItem>();

            for (int i = (int)startIndex; i < maxToRetrieve && i < tempo.Count; i++)
            {
                items.Add(InitializeOneDriveStorageItem(tempo[i].CopyToDriveItem()));
            }

            return new OneDriveStorageItemsCollection(items);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<IOneDriveStorageItem>> NextItemsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_nextPageItemsRequest != null)
            {
                return await RequestOneDriveItemsAsync(_nextPageItemsRequest, cancellationToken);
            }

            // no more items
            return null;
        }

        /// <inheritdoc/>
        public async Task CancelSessionAsync()
        {
            if (_uploadProvider != null)
            {
                await _uploadProvider.DeleteSession();
            }
        }

        /// <inheritdoc/>
        public async Task<List<IOneDriveStorageFolder>> NextFoldersAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_nextPageFoldersRequest != null)
            {
                return await RequestOneDriveFoldersAsync(_nextPageFoldersRequest, cancellationToken);
            }

            // no more folders
            return null;
        }

        /// <inheritdoc/>
        public async Task<List<IOneDriveStorageFile>> NextFilesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_nextPageFilesRequest != null)
            {
                return await RequestOneDriveFilesAsync(_nextPageFilesRequest, cancellationToken);
            }

            // no more folders
            return null;
        }

        /// <inheritdoc/>
        public async Task<long> GetUploadStatusAsync()
        {
            if (_uploadProvider != null && IsUploadCompleted == false)
            {
                if (Provider.AuthenticationProvider is MsaAuthenticationProvider)
                {
                    var remainingRequests = _uploadProvider.GetUploadChunkRequests();
                    var firstChunck = remainingRequests.First<OneDriveSdk.UploadChunkRequest>();
                    var status = await _uploadProvider.UpdateSessionStatusAsync();
                    return firstChunck.RangeBegin;
                }
            }

            // no more data
            return 0;
        }

        /// <inheritdoc/>
        public async Task<IOneDriveStorageFile> UploadFileAsync(string desiredName, IRandomAccessStream content, CreationCollisionOption options = CreationCollisionOption.FailIfExists,   int maxChunkSize = -1)
        {
            int currentChunkSize = maxChunkSize < 0 ? OneDriveUploadConstants.DefaultMaxChunkSizeForUploadSession : maxChunkSize;

            if (currentChunkSize % OneDriveUploadConstants.RequiredChunkSizeIncrementForUploadSession != 0)
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

            var uploadSessionUri = $"{Provider.BaseUrl}/drive/items/{OneDriveItem.Id}:/{desiredName}:/oneDrive.createSession";

            var conflictBehavior = new OneDriveItemConflictBehavior { Item = new OneDriveConflictItem { ConflictBehavior = OneDriveHelper.TransformCollisionOptionToConflictBehavior(options.ToString()) } };

            var jsonConflictBehavior = JsonConvert.SerializeObject(conflictBehavior);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uploadSessionUri)
            {
                Content = new StringContent(jsonConflictBehavior, Encoding.UTF8, "application/json")
            };
            await Provider.AuthenticationProvider.AuthenticateRequestAsync(request).ConfigureAwait(false);

            var response = await Provider.HttpProvider.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new ServiceException(new Error { Message = "Could not create an UploadSession", Code = "NoUploadSession", ThrowSite = "UWP Community Toolkit" });
            }

            IsUploadCompleted = false;
            var jsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            _uploadSession = JsonConvert.DeserializeObject<Microsoft.OneDrive.Sdk.UploadSession>(jsonData);

            var streamToUpload = content.AsStreamForRead();
            _uploadProvider = new Microsoft.OneDrive.Sdk.Helpers.ChunkedUploadProvider(_uploadSession, Provider, streamToUpload, maxChunkSize);

            var uploadedItem = await _uploadProvider.UploadAsync().ConfigureAwait(false);
            IsUploadCompleted = true;

            return InitializeOneDriveStorageFile(uploadedItem.CopyToDriveItem());
        }

        /// <summary>
        /// Create children http request with specific options
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <returns>Returns the http request</returns>
        private IItemChildrenCollectionRequest CreateChildrenRequest(int top, OrderBy orderBy = OrderBy.None, string filter = null)
        {
            IItemChildrenCollectionRequest oneDriveitemsRequest = null;
            if (orderBy == OrderBy.None && string.IsNullOrEmpty(filter))
            {
                return ((IItemRequestBuilder)RequestBuilder).Children.Request().Top(top);
            }

            if (orderBy == OrderBy.None)
            {
                return ((IItemRequestBuilder)RequestBuilder).Children.Request().Top(top).Filter(filter);
            }

            string order = $"{orderBy} asc".ToLower();

            if (string.IsNullOrEmpty(filter))
            {
                oneDriveitemsRequest = ((IItemRequestBuilder)RequestBuilder).Children.Request().Top(top).OrderBy(order);
            }
            else
            {
                oneDriveitemsRequest = ((IItemRequestBuilder)RequestBuilder).Children.Request().Top(top).OrderBy(order).Filter(filter);
            }

            return oneDriveitemsRequest;
        }

        /// <summary>
        /// Request a list of DriveItem from oneDrive and create a MicrosoftGraphOneDriveItemCollection Collection
        /// </summary>
        /// <param name="request">Http request to execute</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns MicrosoftGraphOneDriveItemCollection that represents the specified files or folders</returns>
        private async Task<OneDriveStorageItemsCollection> RequestOneDriveItemsAsync(IItemChildrenCollectionRequest request, CancellationToken cancellationToken)
        {
            var oneDriveItems = await request.GetAsync(cancellationToken).ConfigureAwait(false);

            _nextPageItemsRequest = oneDriveItems.NextPageRequest;

            List<IOneDriveStorageItem> items = new List<IOneDriveStorageItem>();

            foreach (var oneDriveItem in oneDriveItems)
            {
                items.Add(InitializeOneDriveStorageItem(oneDriveItem.CopyToDriveItem()));
            }

            return new OneDriveStorageItemsCollection(items);
        }

        /// <summary>
        /// Request a list of Folder from oneDrive and create a MicrosoftGraphOneDriveItemCollection Collection
        /// </summary>
        /// <param name="request">Http request to execute</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns MicrosoftGraphOneDriveItemCollection that represents the specified files or folders</returns>
        private async Task<List<IOneDriveStorageFolder>> RequestOneDriveFoldersAsync(IItemChildrenCollectionRequest request, CancellationToken cancellationToken)
        {
            var oneDriveItems = await request.GetAsync(cancellationToken);

            _nextPageFoldersRequest = oneDriveItems.NextPageRequest;

            List<Item> oneDriveFolders = QueryFolders(oneDriveItems);
            List<IOneDriveStorageFolder> folders = new List<IOneDriveStorageFolder>();

            foreach (var oneDriveFolder in oneDriveFolders)
            {
                folders.Add(InitializeOneDriveStorageFolder(oneDriveFolder.CopyToDriveItem()));
            }

            return folders;
        }

        /// <summary>
        /// Filter folder
        /// </summary>
        /// <param name="itemFolders">Collection of DriveItem </param>
        /// <returns>Only the DriveItems folders </returns>
        private List<Item> QueryFolders(IItemChildrenCollectionPage itemFolders)
        {
            var query = from f in itemFolders.CurrentPage where f.Folder != null || f.SpecialFolder != null select f;
            return query.ToList<Item>();
        }

        /// <summary>
        /// Request a list of items
        /// </summary>
        /// <param name="request">Http request to execute</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a list of OneDriveStorageFile that represents the specified files</returns>
        private async Task<List<IOneDriveStorageFile>> RequestOneDriveFilesAsync(IItemChildrenCollectionRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var oneDriveItems = await request.GetAsync(cancellationToken);

            _nextPageFilesRequest = oneDriveItems.NextPageRequest;

            // TODO: The first items on the list are never a file
            List<Item> oneDriveFiles = QueryFiles(oneDriveItems);

            // TODO: Algo to get only File
            List<IOneDriveStorageFile> files = new List<IOneDriveStorageFile>();

            foreach (var oneDriveFile in oneDriveFiles)
            {
                files.Add(InitializeOneDriveStorageFile(oneDriveFile.CopyToDriveItem()));
            }

            return files;
        }

        /// <summary>
        /// Filter files
        /// </summary>
        /// <param name="itemFiles">Collection of DriveItem</param>
        /// <returns>A list of DriveItem</returns>
        private List<Item> QueryFiles(IItemChildrenCollectionPage itemFiles)
        {
            var query = from f in itemFiles.CurrentPage where f.File != null select f;
            return query.ToList<Item>();
        }
    }
}
