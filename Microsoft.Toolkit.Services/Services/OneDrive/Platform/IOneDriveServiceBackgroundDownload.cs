using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services.OneDrive.Platform
{
    /// <summary>
    /// Background service interface.
    /// </summary>
    public interface IOneDriveServiceBackgroundDownload
    {
        /// <summary>
        /// Creates a background download for the current item.
        /// </summary>
        /// <param name="oneDriveId">OneDrive Id.</param>
        /// <param name="destinationFile">Destination storage file.</param>
        /// <param name="completionGroup">Completion group.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Storage file.</returns>
        Task<object> CreateBackgroundDownloadForItemAsync(string oneDriveId, object destinationFile, object completionGroup = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
