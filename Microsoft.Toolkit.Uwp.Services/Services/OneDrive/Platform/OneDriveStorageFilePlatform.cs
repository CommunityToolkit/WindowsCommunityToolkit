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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.OneDrive.Platform;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive.Platform
{
    /// <summary>
    /// Platform implementation to handle file and download operations.
    /// </summary>
    public class OneDriveStorageFilePlatform : IOneDriveStorageFilePlatform
    {
        private Toolkit.Services.OneDrive.OneDriveService _service;
        private Toolkit.Services.OneDrive.OneDriveStorageFile _oneDriveStorageFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveStorageFilePlatform"/> class.
        /// </summary>
        /// <param name="service">Instance of OneDriveService</param>
        /// <param name="oneDriveStorageFile">Instance of OneDriveStorageFile</param>
        public OneDriveStorageFilePlatform(
            Toolkit.Services.OneDrive.OneDriveService service,
            Toolkit.Services.OneDrive.OneDriveStorageFile oneDriveStorageFile)
        {
            _service = service;
            _oneDriveStorageFile = oneDriveStorageFile;
        }

        /// <summary>
        /// Creates a background download for the current file
        /// </summary>
        /// <param name="destinationFile">Storage file to download to.</param>
        /// <param name="completionGroup">Completion Group.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>The created DownloadOperation.</returns>
        public async Task<object> CreateBackgroundDownloadAsync(object destinationFile, object completionGroup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreateBackgroundDownloadInternalAsync(destinationFile as StorageFile, completionGroup as BackgroundTransferCompletionGroup, cancellationToken);
        }

        /// <summary>
        /// Opens a random-access stream over the specified file.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, it returns an IRandomAccessStream that contains the
        ///     requested random-access stream.</returns>
        public async Task<object> OpenAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await OpenInternalAsync(cancellationToken);
        }

        /// <summary>
        /// Creates a background download for the current file
        /// </summary>
        /// <param name="destinationFile">A <see cref="StorageFile"/> to which content will be downloaded</param>
        /// <param name="completionGroup">The <see cref="BackgroundTransferCompletionGroup"/> to which should <see cref="BackgroundDownloader"/> reffer to</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request</param>
        /// <returns>The created <see cref="DownloadOperation"/></returns>
        private async Task<DownloadOperation> CreateBackgroundDownloadInternalAsync(StorageFile destinationFile, BackgroundTransferCompletionGroup completionGroup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (destinationFile == null)
            {
                throw new ArgumentNullException(nameof(destinationFile));
            }

            return await Task.Run(
                async () =>
                {
                    var requestMessage = ((IGraphServiceClient)_service.Provider.GraphProvider).Drive.Items[_oneDriveStorageFile.OneDriveItem.Id].Content.Request().GetHttpRequestMessage();
                    await _service.Provider.GraphProvider.AuthenticationProvider.AuthenticateRequestAsync(requestMessage).AsAsyncAction().AsTask(cancellationToken);
                    var downloader = completionGroup == null ? new BackgroundDownloader() : new BackgroundDownloader(completionGroup);
                    foreach (var item in requestMessage.Headers)
                    {
                        downloader.SetRequestHeader(item.Key, item.Value.First());
                    }
                    return downloader.CreateDownload(requestMessage.RequestUri, destinationFile);
                }, cancellationToken);
        }

        /// <summary>
        /// Opens a random-access stream over the specified file.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, it returns an IRandomAccessStream that contains the
        ///     requested random-access stream.</returns>
        private async Task<IRandomAccessStream> OpenInternalAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IRandomAccessStream contentStream = null;
            try
            {
                System.IO.Stream content = null;
                content = await ((IDriveItemRequestBuilder)_oneDriveStorageFile.RequestBuilder).Content.Request().GetAsync(cancellationToken).ConfigureAwait(false);
                if (content != null)
                {
                    contentStream = content.AsRandomAccessStream();
                }
            }
            catch (Microsoft.Graph.ServiceException ex)
            {
                // Swallow error in case of no content found
                if (!ex.Error.Code.Equals("ErrorItemNotFound"))
                {
                    throw;
                }
            }

            return contentStream;
        }
    }
}
