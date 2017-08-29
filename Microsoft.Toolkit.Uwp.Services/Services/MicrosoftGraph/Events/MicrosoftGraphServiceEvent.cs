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

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class using Office 365 Microsoft Graph Messages API
    /// </summary>
    public class MicrosoftGraphServiceEvent
    {
        private const string OrderBy = "start/datetime desc";

        /// <summary>
        /// GraphServiceClient instance
        /// </summary>
        private readonly GraphServiceClient _graphProvider;

        /// <summary>
        /// Store the request for the next page of messages
        /// </summary>
        private IUserEventsCollectionRequest _nextPageRequest;

        /// <summary>
        /// Store the connected user's profile
        /// </summary>
        private Graph.User _currentUser = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphServiceEvent"/> class.
        /// </summary>
        /// <param name="graphClientProvider">Instance of GraphClientService class</param>
        /// <param name="currentConnectedUser">Instance of Graph.User class</param>
        public MicrosoftGraphServiceEvent(GraphServiceClient graphClientProvider, User currentConnectedUser)
        {
            _graphProvider = graphClientProvider;
            _currentUser = currentConnectedUser;
        }

        /// <summary>
        /// retrieve the next page of events
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>the next collection of messages or null if there are no more messages</returns>
        public async Task<IUserEventsCollectionPage> NextPageEventsAsync(CancellationToken cancellationToken)
        {
            if (_nextPageRequest != null)
            {
                var events = await _nextPageRequest.GetAsync(cancellationToken);
                _nextPageRequest = events.NextPageRequest;
                return events;
            }

            // no more messages
            return null;
        }

        /// <summary>
        /// retrieve the next page of messages
        /// </summary>
        /// <returns>the next collection of messages or null if there are no more messages</returns>
        public Task<IUserEventsCollectionPage> NextPageEventsAsync()
        {
            return NextPageEventsAsync(CancellationToken.None);
        }

        /// <summary>
        /// Retrieve current connected user's events.
        /// <para>(default=10)</para>
        /// <para>Permission Scope: Event.Read (Read user calendar)</para>
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>a Collection of Pages containing the events</returns>
        public Task<IUserEventsCollectionPage> GetEventsAsync(int top = 10, MicrosoftGraphEventFields[] selectFields = null)
        {
            return GetEventsAsync(CancellationToken.None, top, selectFields);
        }

        /// <summary>
        /// Retrieve current connected user's events.
        /// <para>(default=10)</para>
        /// <para>Permission Scope : Event.Read (Read user calendar)</para>
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>a Collection of Pages containing the events</returns>
        public async Task<IUserEventsCollectionPage> GetEventsAsync(CancellationToken cancellationToken, int top = 10, MicrosoftGraphEventFields[] selectFields = null)
        {
            IUserEventsCollectionPage events = null;
            if (selectFields == null)
            {
                events = await _graphProvider.Me.Events.Request().OrderBy(OrderBy).Top(top).GetAsync(cancellationToken);
            }
            else
            {
                string selectedProperties = MicrosoftGraphHelper.BuildString(selectFields);
                events = await _graphProvider.Me.Events.Request().OrderBy(OrderBy).Top(top).Select(selectedProperties).GetAsync();
            }

            _nextPageRequest = events.NextPageRequest;
            return events;
        }
    }
}
