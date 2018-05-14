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

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph.Platform;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Services.MicrosoftGraph.Platform
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
