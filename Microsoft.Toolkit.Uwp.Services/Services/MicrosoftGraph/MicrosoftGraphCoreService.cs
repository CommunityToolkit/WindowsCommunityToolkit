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

using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class for connecting to Office 365 Microsoft Graph
    /// </summary>
    public partial class MicrosoftGraphService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphService"/> class.
        /// </summary>
        public MicrosoftGraphService()
        {
        }

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static MicrosoftGraphService _instance;

        /// <summary>
        /// Store an instance of the MicrosoftGraphAuthenticationHelper class
        /// </summary>
        private MicrosoftGraphAuthenticationHelper _authentication;

        /// <summary>
        /// Store a reference to an instance of the underlying data provider.
        /// </summary>
        private GraphServiceClient _graphClientProvider;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static MicrosoftGraphService Instance => _instance ?? (_instance = new MicrosoftGraphService());

        /// <summary>
        /// Field for tracking initialization status.
        /// </summary>
        private bool _isInitialized;

        /// <summary>
        /// Field for tracking if the user is connected.
        /// </summary>
        private bool _isConnected;

        /// <summary>
        /// Field to store Azure AD Application clientid
        /// </summary>
        private string _appClientId;

        /// <summary>
        /// Fields to store a MicrosoftGraphServiceMessages instance
        /// </summary>
        private MicrosoftGraphUserService _user;

        /// <summary>
        /// Gets a reference to an instance of the MicrosoftGraphUserService class
        /// </summary>
        public MicrosoftGraphUserService User
        {
            get { return _user; }
        }

        /// <summary>
        /// Initialize Microsoft Graph.
        /// </summary>
        /// <param name='appClientId'>Azure AD's App client id</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string appClientId)
        {
            if (string.IsNullOrEmpty(appClientId))
            {
                throw new ArgumentNullException(nameof(appClientId));
            }

            _appClientId = appClientId;

            _graphClientProvider = CreateGraphClient(appClientId);
            _isInitialized = true;
            return true;
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <returns>success or failure</returns>
        public bool Logout()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            _authentication = null;

            return true;
        }

        /// <summary>
        /// Login the user from Azure AD and Get Microsoft Graph access token.
        /// </summary>
        /// <remarks>Need Sign in and read user profile scopes (User.Read)</remarks>
        /// <returns>Returns success or failure of login attempt.</returns>
        public async Task<bool> LoginAsync()
        {
            _isConnected = false;
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            _authentication = new MicrosoftGraphAuthenticationHelper();
            var accessToken = await _authentication.GetUserTokenAsync(_appClientId);
            if (string.IsNullOrEmpty(accessToken))
            {
                return _isConnected;
            }

            _isConnected = true;

            await InitializeUserAsync();

            return _isConnected;
        }

        /// <summary>
        /// Create Microsoft Graph client
        /// </summary>
        /// <param name='appClientId'>Azure AD's App client id</param>
        /// <returns>instance of the GraphServiceclient</returns>
        private GraphServiceClient CreateGraphClient(string appClientId)
        {
            return new GraphServiceClient(
                  new DelegateAuthenticationProvider(
                     async (requestMessage) =>
                     {
                         // requestMessage.Headers.Add('outlook.timezone', 'Romance Standard Time');
                         requestMessage.Headers.Authorization =
                                            new AuthenticationHeaderValue(
                                                     "bearer",
                                                     await _authentication.GetUserTokenAsync(appClientId).ConfigureAwait(false));
                         return;
                     }));
        }

        /// <summary>
        /// Initialize a instance of MicrosoftGraphUserService class
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        private Task InitializeUserAsync()
        {
            MicrosoftGraphUserFields[] selectedFields =
            {
                MicrosoftGraphUserFields.Id
            };
            _user = new MicrosoftGraphUserService(_graphClientProvider);
            return _user.GetProfileAsync(selectedFields);
        }
    }
}
