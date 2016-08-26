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
    using System.Threading;
    using System.Threading.Tasks;
    using Graph;

    /// <summary>
    /// User's GraphServiceClient Extensions
    /// </summary>
    public static class MicrosoftGraphUserExtensions
    {
        /// <summary>
        /// Retrieve user's profile.
        /// </summary>
        /// <param name="graphClient">Microsoft Graph Client's instance</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>Strongly type User info from the service</returns>
        public static async Task<Microsoft.Graph.User> GetMeProfileAsync(this GraphServiceClient graphClient, MicrosoftGraphUserFields[] selectFields, CancellationToken cancellationToken)
        {

            if (selectFields == null)
            {
                return await graphClient.Me.Request().GetAsync(cancellationToken);
            }

            string selectedProperties = MicrosoftGraphHelper.FormatString<MicrosoftGraphUserFields>(selectFields);

            return await graphClient.Me.Request().Select(selectedProperties).GetAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieve user's profile.
        /// </summary>
        /// <param name="graphClient">Microsoft Graph Client's instance</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>Strongly type User info from the service</returns>
        public static async Task<Microsoft.Graph.User> GetMeProfileAsync(this GraphServiceClient graphClient, MicrosoftGraphUserFields[] selectFields)
        {
            return await graphClient.GetMeProfileAsync(selectFields, CancellationToken.None);
        }

        /// <summary>
        /// Retrieve the user's photo
        /// </summary>
        /// <param name="graphClient">Microsoft Graph Client's instance</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>A stream containing the user's photo</returns>
        public static async Task<System.IO.Stream> GetMePhotoAsync(this GraphServiceClient graphClient, CancellationToken cancellationToken)
        {
            System.IO.Stream photo = null;
            try
            {
                photo = await graphClient.Me.Photo.Content.Request().GetAsync(cancellationToken);
            }
            catch (Microsoft.Graph.ServiceException ex)
            {
                // Swallow error in case of no photo found
                if (!ex.Error.Code.Equals("ErrorItemNotFound"))
                {
                    throw;
                }
            }

            return photo;
        }

        /// <summary>
        /// Retrieve the user's photo
        /// </summary>
        /// <param name="graphClient">Microsoft Graph Client's instance</param>
        /// <returns>A stream containing the user's photo</returns>
        public static async Task<System.IO.Stream> GetMyPhotoAsync(this GraphServiceClient graphClient)
        {
            return await graphClient.GetMePhotoAsync(CancellationToken.None);
        }

    }
}
