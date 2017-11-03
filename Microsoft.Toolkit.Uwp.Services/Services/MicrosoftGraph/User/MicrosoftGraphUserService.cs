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

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Uwp.Services.MicrosoftGraph.Platform;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class for using Office 365 Microsoft Graph User API
    /// </summary>
    public class MicrosoftGraphUserService : Toolkit.Services.MicrosoftGraph.MicrosoftGraphUserService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphUserService"/> class.
        /// </summary>
        /// <param name="graphProvider">Instance of GraphClientService class</param>
        public MicrosoftGraphUserService(GraphServiceClient graphProvider)
            : base(graphProvider, null)
        {
            PhotosService = new MicrosoftGraphUserServicePhotos(graphProvider);
        }

        /// <summary>
        /// Retrieve current connected user's photo.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>A stream containing the user"s photo</returns>
        public async Task<IRandomAccessStream> GetPhotoAsync(CancellationToken cancellationToken)
        {
            return (await PhotosService.GetPhotoAsync(CancellationToken.None)) as IRandomAccessStream;
        }

        /// <summary>
        /// Retrieve current connected user's photo.
        /// </summary>
        /// <returns>A stream containing the user"s photo</returns>
        public async Task<IRandomAccessStream> GetPhotoAsync()
        {
            return (await PhotosService.GetPhotoAsync()) as IRandomAccessStream;
        }
    }
}
