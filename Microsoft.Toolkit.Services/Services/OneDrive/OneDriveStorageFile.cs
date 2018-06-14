// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Services.OneDrive
{
    internal delegate void InternalEventHandler();

    /// <summary>
    ///  Class representing a OneDrive file
    /// </summary>
    public class OneDriveStorageFile : OneDriveStorageItem
    {
        /// <summary>
        /// Gets or sets platform-specific implementation of platform services.
        /// </summary>
        public IOneDriveStorageFilePlatform StorageFilePlatformService { get; set; }

        /// <summary>
        /// Gets OneDrive file type
        /// </summary>
        public string FileType { get; private set; }

        /// <summary>
        /// Gets the smallest available thumbnail for the object.  This will be null until you call GetThumbnailAsync().
        /// </summary>
        public string Thumbnail { get; private set; }

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
            FileType = name.Substring(index, s);
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

        /// <summary>
        /// Acquires the smallest available thumbnail url as string for the OneDrive file item, asyncrounously, and applies it to the Thumbnail property.
        /// </summary>
        /// <returns>awaitable task</returns>
        public async Task UpdateThumbnailPropertyAsync()
        {
            var newValue = Thumbnail;

            try
            {
                var set = await GetThumbnailSetAsync();
                if (set != null)
                {
                    newValue = set.Small ?? set.Medium ?? set.Large;
                }
            }
            catch (Exception)
            {
            }

            Thumbnail = newValue;
        }
    }
}
