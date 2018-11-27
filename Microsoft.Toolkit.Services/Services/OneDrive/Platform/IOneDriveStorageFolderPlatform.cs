// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    /// Platform abstraction.
    /// </summary>
    public interface IOneDriveStorageFolderPlatform
    {
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
        Task<OneDriveStorageFile> CreateFileAsync(string desiredName, object options = null, object content = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates a new subfolder in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new subfolder to create in the current folder.</param>
        /// <param name="options">>One of the enumeration values that determines how to handle the collision if a file with the specified desiredName already exists in the current folder.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, it returns a IOneDriveStorageFolder that represents the new subfolder.</returns>
        Task<OneDriveStorageFolder> CreateFolderAsync(string desiredName, object options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates a new large file in the current folder.
        /// Use this method when your file is larger than
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <param name="content">The data's stream to push into the file</param>
        /// <param name="options">One of the enumeration values that determines how to handle the collision if a file with the specified desiredName already exists in the current folder.</param>
        /// <param name="maxChunkSize">Max chunk size must be a multiple of 320 KiB (ie: 320*1024)</param>
        /// <returns>When this method completes, it returns a IOneDriveStorageFile that represents the new file.</returns>
        Task<OneDriveStorageFile> UploadFileAsync(string desiredName, object content, object options, int maxChunkSize = -1);
    }
}
