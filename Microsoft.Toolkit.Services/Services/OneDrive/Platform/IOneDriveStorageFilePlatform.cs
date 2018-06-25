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
    public interface IOneDriveStorageFilePlatform
    {
        /// <summary>
        /// Opens a random-access stream over the specified file.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, it returns an IRandomAccessStream that contains the
        ///     requested random-access stream.</returns>
        Task<object> OpenAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates a background download for the current file
        /// </summary>
        /// <param name="destinationFile">Storage file to download to.</param>
        /// <param name="completionGroup">Completion Group.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>The created DownloadOperation.</returns>
        Task<object> CreateBackgroundDownloadAsync(object destinationFile, object completionGroup = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
