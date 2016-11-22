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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class representing a OneDrive file
    /// </summary>
    public class OneDriveStorageFile : OneDriveStorageItem
    {
        /// <summary>
        /// Instance of Http request builder for the current file
        /// </summary>
        private IDriveItemRequestBuilder _builder;

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
        /// <param name="graphProvider">Instance of GraphClientService class</param>
        /// <param name="builder">Http request builder.</param>
        /// <param name="driveItem">OneDrive's item</param>
        public OneDriveStorageFile(GraphServiceClient graphProvider, IDriveItemRequestBuilder builder, DriveItem driveItem)
            : base(graphProvider, builder, driveItem)
        {
            _builder = builder;
            ParseFileType(driveItem.Name);
        }

        /// <summary>
        /// Opens a random-access stream over the specified file.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, it returns an IRandomAccessStream that contains the
        ///     requested random-access stream.</returns>
        public async Task<IRandomAccessStream> OpenAsync(CancellationToken cancellationToken)
        {
            IRandomAccessStream contentStream = null;
            try
            {
                System.IO.Stream content = null;
                content = await Builder.OpenAsync(cancellationToken);
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

        /// <summary>
        /// Opens a random-access stream over the specified file.
        /// </summary>
        /// <returns>When this method completes, it returns an IRandomAccessStream that contains the
        ///     requested random-access stream.</returns>
        public Task<IRandomAccessStream> OpenAsync()
        {
            return OpenAsync(CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously writes a Stream to the current file
        /// </summary>
        /// <param name="content">The stream to write data from.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task WriteAsync(IRandomAccessStream content, CancellationToken cancellationToken)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (content.Size > OneDriveConstants.SimpleUploadMaxSize)
            {
                throw new ServiceException(new Error { Message = "The file size cannot exceed 4MB, use UploadFileAsync instead ", Code = "MaxSizeExceeded", ThrowSite = "UWP Community Toolkit" });
            }

            await Builder.WriteAsync(content, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a Stream to the current file
        /// </summary>
        /// <param name="content">The stream to write data from.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task WriteAsync(IRandomAccessStream content)
        {
            await WriteAsync(content, CancellationToken.None);
        }

        /// <summary>
        /// Renames the current file.
        /// </summary>
        /// <param name="desiredName">The desired, new name for the current file.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns an MicrosoftGraphOneDriveFile that represents the specified file.</returns>
        public async Task<OneDriveStorageFile> RenameAsync(string desiredName, CancellationToken cancellationToken)
        {
            var itemRenamed = await Builder.RenameAsync(OneDriveItem, desiredName, cancellationToken);
            return InitializeOneDriveFile(itemRenamed);
        }

        /// <summary>
        /// Renames the current file.
        /// </summary>
        /// <param name="desiredName">The desired, new name for the current file.</param>
        /// <returns>When this method completes successfully, it returns an MicrosoftGraphOneDriveFile that represents the specified file.</returns>
        public Task<OneDriveStorageFile> RenameAsync(string desiredName)
        {
            return RenameAsync(desiredName, CancellationToken.None);
        }
    }
}
