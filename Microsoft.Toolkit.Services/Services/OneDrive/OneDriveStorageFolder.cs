// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using static Microsoft.Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    /// OneDriveStorageFolder Type
    /// </summary>
    public class OneDriveStorageFolder : OneDriveStorageItem
    {
        private IDriveItemChildrenCollectionRequest _nextPageItemsRequest;
        private IDriveItemChildrenCollectionRequest _nextPageFoldersRequest;
        private IDriveItemChildrenCollectionRequest _nextPageFilesRequest;

        /// <summary>
        /// Gets or sets platform-specific implementation of platform services.
        /// </summary>
        public IOneDriveStorageFolderPlatform StorageFolderPlatformService { get; set; }

        /// <summary>
        /// Requests from OneDrive the file or folder with the specified name from the current folder.
        /// </summary>
        /// <param name="path">The name (or path relative to the current folder) of the file or folder to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a DriveItem that represents the specified file or folder.</returns>
        private Task<DriveItem> RequestChildrenAsync(string path, CancellationToken cancellationToken)
        {
            return ((IDriveItemRequestBuilder)RequestBuilder).ItemWithPath(path).Request().GetAsync(cancellationToken);
        }

        private ChunkedUploadProvider _uploadProvider;

        private UploadSession _uploadSession;

        /// <summary>
        /// Gets or sets the upload provider
        /// </summary>
        internal ChunkedUploadProvider UploadProvider
        {
            get { return _uploadProvider; }
            set { _uploadProvider = value; }
        }

        /// <summary>
        /// Gets or sets the upload session
        /// </summary>
        internal UploadSession UploadSession
        {
            get { return _uploadSession; }
            set { _uploadSession = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if a large file upload is completed
        /// </summary>
        public bool IsUploadCompleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveStorageFolder"/> class.
        /// <para>Permissions : Have full access to user files and files shared with user</para>
        /// </summary>
        /// <param name="oneDriveProvider">Instance of Graph Client class</param>
        /// <param name="requestBuilder">Http request builder.</param>
        /// <param name="oneDriveItem">OneDrive's item</param>
        public OneDriveStorageFolder(IBaseClient oneDriveProvider, IBaseRequestBuilder requestBuilder, DriveItem oneDriveItem)
            : base(oneDriveProvider, requestBuilder, oneDriveItem)
        {
            StorageFolderPlatformService = OneDriveService.ServicePlatformInitializer.CreateOneDriveStorageFolderPlatformInstance(OneDriveService.Instance, this);
        }

        /// <summary>
        /// Renames the current folder.
        /// </summary>
        /// <param name="desiredName">The desired, new name for the current folder.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns an IOneDriveStorageFolder that represents the specified folder.</returns>
        public async new Task<OneDriveStorageFolder> RenameAsync(string desiredName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var renameItem = await base.RenameAsync(desiredName, cancellationToken);
            return InitializeOneDriveStorageFolder(renameItem.OneDriveItem);
        }

        /// <summary>
        /// Gets the file with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the file to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a IOneDriveStorageFile that represents the specified file.</returns>
        public async Task<OneDriveStorageFile> GetFileAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var oneDriveItem = await RequestChildrenAsync(name, cancellationToken);
            if (oneDriveItem == null)
            {
                return null;
            }

            return InitializeOneDriveStorageFile(oneDriveItem);
        }

        /// <summary>
        /// Gets the files in the current folder.
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a list of the files in the current folder.</returns>
        public Task<List<OneDriveStorageFile>> GetFilesAsync(int top = 20, OrderBy orderBy = OrderBy.None, string filter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            IDriveItemChildrenCollectionRequest oneDriveItemsRequest = CreateChildrenRequest(top, orderBy, filter);
            return RequestOneDriveFilesAsync(oneDriveItemsRequest, cancellationToken);
        }

        /// <summary>
        /// Gets the folder with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the folder to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a IOneDriveStorageFolder that represents the specified file.</returns>
        public async Task<OneDriveStorageFolder> GetFolderAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var oneDriveItem = await RequestChildrenAsync(name, cancellationToken).ConfigureAwait(false);
            return InitializeOneDriveStorageFolder(oneDriveItem);
        }

        /// <summary>
        /// Gets the subfolders in the current folder.
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a list of the subfolders in the current folder.</returns>
        public Task<List<OneDriveStorageFolder>> GetFoldersAsync(int top = 100, OrderBy orderBy = OrderBy.None, string filter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            IDriveItemChildrenCollectionRequest oneDriveItemsRequest = CreateChildrenRequest(top, orderBy, filter);
            return RequestOneDriveFoldersAsync(oneDriveItemsRequest, cancellationToken);
        }

        /// <summary>
        /// Gets the item with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the folder to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a IOneDriveStorageItem that represents the specified file.</returns>
        public async Task<OneDriveStorageItem> GetItemAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var oneDriveItem = await RequestChildrenAsync(name, cancellationToken).ConfigureAwait(false);
            return InitializeOneDriveStorageItem(oneDriveItem);
        }

        /// <summary>
        /// Gets the items from the current folder.
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a list of the subfolders and files in the current folder.</returns>
        public async Task<IReadOnlyList<OneDriveStorageItem>> GetItemsAsync(int top, OrderBy orderBy = OrderBy.None, string filter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            IDriveItemChildrenCollectionRequest oneDriveItemsRequest = ((IDriveItemRequestBuilder)RequestBuilder).CreateChildrenRequest(top, orderBy, filter);
            return await RequestOneDriveItemsAsync(oneDriveItemsRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets an index-based range of files and folders from the list of all files and subfolders in the current folder.
        /// </summary>
        /// <param name="startIndex">The zero-based index of the first item in the range to get</param>
        /// <param name="maxItemsToRetrieve">The maximum number of items to get</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a list of the subfolders and files in the current folder.</returns>
        public async Task<IReadOnlyList<OneDriveStorageItem>> GetItemsAsync(uint startIndex, uint maxItemsToRetrieve, CancellationToken cancellationToken = default(CancellationToken))
        {
            IDriveItemChildrenCollectionRequest oneDriveitemsRequest = null;
            var request = ((IDriveItemRequestBuilder)RequestBuilder).Children.Request();

            // skip is not working right now
            // oneDriveitemsRequest = request.Top((int)maxItemsToRetrieve).Skip((int)startIndex);
            int maxToRetrieve = (int)(maxItemsToRetrieve + startIndex);
            oneDriveitemsRequest = request.Top(maxToRetrieve);
            var tempo = await oneDriveitemsRequest.GetAsync(cancellationToken).ConfigureAwait(false);

            List<OneDriveStorageItem> items = new List<OneDriveStorageItem>();

            for (int i = (int)startIndex; i < maxToRetrieve && i < tempo.Count; i++)
            {
                items.Add(InitializeOneDriveStorageItem(tempo[i]));
            }

            return new OneDriveStorageItemsCollection(items);
        }

        /// <summary>
        /// Retrieve the next page of items
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The next collection of items or null if there are no more items (an item could be a folder or  file)</returns>
        public async Task<IReadOnlyList<OneDriveStorageItem>> NextItemsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_nextPageItemsRequest != null)
            {
                return await RequestOneDriveItemsAsync(_nextPageItemsRequest, cancellationToken);
            }

            // no more items
            return null;
        }

        /// <summary>
        /// Cancel the upload Session
        /// </summary>
        /// <returns>Task to support await of async call.</returns>
        public async Task CancelSessionAsync()
        {
            if (_uploadProvider != null)
            {
                await _uploadProvider.DeleteSession();
            }
        }

        /// <summary>
        /// Retrieve the next page of folders
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The next collection of folders or null if there are no more folders</returns>
        public async Task<List<OneDriveStorageFolder>> NextFoldersAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_nextPageFoldersRequest != null)
            {
                return await RequestOneDriveFoldersAsync(_nextPageFoldersRequest, cancellationToken);
            }

            // no more folders
            return null;
        }

        /// <summary>
        /// Retrieve the next page of files
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The next collection of files or null if there are no more files</returns>
        public async Task<List<OneDriveStorageFile>> NextFilesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_nextPageFilesRequest != null)
            {
                return await RequestOneDriveFilesAsync(_nextPageFilesRequest, cancellationToken);
            }

            // no more folders
            return null;
        }

        /// <summary>
        /// Gets the next expected ranges of the upload
        /// </summary>
        /// <remarks>Not available for OneDriveForBusiness</remarks>
        /// <returns>return next expected ranges, 0 if no more data</returns>
        public Task<long> GetUploadStatusAsync()
        {
            return Task.FromResult(0L);
        }

        /// <summary>
        /// Create children http request with specific options
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <returns>Returns the http request</returns>
        private IDriveItemChildrenCollectionRequest CreateChildrenRequest(int top, OrderBy orderBy = OrderBy.None, string filter = null)
        {
            IDriveItemChildrenCollectionRequest oneDriveitemsRequest = null;
            if (orderBy == OrderBy.None && string.IsNullOrEmpty(filter))
            {
                return ((IDriveItemRequestBuilder)RequestBuilder).Children.Request().Top(top);
            }

            if (orderBy == OrderBy.None)
            {
                return ((IDriveItemRequestBuilder)RequestBuilder).Children.Request().Top(top).Filter(filter);
            }

            string order = $"{orderBy} asc".ToLower();

            if (string.IsNullOrEmpty(filter))
            {
                oneDriveitemsRequest = ((IDriveItemRequestBuilder)RequestBuilder).Children.Request().Top(top).OrderBy(order);
            }
            else
            {
                oneDriveitemsRequest = ((IDriveItemRequestBuilder)RequestBuilder).Children.Request().Top(top).OrderBy(order).Filter(filter);
            }

            return oneDriveitemsRequest;
        }

        /// <summary>
        /// Request a list of DriveItem from oneDrive and create a GraphOneDriveStorageItemsCollection Collection
        /// </summary>
        /// <param name="request">Http request to execute</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns GraphOneDriveStorageItemsCollection that represents the specified files or folders</returns>
        private async Task<OneDriveStorageItemsCollection> RequestOneDriveItemsAsync(IDriveItemChildrenCollectionRequest request, CancellationToken cancellationToken)
        {
            var oneDriveItems = await request.GetAsync(cancellationToken).ConfigureAwait(false);

            _nextPageItemsRequest = oneDriveItems.NextPageRequest;

            List<OneDriveStorageItem> items = new List<OneDriveStorageItem>();

            foreach (var oneDriveItem in oneDriveItems)
            {
                items.Add(InitializeOneDriveStorageItem(oneDriveItem));
            }

            return new OneDriveStorageItemsCollection(items);
        }

        /// <summary>
        /// Request a list of Folder from oneDrive and create a MicrosoftGraphOneDriveItemCollection Collection
        /// </summary>
        /// <param name="request">Http request to execute</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns GraphOneDriveStorageItemsCollection that represents the specified files or folders</returns>
        private async Task<List<OneDriveStorageFolder>> RequestOneDriveFoldersAsync(IDriveItemChildrenCollectionRequest request, CancellationToken cancellationToken)
        {
            var oneDriveItems = await request.GetAsync(cancellationToken);

            _nextPageFoldersRequest = oneDriveItems.NextPageRequest;

            List<DriveItem> oneDriveFolders = QueryFolders(oneDriveItems);
            List<OneDriveStorageFolder> folders = new List<OneDriveStorageFolder>();

            foreach (var oneDriveFolder in oneDriveFolders)
            {
                folders.Add(InitializeOneDriveStorageFolder(oneDriveFolder));
            }

            return folders;
        }

        /// <summary>
        /// Filter folder
        /// </summary>
        /// <param name="itemFolders">Collection of DriveItem </param>
        /// <returns>Only the DriveItems folders </returns>
        private List<DriveItem> QueryFolders(IDriveItemChildrenCollectionPage itemFolders)
        {
            var query = from f in itemFolders.CurrentPage where f.Folder != null || f.SpecialFolder != null select f;
            return query.ToList<DriveItem>();
        }

        /// <summary>
        /// Request a list of items
        /// </summary>
        /// <param name="request">Http request to execute</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a list of IOneDriveStorageFile that represents the specified files</returns>
        private async Task<List<OneDriveStorageFile>> RequestOneDriveFilesAsync(IDriveItemChildrenCollectionRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var oneDriveItems = await request.GetAsync(cancellationToken);

            _nextPageFilesRequest = oneDriveItems.NextPageRequest;

            // TODO: The first items on the list are never a file
            List<DriveItem> oneDriveFiles = QueryFiles(oneDriveItems);

            // TODO: Algo to get only File
            List<OneDriveStorageFile> files = new List<OneDriveStorageFile>();

            foreach (var oneDriveFile in oneDriveFiles)
            {
                files.Add(InitializeOneDriveStorageFile(oneDriveFile));
            }

            return files;
        }

        /// <summary>
        /// Filter files
        /// </summary>
        /// <param name="itemFiles">Collection of DriveItem</param>
        /// <returns>A list of DriveItem</returns>
        private List<DriveItem> QueryFiles(IDriveItemChildrenCollectionPage itemFiles)
        {
            var query = from f in itemFiles.CurrentPage where !string.IsNullOrEmpty(f.Id) select f;
            return query.ToList<DriveItem>();
        }
    }
}
