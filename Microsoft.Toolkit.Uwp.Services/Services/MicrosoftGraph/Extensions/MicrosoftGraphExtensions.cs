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
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Graph;

    /// <summary>
    /// GraphServiceClient Extensions
    /// </summary>
    public static class MicrosoftGraphExtensions
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

            string selectedProperties = selectFields.FormatSelectedFields();

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
            return await graphClient.Me.Photo.Content.Request().GetAsync(cancellationToken);
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

        private const string OrderBy = "receivedDateTime desc";

        /// <summary>
        /// Retrieve the user's emails
        /// </summary>
        /// <param name="graphClient">Microsoft Graph Client's instance</param>
        /// <param name="topMessages">The number of messages to return in a response</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response</param>
        /// <returns>A strongly type including a list of the messages</returns>
        public static async Task<IUserMessagesCollectionPage> GetUserMessagesAsync(this GraphServiceClient graphClient, int topMessages, MicrosoftGraphMessageFields[] selectFields)
        {
            return await graphClient.GetUserMessagesAsync(topMessages, selectFields, CancellationToken.None);
        }

        /// <summary>
        /// Retrieve the user's emails
        /// </summary>
        /// <param name="graphClient">Microsoft Graph Client's instance</param>
        /// <param name="topMessages">The number of messages to return in a response</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>A strongly type including a list of the messages</returns>
        public static async Task<IUserMessagesCollectionPage> GetUserMessagesAsync(this GraphServiceClient graphClient, int topMessages, MicrosoftGraphMessageFields[] selectFields, CancellationToken cancellationToken)
        {
            if (selectFields == null)
            {
                return await graphClient.Me.Messages.Request().Top(topMessages).OrderBy(OrderBy).GetAsync(cancellationToken);
            }

            string selectedProperties = selectFields.FormatSelectedFields();

            return await graphClient.Me.Messages.Request().Top(topMessages).OrderBy(OrderBy).Select(selectedProperties).GetAsync();
        }

        /// <summary>
        /// Format an unique string with each array's items.
        /// </summary>
        /// <param name="selectFields">an array of MicrosoftGraphUserFields containing the fields</param>
        /// <returns>a string with all fields separate by a comma.</returns>
        private static string FormatSelectedFields(this MicrosoftGraphUserFields[] selectFields)
        {
            return FormatString<MicrosoftGraphUserFields>(selectFields);
        }

        /// <summary>
        /// Format an unique string with each array's items.
        /// </summary>
        /// <param name="selectFields">an array of MicrosoftGraphUserFields containing the fields</param>
        /// <returns>a string with all fields separate by a comma.</returns>
        private static string FormatSelectedFields(this MicrosoftGraphMessageFields[] selectFields)
        {
            return FormatString<MicrosoftGraphMessageFields>(selectFields);
        }

        private static string FormatString<T>(T[] t)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var field in t)
            {
                sb.Append(field.ToString());
                sb.Append(",");
            }

            string tempo = sb.ToString();

            // Remove the trailing comma character
            int lastPosition = tempo.Length - 1;

            return tempo.Substring(0, lastPosition);
        }
    }
}
