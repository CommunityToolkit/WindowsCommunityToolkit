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
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// Interface IOneDriveStorageItem
    /// </summary>
    public interface IOneDriveStorageItem
    {
        /// <summary>
        /// Gets the thumbnail's item.
        /// </summary>
        IRandomAccessStream ThumbNail { get; }

        /// <summary>
        /// Gets the date and time that the current OneDrive item was created.
        /// </summary>
        DateTimeOffset? DateCreated { get;  }

        /// <summary>
        /// Gets the date and time that the current OneDrive item was last modified.
        /// </summary>
        DateTimeOffset? DateModified { get; }

        /// <summary>
        /// Gets the user-friendly name of the current folder.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets The user-friendly type of the item.
        /// </summary>
        string DisplayType { get; }

        /// <summary>
        /// Gets the id of the current OneDrive Item.
        /// </summary>
        string FolderRelativeId { get; }

        /// <summary>
        /// Gets the name of the current OneDrive Item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the path of the current item if the path is available
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets an instance of a DriveItem
        /// </summary>
        DriveItem OneDriveItem { get; }

        /// <summary>
        /// Check if the item is a file
        /// </summary>
        /// <returns>Return true if it's a file</returns>
        bool IsFile();

        /// <summary>
        /// Check if the item is a OneNote focument
        /// </summary>
        /// <returns>Return true if it's a OneNote document</returns>
        bool IsOneNote();

        /// <summary>
        /// Check if the item is a folder
        /// </summary>
        /// <returns>Return true if it's a folder</returns>
        bool IsFolder();

        /// <summary>
        /// Store a reference to an instance of the underlying data provider.
        /// </summary>
        IBaseClient Provider { get; set; }

        /// <summary>
        /// Store a reference to an instance of current request builder
        /// </summary>
        IBaseRequestBuilder RequestBuilder { get; }

        /// <summary>
        /// Retrieves a thumbnail set for the file
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, return a thumbnail set, or null if no thumbnail are available</returns>
        Task<OneDriveThumbnailSet> GetThumbnailSetAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieves a thumbnail image for the file
        /// </summary>
        /// <param name="optionSize"> A value from the enumeration that specifies the size of the image to retrieve. Small ,Medium, Large</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, return a stream containing the thumbnail, or null if no thumbnail are available</returns>
        Task<IRandomAccessStream> GetThumbnailAsync(OneDriveEnums.ThumbnailSize optionSize = OneDriveEnums.ThumbnailSize.Small, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///  Determines whether the current IStorageItem matches the specified StorageItemTypes value.
        /// </summary>
        /// <param name="type">The value to match against.</param>
        /// <returns>True if the IStorageItem matches the specified value; otherwise false.</returns>
        bool IsOfType(StorageItemTypes type);

        /// <summary>
        /// Copy the current item to the specified folder and renames the item according to the desired name.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the item is moved.</param>
        /// <param name="desiredNewName">The desired name of the item after it is moved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>return success or failure</returns>
        Task<bool> CopyAsync(IOneDriveStorageFolder destinationFolder, string desiredNewName = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Renames the current folder.
        /// </summary>
        /// <param name="desiredName">The desired, new name for the current folder.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns an OneDriveStorageItem that represents the specified folder.</returns>
        Task<IOneDriveStorageItem> RenameAsync(string desiredName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Moves the current item to the specified folder and renames the item according to the desired name.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the item is moved.</param>
        /// <param name="desiredNewName">The desired name of the item after it is moved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>return success or failure</returns>
        Task<bool> MoveAsync(IOneDriveStorageFolder destinationFolder, string desiredNewName = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes the current item.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns> No object or value is returned by this method when it completes.</returns>
        Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
