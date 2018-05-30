// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace Microsoft.Toolkit.Services.OneDrive.Uwp
{
    /// <summary>
    /// Platform implementation of background download service.
    /// </summary>
    public class OneDriveServicePlatform : IOneDriveServicePlatform
    {
        private Toolkit.Services.OneDrive.OneDriveService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveServicePlatform"/> class.
        /// </summary>
        /// <param name="service">Instance of OneDriveService</param>
        public OneDriveServicePlatform(Toolkit.Services.OneDrive.OneDriveService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a background download for the current item.
        /// </summary>
        /// <param name="oneDriveId">OneDrive Id.</param>
        /// <param name="destinationFile">Destination storage file.</param>
        /// <param name="completionGroup">Completion group.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Storage file.</returns>
        public async Task<object> CreateBackgroundDownloadForItemAsync(string oneDriveId, object destinationFile, object completionGroup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreateBackgroundDownloadForItemInternalAsync(oneDriveId, destinationFile as StorageFile, completionGroup as BackgroundTransferCompletionGroup, cancellationToken);
        }

        /// <summary>
        /// Creates a background download for given OneDriveId
        /// </summary>
        /// <param name="oneDriveId">OneDrive item's Id</param>
        /// <param name="destinationFile">A <see cref="StorageFile"/> to which content will be downloaded</param>
        /// <param name="completionGroup">The <see cref="BackgroundTransferCompletionGroup"/> to which should <see cref="BackgroundDownloader"/> refer to</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request</param>
        /// <returns>The created <see cref="DownloadOperation"/></returns>
        private async Task<DownloadOperation> CreateBackgroundDownloadForItemInternalAsync(string oneDriveId, StorageFile destinationFile, BackgroundTransferCompletionGroup completionGroup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(oneDriveId))
            {
                throw new ArgumentNullException(nameof(oneDriveId));
            }

            if (destinationFile == null)
            {
                throw new ArgumentNullException(nameof(destinationFile));
            }

            return await Task.Run(
                async () =>
                {
                    var requestMessage = _service.Provider.GraphProvider.Drive.Items[oneDriveId].Content.Request().GetHttpRequestMessage();
                    await _service.Provider.GraphProvider.AuthenticationProvider.AuthenticateRequestAsync(requestMessage).AsAsyncAction().AsTask(cancellationToken);
                    var downloader = completionGroup == null ? new BackgroundDownloader() : new BackgroundDownloader(completionGroup);
                    foreach (var item in requestMessage.Headers)
                    {
                        downloader.SetRequestHeader(item.Key, item.Value.First());
                    }
                    return downloader.CreateDownload(requestMessage.RequestUri, destinationFile);
                }, cancellationToken);
        }
    }
}
