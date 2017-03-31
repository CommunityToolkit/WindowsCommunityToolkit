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
using Microsoft.OneDrive.Sdk;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    ///  Class representing a OneDrive file
    /// </summary>
    public class OneDriveStorageFile : OneDriveStorageItem
    {
        private string _fileType;

        /// <summary>
        /// Gets OneDrive file type
        /// </summary>
        public string FileType
        {
            get
            {
                return _fileType;
            }
        }

        /// <summary>
        /// Parse the extension of the file from its name
        /// </summary>
        /// <param name="name">name of the file to parse</param>
        private void ParseFileType(string name)
        {
            var index = name.LastIndexOf('.');

            // This is a OneNote File
            if (index == -1)
            {
                return;
            }

            var length = name.Length;
            var s = length - index;
            _fileType = name.Substring(index, s);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveStorageFile"/> class.
        /// <para>Permissions : Have full access to user files and files shared with user</para>
        /// </summary>
        /// <param name="oneDriveProvider">Instance of OneDriveClient class</param>
        /// <param name="requestBuilder">Http request builder.</param>
        /// <param name="oneDriveItem">OneDrive's item</param>
        public OneDriveStorageFile(IOneDriveClient oneDriveProvider, IItemRequestBuilder requestBuilder, Item oneDriveItem)
          : base(oneDriveProvider, requestBuilder, oneDriveItem)
        {
            ParseFileType(oneDriveItem.Name);
        }

        /// <summary>
        /// Renames the current file.
        /// </summary>
        /// <param name="desiredName">The desired, new name for the current folder.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns an OneDriveStorageFile that represents the specified folder.</returns>
        public async new Task<OneDriveStorageFile> RenameAsync(string desiredName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var renameItem = await base.RenameAsync(desiredName, cancellationToken);
            return InitializeOneDriveStorageFile(renameItem.OneDriveItem);
        }

        /// <summary>
        /// Creates a background download for the current file
        /// </summary>
        /// <param name="destinationFile">A <see cref="StorageFile"/> to which content will be downloaded</param>
        /// <param name="completionGroup">The <see cref="BackgroundTransferCompletionGroup"/> to which should <see cref="BackgroundDownloader"/> reffer to</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request</param>
        /// <returns>The created <see cref="DownloadOperation"/></returns>
        public async Task<DownloadOperation> CreateBackgroundDownloadAsync(StorageFile destinationFile, BackgroundTransferCompletionGroup completionGroup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (destinationFile == null)
            {
                throw new ArgumentNullException(nameof(destinationFile));
            }

            return await Task.Run(
                async () =>
                {
                    var requestMessage = Provider.Drive.Items[OneDriveItem.Id].Content.Request().GetHttpRequestMessage();
                    await Provider.AuthenticationProvider.AuthenticateRequestAsync(requestMessage).AsAsyncAction().AsTask(cancellationToken);
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
        public async Task<IRandomAccessStream> OpenAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IRandomAccessStream contentStream = null;
            try
            {
                System.IO.Stream content = null;
                content = await RequestBuilder.Content.Request().GetAsync(cancellationToken).ConfigureAwait(false);
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
