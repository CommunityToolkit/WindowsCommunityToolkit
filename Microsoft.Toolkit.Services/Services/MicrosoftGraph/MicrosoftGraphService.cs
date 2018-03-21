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
using Microsoft.Identity.Client;
using Microsoft.Toolkit.Services.MicrosoftGraph.Platform;
using static Microsoft.Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class for connecting to Office 365 Microsoft Graph
    /// </summary>
    public class MicrosoftGraphService
    {
        /// <summary>
        /// Gets or sets Authentication instance.
        /// </summary>
        internal MicrosoftGraphAuthenticationHelper Authentication { get; set; }

        /// <summary>
        /// Gets or sets store a reference to an instance of the underlying data provider.
        /// </summary>
        public GraphServiceClient GraphProvider { get; set; }

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static MicrosoftGraphService _instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static MicrosoftGraphService Instance => _instance ?? (_instance = new MicrosoftGraphService());

        /// <summary>
        /// Gets or sets a value indicating whether initialization status.
        /// </summary>
        protected bool IsInitialized { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user is connected.
        /// </summary>
        protected bool IsConnected { get; set; }

        /// <summary>
        /// Gets or sets AppClientId.
        /// </summary>
        protected string AppClientId { get; set; }

        /// <summary>
        /// Gets or sets field to store the services to initialize
        /// </summary>
        protected ServicesToInitialize ServicesToInitialize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating deletgated permission scopes for MSAL (v2) endpoint
        /// </summary>
        protected string[] DelegatedPermissionScopes { get; set; }

        /// <summary>
        /// Gets or sets fields to store a MicrosoftGraphServiceMessages instance
        /// </summary>
        public virtual MicrosoftGraphUserService User { get; set; }

        /// <summary>
        /// Gets the code to enter in http://aka.ms/deviceauth
        /// </summary>
        public virtual string UserCode { get; }

        private IMicrosoftGraphUserServicePhotos _photosService;

        private UIParent _uiParent = null;

        private string _redirectUri = string.Empty;

        /// <summary>
        /// Initialize Microsoft Graph.
        /// </summary>
        /// <typeparam name="T">Concrete type that inherits IMicrosoftGraphUserServicePhotos.</typeparam>
        /// <param name='appClientId'>Azure AD's App client id</param>
        /// <param name="servicesToInitialize">A combination of value to instanciate different services</param>
        /// <param name="delegatedPermissionScopes">Permission scopes for MSAL v2 endpoints</param>
        /// <param name="uiParent">UiParent instance - required for Android</param>
        /// <param name="redirectUri">Redirect Uri - required for Android</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize<T>(string appClientId, ServicesToInitialize servicesToInitialize = ServicesToInitialize.Message | ServicesToInitialize.UserProfile | ServicesToInitialize.Event, string[] delegatedPermissionScopes = null, UIParent uiParent = null, string redirectUri = null)
            where T : IMicrosoftGraphUserServicePhotos, new()
        {
            if (string.IsNullOrEmpty(appClientId))
            {
                throw new ArgumentNullException(nameof(appClientId));
            }

            _redirectUri = redirectUri;
            _uiParent = uiParent;
            _photosService = new T();
            AppClientId = appClientId;
            GraphProvider = CreateGraphClientProvider(appClientId);
            ServicesToInitialize = servicesToInitialize;
            IsInitialized = true;
            DelegatedPermissionScopes = delegatedPermissionScopes;
            return true;
        }

        /// <summary>
        /// Initialize Microsoft Graph.
        /// </summary>
        /// <param name='appClientId'>Azure AD's App client id</param>
        /// <param name="servicesToInitialize">A combination of value to instanciate different services</param>
        /// <param name="delegatedPermissionScopes">Permission scopes for MSAL v2 endpoints</param>
        /// <param name="uiParent">UiParent instance - required for Android</param>
        /// <param name="redirectUri">Redirect Uri - required for Android</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string appClientId, ServicesToInitialize servicesToInitialize = ServicesToInitialize.Message | ServicesToInitialize.UserProfile | ServicesToInitialize.Event, string[] delegatedPermissionScopes = null, UIParent uiParent = null, string redirectUri = null)
        {
            if (string.IsNullOrEmpty(appClientId))
            {
                throw new ArgumentNullException(nameof(appClientId));
            }

            _redirectUri = redirectUri;
            _uiParent = uiParent;
            AppClientId = appClientId;
            GraphProvider = CreateGraphClientProvider(appClientId);
            ServicesToInitialize = servicesToInitialize;
            IsInitialized = true;
            DelegatedPermissionScopes = delegatedPermissionScopes;
            return true;
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <returns>success or failure</returns>
        public virtual Task<bool> Logout()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            return Task.Run(() => { return Authentication.Logout(); });
        }

        /// <summary>
        /// Login the user from Azure AD and Get Microsoft Graph access token.
        /// </summary>
        /// <remarks>Need Sign in and read user profile scopes (User.Read)</remarks>
        /// <returns>Returns success or failure of login attempt.</returns>
        public virtual async Task<bool> LoginAsync()
        {
            IsConnected = false;
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            Authentication = new MicrosoftGraphAuthenticationHelper(DelegatedPermissionScopes);
            string accessToken = await Authentication.GetUserTokenV2Async(AppClientId, _uiParent, _redirectUri);

            if (string.IsNullOrEmpty(accessToken))
            {
                return IsConnected;
            }

            IsConnected = true;

            User = new MicrosoftGraphUserService(GraphProvider, _photosService);

            if ((ServicesToInitialize & Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ServicesToInitialize.UserProfile) == Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ServicesToInitialize.UserProfile)
            {
                await GetUserAsyncProfile();
            }

            if ((ServicesToInitialize & Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ServicesToInitialize.Message) == Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ServicesToInitialize.Message)
            {
                User.InitializeMessage();
            }

            if ((ServicesToInitialize & Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ServicesToInitialize.Event) == Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ServicesToInitialize.Event)
            {
                User.InitializeEvent();
            }

            return IsConnected;
        }

        /// <summary>
        /// Create Microsoft Graph client
        /// </summary>
        /// <param name='appClientId'>Azure AD's App client id</param>
        /// <returns>instance of the GraphServiceclient</returns>
        internal virtual GraphServiceClient CreateGraphClientProvider(string appClientId)
        {
            return new GraphServiceClient(
                  new DelegateAuthenticationProvider(
                     async (requestMessage) =>
                     {
                         // requestMessage.Headers.Add('outlook.timezone', 'Romance Standard Time');
                         requestMessage.Headers.Authorization =
                                            new AuthenticationHeaderValue(
                                                     "bearer",
                                                     await Authentication.GetUserTokenV2Async(appClientId).ConfigureAwait(false));
                     }));
        }

        /// <summary>
        /// Initialize a instance of MicrosoftGraphUserService class
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task GetUserAsyncProfile()
        {
            MicrosoftGraphUserFields[] selectedFields =
            {
                MicrosoftGraphUserFields.Id
            };

            await User.GetProfileAsync(selectedFields);
        }
    }
}
