// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
// ******************************************************************

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Graph;
    using Windows.Storage.Streams;

    /// <summary>
    ///  Class for using  Office 365 Microsoft Graph User API
    /// </summary>
    public partial class MicrosoftGraphService
    {
        private Graph.User currentConnectedUser = null;

        /// <summary>
        /// Retrieve user data.
        /// <para>Permission Scopes:
        /// User.Read (Sign in and read user profile)</para>
        /// </summary>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>Strongly type User info from the service</returns>
        public Task<Graph.User> GetUserAsync(MicrosoftGraphUserFields[] selectFields = null)
        {
            return this.GetUserAsync(CancellationToken.None, selectFields);
        }

        /// <summary>
        /// Retrieve user"s data.
        /// <para>Permission Scopes:
        /// User.Read (Sign in and read user profile)</para>
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>Strongly type User info from the service</returns>
        public async Task<Graph.User> GetUserAsync(CancellationToken cancellationToken, MicrosoftGraphUserFields[] selectFields = null)
        {
            if (selectFields == null)
            {
                currentConnectedUser = await graphServiceClient.Me.Request().GetAsync(cancellationToken);
            }
            else
            {
                string selectedProperties = MicrosoftGraphHelper.BuildString<MicrosoftGraphUserFields>(selectFields);
                currentConnectedUser = await graphServiceClient.Me.Request().Select(selectedProperties).GetAsync(cancellationToken);
            }

            return currentConnectedUser;
        }

        /// <summary>
        /// Retrieve the user"s Photo
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>A stream containing the user"s photo</returns>
        public async Task<IRandomAccessStream> GetUserPhotoAsync(CancellationToken cancellationToken)
        {

            IRandomAccessStream windowsPhotoStream = null;
            try
            {
                System.IO.Stream photo = null;
                photo = await graphServiceClient.Me.Photo.Content.Request().GetAsync(cancellationToken);
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
        /// Retrieve the user"s Photo
        /// </summary>
        /// <returns>A stream containing the user"s photo</returns>
        public Task<IRandomAccessStream> GetUserPhotoAsync()
        {
            return this.GetUserPhotoAsync(CancellationToken.None);
        }
    }
}
