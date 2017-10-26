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

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// Interface IOneDriveStorageFolder
    /// </summary>
    public interface IOneDriveStorageFolder : IOneDriveStorageItem
    {
        /// <summary>
        /// Gets or sets a value indicating whether if a large file upload is completed
        /// </summary>
        bool IsUploadCompleted { get; set; }

        /// <summary>
        /// Renames the current folder.
        /// </summary>
        /// <param name="desiredName">The desired, new name for the current folder.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns an IOneDriveStorageFolder that represents the specified folder.</returns>
        new Task<IOneDriveStorageFolder> RenameAsync(string desiredName, CancellationToken cancellationToken = default(CancellationToken));

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
        Task<IOneDriveStorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options, IRandomAccessStream content = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates a new subfolder in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new subfolder to create in the current folder.</param>
        /// <param name="options">>One of the enumeration values that determines how to handle the collision if a file with the specified desiredName already exists in the current folder.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, it returns a IOneDriveStorageFolder that represents the new subfolder.</returns>
        Task<IOneDriveStorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the file with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the file to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a IOneDriveStorageFile that represents the specified file.</returns>
        Task<IOneDriveStorageFile> GetFileAsync(string name, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the files in the current folder.
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a list of the files in the current folder.</returns>
        Task<List<IOneDriveStorageFile>> GetFilesAsync(int top = 20, OneDriveEnums.OrderBy orderBy = OneDriveEnums.OrderBy.None, string filter = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the folder with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the folder to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a IOneDriveStorageFolder that represents the specified file.</returns>
        Task<IOneDriveStorageFolder> GetFolderAsync(string name, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the subfolders in the current folder.
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a list of the subfolders in the current folder.</returns>
        Task<List<IOneDriveStorageFolder>> GetFoldersAsync(int top = 100, OneDriveEnums.OrderBy orderBy = OneDriveEnums.OrderBy.None, string filter = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the item with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the folder to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a IOneDriveStorageItem that represents the specified file.</returns>
        Task<IOneDriveStorageItem> GetItemAsync(string name, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the items from the current folder.
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a list of the subfolders and files in the current folder.</returns>
        Task<IReadOnlyList<IOneDriveStorageItem>> GetItemsAsync(int top, OneDriveEnums.OrderBy orderBy = OneDriveEnums.OrderBy.None, string filter = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the items from the current folder.
        /// </summary>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <remarks>don't use awaitable</remarks>
        /// <returns>When this method completes successfully, it returns a list of the subfolders and files in the current folder.</returns>
        IncrementalLoadingCollection<OneDriveRequestSource<IOneDriveStorageItem>, IOneDriveStorageItem> GetItemsAsync(OneDriveEnums.OrderBy orderBy = OneDriveEnums.OrderBy.None);

        /// <summary>
        /// Gets an index-based range of files and folders from the list of all files and subfolders in the current folder.
        /// </summary>
        /// <param name="startIndex">The zero-based index of the first item in the range to get</param>
        /// <param name="maxItemsToRetrieve">The maximum number of items to get</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a list of the subfolders and files in the current folder.</returns>
        Task<IReadOnlyList<IOneDriveStorageItem>> GetItemsAsync(uint startIndex, uint maxItemsToRetrieve, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve the next page of items
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The next collection of items or null if there are no more items (an item could be a folder or  file)</returns>
        Task<IReadOnlyList<IOneDriveStorageItem>> NextItemsAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Cancel the upload Session
        /// </summary>
        /// <returns>Task to support await of async call.</returns>
        Task CancelSessionAsync();

        /// <summary>
        /// Retrieve the next page of folders
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The next collection of folders or null if there are no more folders</returns>
        Task<List<IOneDriveStorageFolder>> NextFoldersAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve the next page of files
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>The next collection of files or null if there are no more files</returns>
        Task<List<IOneDriveStorageFile>> NextFilesAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the next expected ranges of the upload
        /// </summary>
        /// <remarks>Not available for OneDriveForBusiness</remarks>
        /// <returns>return next expected ranges, 0 if no more data</returns>
        Task<long> GetUploadStatusAsync();

        /// <summary>
        /// Creates a new large file in the current folder.
        /// Use this method when your file is larger than
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <param name="content">The data's stream to push into the file</param>
        /// <param name="options">One of the enumeration values that determines how to handle the collision if a file with the specified desiredName already exists in the current folder.</param>
        /// <param name="maxChunkSize">Max chunk size must be a multiple of 320 KiB (ie: 320*1024)</param>
        /// <returns>When this method completes, it returns a IOneDriveStorageFile that represents the new file.</returns>
        Task<IOneDriveStorageFile> UploadFileAsync(string desiredName, IRandomAccessStream content, CreationCollisionOption options = CreationCollisionOption.FailIfExists, int maxChunkSize = -1);
    }
}