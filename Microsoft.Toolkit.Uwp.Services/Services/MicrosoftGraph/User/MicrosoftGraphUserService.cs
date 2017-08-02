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
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class for using Office 365 Microsoft Graph User API
    /// </summary>
    public class MicrosoftGraphUserService
    {
        private GraphServiceClient _graphProvider;
        private User _currentConnectedUser = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphUserService"/> class.
        /// </summary>
        /// <param name="graphProvider">Instance of GraphClientService class</param>
        public MicrosoftGraphUserService(GraphServiceClient graphProvider)
        {
            _graphProvider = graphProvider;
        }

        ///// <summary>
        ///// MicrosoftGraphServiceMessages instance
        ///// </summary>
        private MicrosoftGraphServiceMessage _message;

        /// <summary>
        /// Gets MicrosoftGraphServiceMessage instance
        /// </summary>
        public MicrosoftGraphServiceMessage Message
        {
            get { return _message; }
        }

        ///// <summary>
        ///// MicrosoftGraphServiceEvent instance
        ///// </summary>
        private MicrosoftGraphServiceEvent _event;

        /// <summary>
        /// Gets MicrosoftGraphServiceEvent instance
        /// </summary>
        public MicrosoftGraphServiceEvent Event
        {
            get { return _event; }
        }

        /// <summary>
        /// Retrieve current connected user's data.
        /// <para>Permission Scopes:
        /// User.Read (Sign in and read user profile)</para>
        /// </summary>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>Strongly type User info from the service</returns>
        public Task<Graph.User> GetProfileAsync(MicrosoftGraphUserFields[] selectFields = null)
        {
            return GetProfileAsync(CancellationToken.None, selectFields);
        }

        /// <summary>
        /// Retrieve current connected user's data.
        /// <para>Permission Scopes:
        /// User.Read (Sign in and read user profile)</para>
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>Strongly type User info from the service</returns>
        public async Task<Graph.User> GetProfileAsync(CancellationToken cancellationToken, MicrosoftGraphUserFields[] selectFields = null)
        {
            if (selectFields == null)
            {
                _currentConnectedUser = await _graphProvider.Me.Request().GetAsync(cancellationToken);
            }
            else
            {
                string selectedProperties = MicrosoftGraphHelper.BuildString<MicrosoftGraphUserFields>(selectFields);
                _currentConnectedUser = await _graphProvider.Me.Request().Select(selectedProperties).GetAsync(cancellationToken);
            }

            return _currentConnectedUser;
        }

        /// <summary>
        /// Retrieve current connected user's photo.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>A stream containing the user"s photo</returns>
        public async Task<IRandomAccessStream> GetPhotoAsync(CancellationToken cancellationToken)
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
        public Task<IRandomAccessStream> GetPhotoAsync()
        {
            return GetPhotoAsync(CancellationToken.None);
        }

        /// <summary>
        /// Create an instance of MicrosoftGraphServiceMessage
        /// </summary>
        internal void InitializeMessage()
        {
            _message = new MicrosoftGraphServiceMessage(_graphProvider, _currentConnectedUser);
        }

        /// <summary>
        /// Create an instance of MicrosoftGraphServiceEvent
        /// </summary>
        internal void InitializeEvent()
        {
            _event = new MicrosoftGraphServiceEvent(_graphProvider, _currentConnectedUser);
        }
    }
}
