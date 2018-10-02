// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Uwp;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using static Microsoft.Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Services.OneDrive.Uwp
{
    /// <summary>
    /// Platform implementation of file operations.
    /// </summary>
    public class OneDriveStorageFolderPlatform : IOneDriveStorageFolderPlatform
    {
        private Toolkit.Services.OneDrive.OneDriveService _service;
        private OneDriveStorageFolder _oneDriveStorageFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveStorageFolderPlatform"/> class.
        /// </summary>
        /// <param name="service">Instance of OneDriveService</param>
        /// <param name="oneDriveStorageFolder">Instance of OneDriveStorageFolder</param>
        public OneDriveStorageFolderPlatform(
            Toolkit.Services.OneDrive.OneDriveService service,
            Toolkit.Services.OneDrive.OneDriveStorageFolder oneDriveStorageFolder)
        {
            _service = service;
            _oneDriveStorageFolder = oneDriveStorageFolder;
        }

        /// <summary>
        /// Creates a new file in the current folder. This method also specifies what to
        /// do if a file with the same name already exists in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <param name="options">One of the enumeration values that determines how to handle the collision if a file with the specified desiredName already exists in the current folder.</param>
        /// <param name="content">The data's stream to push into the file</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <remarks>With OneDrive Consumer, the content could not be null</remarks>
        /// One of the enumeration values that determines how to handle the collision if
        /// a file with the specified desiredNewName already exists in the destination folder.
        /// Default : Fail
        /// <returns>When this method completes, it returns a IOneDriveStorageFile that represents the new file.</returns>
        public async Task<OneDriveStorageFile> CreateFileAsync(string desiredName, object options, object content = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (options == null)
            {
                options = CreationCollisionOption.FailIfExists;
            }

            return await CreateFileInternalAsync(desiredName, (CreationCollisionOption)options, content as IRandomAccessStream, cancellationToken);
        }

        /// <summary>
        /// Creates a new subfolder in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new subfolder to create in the current folder.</param>
        /// <param name="options">>One of the enumeration values that determines how to handle the collision if a file with the specified desiredName already exists in the current folder.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, it returns a IOneDriveStorageFolder that represents the new subfolder.</returns>
        public async Task<OneDriveStorageFolder> CreateFolderAsync(string desiredName, object options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (options == null)
            {
                options = CreationCollisionOption.FailIfExists;
            }

            return await CreateFolderInternalAsync(desiredName, (CreationCollisionOption)options, cancellationToken);
        }

        private async Task<OneDriveStorageFile> CreateFileInternalAsync(string desiredName, CreationCollisionOption options = CreationCollisionOption.FailIfExists, IRandomAccessStream content = null, CancellationToken cancellationToken = default(CancellationToken))
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
                    throw new ServiceException(new Error { Message = "The file size cannot exceed 4MB, use UploadFileAsync instead ", Code = "MaxSizeExceeded", ThrowSite = "Windows Community Toolkit" });
                }

                streamContent = content.AsStreamForRead();
            }

            var childrenRequest = ((IDriveItemRequestBuilder)_oneDriveStorageFolder.RequestBuilder).Children.Request();
            string requestUri = $"{childrenRequest.RequestUrl}/{desiredName}/content?@name.conflictBehavior={OneDriveHelper.TransformCollisionOptionToConflictBehavior(options.ToString())}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, requestUri)
            {
                Content = new StreamContent(streamContent)
            };
            var createdFile = await ((IGraphServiceClient)_service.Provider.GraphProvider).SendAuthenticatedRequestAsync(request, cancellationToken);
            return _oneDriveStorageFolder.InitializeOneDriveStorageFile(createdFile);
        }

        private async Task<OneDriveStorageFolder> CreateFolderInternalAsync(string desiredName, CreationCollisionOption options = CreationCollisionOption.FailIfExists, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(desiredName))
            {
                throw new ArgumentNullException(nameof(desiredName));
            }

            var childrenRequest = ((IDriveItemRequestBuilder)_oneDriveStorageFolder.RequestBuilder).Children.Request();
            var requestUri = childrenRequest.RequestUrl;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            DriveItem item = new DriveItem { Name = desiredName, Folder = new Graph.Folder { } };
            item.AdditionalData = new Dictionary<string, object>();
            item.AdditionalData.Add(new KeyValuePair<string, object>("@microsoft.graph.conflictBehavior", OneDriveHelper.TransformCollisionOptionToConflictBehavior(options.ToString())));

            var jsonOptions = JsonConvert.SerializeObject(item);
            request.Content = new StringContent(jsonOptions, Encoding.UTF8, "application/json");

            var createdFolder = await ((IGraphServiceClient)_service.Provider.GraphProvider).SendAuthenticatedRequestAsync(request, cancellationToken).ConfigureAwait(false);
            return _oneDriveStorageFolder.InitializeOneDriveStorageFolder(createdFolder);
        }

        /// <summary>
        /// Creates a new large file in the current folder.
        /// Use this method when your file is larger than
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <param name="content">The data's stream to push into the file</param>
        /// <param name="options">One of the enumeration values that determines how to handle the collision if a file with the specified desiredName already exists in the current folder.</param>
        /// <param name="maxChunkSize">Max chunk size must be a multiple of 320 KiB (ie: 320*1024)</param>
        /// <returns>When this method completes, it returns a IOneDriveStorageFile that represents the new file.</returns>
        public async Task<OneDriveStorageFile> UploadFileAsync(string desiredName, object content, object options, int maxChunkSize = -1)
        {
            return await UploadFileInternalAsync(desiredName, content as IRandomAccessStream, (CreationCollisionOption)options, maxChunkSize);
        }

        private async Task<OneDriveStorageFile> UploadFileInternalAsync(string desiredName, IRandomAccessStream content, CreationCollisionOption options = CreationCollisionOption.FailIfExists, int maxChunkSize = -1)
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

            var uploadSessionUri = $"{_service.Provider.GraphProvider.BaseUrl}/drive/items/{_oneDriveStorageFolder.OneDriveItem.Id}:/{desiredName}:/createUploadSession";

            var conflictBehavior = new OneDriveItemConflictBehavior { Item = new OneDriveConflictItem { ConflictBehavior = OneDriveHelper.TransformCollisionOptionToConflictBehavior(options.ToString()) } };

            var jsonConflictBehavior = JsonConvert.SerializeObject(conflictBehavior);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uploadSessionUri)
            {
                Content = new StringContent(jsonConflictBehavior, Encoding.UTF8, "application/json")
            };
            await _service.Provider.GraphProvider.AuthenticationProvider.AuthenticateRequestAsync(request).ConfigureAwait(false);

            var response = await _service.Provider.GraphProvider.HttpProvider.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new ServiceException(new Error { Message = "Could not create an UploadSession", Code = "NoUploadSession", ThrowSite = "Windows Community Toolkit" });
            }

            _oneDriveStorageFolder.IsUploadCompleted = false;
            var jsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            _oneDriveStorageFolder.UploadSession = JsonConvert.DeserializeObject<UploadSession>(jsonData);

            var streamToUpload = content.AsStreamForRead();
            _oneDriveStorageFolder.UploadProvider = new ChunkedUploadProvider(_oneDriveStorageFolder.UploadSession, _service.Provider.GraphProvider, streamToUpload, maxChunkSize);

            var uploadedItem = await _oneDriveStorageFolder.UploadProvider.UploadAsync().ConfigureAwait(false);
            _oneDriveStorageFolder.IsUploadCompleted = true;
            return _oneDriveStorageFolder.InitializeOneDriveStorageFile(uploadedItem);
        }

        /// <summary>
        /// Gets the items from the current folder.
        /// </summary>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <remarks>don't use awaitable</remarks>
        /// <returns>When this method completes successfully, it returns a list of the subfolders and files in the current folder.</returns>
        public IncrementalLoadingCollection<OneDriveRequestSource<OneDriveStorageItem>, OneDriveStorageItem> GetItemsAsync(OrderBy orderBy = OrderBy.None)
        {
            var requestSource = new OneDriveRequestSource<OneDriveStorageItem>(_service.Provider.GraphProvider, _oneDriveStorageFolder.RequestBuilder, orderBy, null);
            return new IncrementalLoadingCollection<OneDriveRequestSource<OneDriveStorageItem>, OneDriveStorageItem>(requestSource);
        }
    }
}
