// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Services.MicrosoftGraph.Uwp
{
    /// <summary>
    /// Platform-specific implementation to capture photo.
    /// </summary>
    public class MicrosoftGraphUserServicePhotos : IMicrosoftGraphUserServicePhotos
    {
        private GraphServiceClient _graphProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphUserServicePhotos"/> class.
        /// Constructor.
        /// </summary>
        public MicrosoftGraphUserServicePhotos(GraphServiceClient graphProvider)
        {
            _graphProvider = graphProvider;
        }

        /// <summary>
        /// Retrieve current connected user's photo.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>A stream containing the user"s photo</returns>
        public async Task<object> GetPhotoAsync(CancellationToken cancellationToken)
        {
            IRandomAccessStream windowsPhotoStream = null;
            try
            {
                System.IO.Stream photo = null;
                photo = await _graphProvider.Me.Photo.Content.Request().GetAsync(cancellationToken);
                if (photo != null)
                {
                    windowsPhotoStream = photo.AsRandomAccessStream();
                }
            }
            catch (Microsoft.Graph.ServiceException ex)
            {
                // Swallow error in case of no photo found
                if (!ex.Error.Code.Equals("ErrorItemNotFound"))
                {
                    throw;
                }
            }

            return windowsPhotoStream;
        }

        /// <summary>
        /// Retrieve current connected user's photo.
        /// </summary>
        /// <returns>A stream containing the user"s photo</returns>
        public async Task<object> GetPhotoAsync()
        {
            return await GetPhotoAsync(CancellationToken.None);
        }
    }
}
