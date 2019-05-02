// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Windows.Storage;
using Windows.Storage.Streams;
using static Microsoft.Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Services.OneDrive.Uwp
{
    /// <summary>
    /// Platform implementation of file operations.
    /// </summary>
    public class OneDriveStorageItemPlatform : IOneDriveStorageItemPlatform
    {
        private IRandomAccessStream _thumbNail;

        /// <summary>
        /// Gets thumbnail stream.
        /// </summary>
        public object ThumbNail
        {
            get
            {
                return _thumbNail;
            }
        }

        private Toolkit.Services.OneDrive.OneDriveService _service;
        private Toolkit.Services.OneDrive.OneDriveStorageItem _oneDriveStorageItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveStorageItemPlatform"/> class.
        /// </summary>
        /// <param name="service">Instance of OneDriveService</param>
        /// <param name="oneDriveStorageItem">Instance of OneDriveStorageItem</param>
        public OneDriveStorageItemPlatform(
            Toolkit.Services.OneDrive.OneDriveService service,
            Toolkit.Services.OneDrive.OneDriveStorageItem oneDriveStorageItem)
        {
            _service = service;
            _oneDriveStorageItem = oneDriveStorageItem;
        }

        /// <summary>
        /// Retrieves a thumbnail image for the file
        /// </summary>
        /// <param name="optionSize"> A value from the enumeration that specifies the size of the image to retrieve. Small ,Medium, Large</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, return a stream containing the thumbnail, or null if no thumbnail are available</returns>
        public async Task<object> GetThumbnailAsync(MicrosoftGraphEnums.ThumbnailSize optionSize = MicrosoftGraphEnums.ThumbnailSize.Small, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await GetThumbnailInternalAsync(optionSize, cancellationToken)) as IRandomAccessStream;
        }

        /// <summary>
        ///  Determines whether the current IStorageItem matches the specified StorageItemTypes value.
        /// </summary>
        /// <param name="type">The value to match against.</param>
        /// <returns>True if the IStorageItem matches the specified value; otherwise false.</returns>
        public bool IsOfType(object type)
        {
            return IsOfTypeInternal((StorageItemTypes)type);
        }

        private async Task<IRandomAccessStream> GetThumbnailInternalAsync(ThumbnailSize optionSize = ThumbnailSize.Small, CancellationToken cancellationToken = default(CancellationToken))
        {
            var thumbnailStream = await Toolkit.Services.OneDrive.OneDriveItemExtension.GetThumbnailAsync((IDriveItemRequestBuilder)_oneDriveStorageItem.RequestBuilder, _service.Provider.GraphProvider, cancellationToken, optionSize);
            if (thumbnailStream == null)
            {
                return null;
            }

            _thumbNail = thumbnailStream.AsRandomAccessStream();
            return _thumbNail;
        }

        private bool IsOfTypeInternal(StorageItemTypes type)
        {
            if (_oneDriveStorageItem.IsFolder() && type == StorageItemTypes.Folder)
            {
                return true;
            }

            if (_oneDriveStorageItem.IsFile() && type == StorageItemTypes.File)
            {
                return true;
            }

            if (_oneDriveStorageItem.IsOneNote() && type == StorageItemTypes.None)
            {
                return true;
            }

            return false;
        }
    }
}
