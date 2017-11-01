using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services.OneDrive.Platform
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
