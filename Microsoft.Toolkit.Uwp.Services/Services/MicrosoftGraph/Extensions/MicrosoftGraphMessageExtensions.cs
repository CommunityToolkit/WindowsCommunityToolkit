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
    /// GraphServiceClient Extensions
    /// </summary>
    public static class MicrosoftGraphMessageExtensions
    {
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
        /// <param name="topMessages">The number of messages to return in the response</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>A strongly type including a list of the messages</returns>
        public static async Task<IUserMessagesCollectionPage> GetUserMessagesAsync(this GraphServiceClient graphClient, int topMessages, MicrosoftGraphMessageFields[] selectFields, CancellationToken cancellationToken)
        {
            if (selectFields == null)
            {
                return await graphClient.Me.Messages.Request().Top(topMessages).OrderBy(OrderBy).GetAsync(cancellationToken);
            }

            string selectedProperties = MicrosoftGraphHelper.BuildString<MicrosoftGraphMessageFields>(selectFields);

            return await graphClient.Me.Messages.Request().Top(topMessages).OrderBy(OrderBy).Select(selectedProperties).GetAsync();
        }

        /// <summary>
        /// IUserMessagesCollectionPage extension collecting the next page of messages
        /// </summary>
        /// <param name="nextPage">Instance of IUserMessagesCollectionPage</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>the next collection of messages or null if there are anymore messages</returns>
        public static async Task<IUserMessagesCollectionPage> NextPageAsync(this IUserMessagesCollectionPage nextPage,CancellationToken cancellationToken)
        {
            if (nextPage.NextPageRequest != null)
            {
                return await nextPage.NextPageRequest.GetAsync(cancellationToken);
            }

            // anymore messages
            return null;
        }

        /// <summary>
        /// IUserMessagesCollectionPage extension collecting the next page of messages
        /// </summary>
        /// <param name="nextPage">Instance of IUserMessagesCollectionPage</param>        
        /// <returns>the next collection of messages or null if there are anymore messages</returns>
        public static async Task<IUserMessagesCollectionPage> NextPageAsync(this IUserMessagesCollectionPage nextPage)
        {

            return await nextPage.NextPageAsync(CancellationToken.None);
        }
    }
}
