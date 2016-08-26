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
