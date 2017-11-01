using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services.MicrosoftGraph.Platform
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
