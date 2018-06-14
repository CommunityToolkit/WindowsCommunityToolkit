// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    /// Background service interface.
    /// </summary>
    public interface IOneDriveServicePlatform
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
