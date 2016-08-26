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
    using Microsoft.Graph;

    /// <summary>
    ///  Class using Office 365 Microsoft Graph Messages API
    /// </summary>
    public partial class MicrosoftGraphService
    {
        private const string OrderBy = "receivedDateTime desc";

        /// <summary>
        /// Retrieve user's emails.
        /// <para>(default=10)</para>
        /// <para>Permission Scope: Mail.Read (Read user mail)</para>
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param> 
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>a Collection of Pages containing the messages</returns>
        public async Task<IUserMessagesCollectionPage> GetUserMessagesAsync(int top = 10, MicrosoftGraphMessageFields[] selectFields = null)
        {
            return await this.GetUserMessagesAsync(CancellationToken.None, top, selectFields);
        }

        /// <summary>
        /// Retrieve user's emails.
        /// <para>(default=10)</para>
        /// <para>Permission Scope : Mail.Read (Read user mail)</para>
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>a Collection of Pages containing the messages</returns>
        public async Task<IUserMessagesCollectionPage> GetUserMessagesAsync(CancellationToken cancellationToken, int top = 10, MicrosoftGraphMessageFields[] selectFields = null)
        {
            if (selectFields == null)
            {
                return await graphServiceClient.Me.Messages.Request().Top(top).OrderBy(OrderBy).GetAsync(cancellationToken);
            }

            string selectedProperties = MicrosoftGraphHelper.BuildString<MicrosoftGraphMessageFields>(selectFields);

            return await graphServiceClient.Me.Messages.Request().Top(top).OrderBy(OrderBy).Select(selectedProperties).GetAsync();
        }
    }
}
