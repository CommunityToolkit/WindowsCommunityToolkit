// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    /// <summary>
    /// Platform-specific implementation to retrieve graph photos.
    /// </summary>
    public interface IMicrosoftGraphUserServicePhotos
    {
        /// <summary>
        /// Overload for returning photo stream.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Stream conttaining photo.</returns>
        Task<object> GetPhotoAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Overload for returning photo stream.
        /// </summary>
        /// <returns>Stream conttaining photo.</returns>
        Task<object> GetPhotoAsync();
    }
}
