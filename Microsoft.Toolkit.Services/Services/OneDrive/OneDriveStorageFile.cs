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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    ///  Class representing a OneDrive file
    /// </summary>
    public class OneDriveStorageFile : OneDriveStorageItem
    {
        /// <summary>
        /// Gets or sets platform-specific implementation of platform services.
        /// </summary>
        public IOneDriveStorageFilePlatform StorageFilePlatformService { get; set; }

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

        private string _thumbnail;

        /// <summary>
        /// Gets the smallest available thumbnail for the object.  This will be null until you call GetThumbnailAsync().
        /// </summary>
        public string Thumbnail
        {
            get
            {
                return _thumbnail;
            }
        }

        /// <summary>
        /// Loads the thumbnail URL for the item asynchronously, attempting to produce a value for Thumbnail property.
        /// </summary>
        public async void GetThumbnailAsync()
        {
            var newValue = _thumbnail;
            var set = await GetThumbnailSetAsync();
            if (set != null)
            {
                newValue = set.Small ?? set.Medium ?? set.Large;
            }

            SetValue(ref _thumbnail, newValue, nameof(Thumbnail));
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
        /// <param name="graphProvider">Instance of Graph Client class</param>
        /// <param name="requestBuilder">Http request builder.</param>
        /// <param name="oneDriveItem">OneDrive's item</param>
        public OneDriveStorageFile(IBaseClient graphProvider, IBaseRequestBuilder requestBuilder, DriveItem oneDriveItem)
          : base(graphProvider, requestBuilder, oneDriveItem)
        {
            StorageFilePlatformService = OneDriveService.ServicePlatformInitializer.CreateOneDriveStorageFilePlatformInstance(OneDriveService.Instance, this);
            ParseFileType(oneDriveItem.Name);
        }

        /// <summary>
        /// Renames the current file.
        /// </summary>
        /// <param name="desiredName">The desired, new name for the current folder.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns an OneDriveStorageFile that represents the specified folder.</returns>
        public new async Task<OneDriveStorageFile> RenameAsync(string desiredName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var renameItem = await base.RenameAsync(desiredName, cancellationToken);
            return InitializeOneDriveStorageFile(renameItem.OneDriveItem);
        }
    }
}
