// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using static Microsoft.Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class for connecting to Office 365 Microsoft Graph
    /// </summary>
    public class MicrosoftGraphService
    {
        private readonly SemaphoreSlim _readLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Gets or sets Authentication instance.
        /// </summary>
        internal MicrosoftGraphAuthenticationHelper Authentication { get; set; }

        /// <summary>
        /// Event raised when user logs in our out.
        /// </summary>
        public event EventHandler IsAuthenticatedChanged;

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

        private bool _isAuthenticated;

        /// <summary>
        /// Gets or sets a value indicating whether user is connected.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return _isAuthenticated;
            }

            protected set
            {
                if (_isAuthenticated != value)
                {
                    _isAuthenticated = value;
                    IsAuthenticatedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

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

        private IMicrosoftGraphUserServicePhotos _photosService;

        private UIParent _uiParent = null;

        private string _redirectUri = string.Empty;

#if WINRT
        /// <summary>
        /// Gets or sets field to store the model of authentication
        /// V1 Only for Work or Scholar account
        /// V2 for MSA and Work or Scholar account
        /// </summary>
        public AuthenticationModel AuthenticationModel { get; set; }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphService"/> class.
        /// </summary>
        public MicrosoftGraphService()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphService"/> class.
        /// </summary>
        /// <param name='appClientId'>Azure AD's App client id</param>
        /// <param name="servicesToInitialize">A combination of value to instanciate different services</param>
        /// <param name="delegatedPermissionScopes">Permission scopes for MSAL v2 endpoints</param>
        /// <param name="uiParent">UiParent instance - required for Android</param>
        /// <param name="redirectUri">Redirect Uri - required for Android</param>
        /// <returns>Success or failure.</returns>
        public MicrosoftGraphService(string appClientId, ServicesToInitialize servicesToInitialize = ServicesToInitialize.Message | ServicesToInitialize.UserProfile | ServicesToInitialize.Event, string[] delegatedPermissionScopes = null, UIParent uiParent = null, string redirectUri = null)
        {
            Initialize(appClientId, servicesToInitialize, delegatedPermissionScopes, uiParent, redirectUri);
        }

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

            IsAuthenticated = false;
            User = null;

#if WINRT
            var authenticationModel = AuthenticationModel.ToString();
            return Authentication.LogoutAsync(authenticationModel);
#else
            return Task.Run(() => { return Authentication.Logout(); });
#endif
        }

        /// <summary>
        /// Login the user from Azure AD and Get Microsoft Graph access token.
        /// </summary>
        /// <remarks>Need Sign in and read user profile scopes (User.Read)</remarks>
        /// <returns>Returns success or failure of login attempt.</returns>
        public virtual async Task<bool> LoginAsync(string loginHint = null)
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            Authentication = new MicrosoftGraphAuthenticationHelper(DelegatedPermissionScopes);
            string accessToken = null;
#if WINRT
            if (AuthenticationModel == AuthenticationModel.V1)
            {
                accessToken = await Authentication.GetUserTokenAsync(AppClientId);
            }
            else
            {
                accessToken = await Authentication.GetUserTokenV2Async(AppClientId, loginHint);
            }
#else
            accessToken = await Authentication.GetUserTokenV2Async(AppClientId, _uiParent, _redirectUri, loginHint);
#endif

            if (string.IsNullOrEmpty(accessToken))
            {
                IsAuthenticated = false;
                return IsAuthenticated;
            }

#if WINRT
            User = new MicrosoftGraphUserService(GraphProvider);
#else
            User = new MicrosoftGraphUserService(GraphProvider, _photosService);
#endif

            if ((ServicesToInitialize & ServicesToInitialize.UserProfile) == ServicesToInitialize.UserProfile)
            {
                await GetUserAsyncProfile();
            }

            if ((ServicesToInitialize & ServicesToInitialize.Message) == ServicesToInitialize.Message)
            {
                User.InitializeMessage();
            }

            if ((ServicesToInitialize & ServicesToInitialize.Event) == ServicesToInitialize.Event)
            {
                User.InitializeEvent();
            }

            IsAuthenticated = true;
            return IsAuthenticated;
        }

        /// <summary>
        /// Tries to log in user if not already loged in
        /// </summary>
        /// <returns>true if service is already loged in</returns>
        internal async Task<bool> TryLoginAsync()
        {
            if (!IsInitialized)
            {
                return false;
            }

            if (IsAuthenticated)
            {
                return true;
            }

            try
            {
                await _readLock.WaitAsync();
                await LoginAsync();
            }
            catch (MsalServiceException ex)
            {
                // Swallow error in case of authentication cancellation.
                if (ex.ErrorCode != "authentication_canceled"
                    && ex.ErrorCode != "access_denied")
                {
                    throw ex;
                }
            }
            finally
            {
                _readLock.Release();
            }

            return IsAuthenticated;
        }

        internal async Task<bool> ConnectForAnotherUserAsync()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            try
            {
                var publicClientApplication = new PublicClientApplication(AppClientId);
                AuthenticationResult result = await publicClientApplication.AcquireTokenAsync(DelegatedPermissionScopes);

                var signedUser = result.User;

                foreach (var user in publicClientApplication.Users)
                {
                    if (user.Identifier != signedUser.Identifier)
                    {
                        publicClientApplication.Remove(user);
                    }
                }

                await LoginAsync();

                return true;
            }
            catch (MsalServiceException ex)
            {
                // Swallow error in case of authentication cancellation.
                if (ex.ErrorCode != "authentication_canceled"
                    && ex.ErrorCode != "access_denied")
                {
                    throw ex;
                }
            }

            return false;
        }

        /// <summary>
        /// Create Microsoft Graph client
        /// </summary>
        /// <param name='appClientId'>Azure AD's App client id</param>
        /// <returns>instance of the GraphServiceclient</returns>
        internal virtual GraphServiceClient CreateGraphClientProvider(string appClientId)
        {
#if WINRT
            if (AuthenticationModel == Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.AuthenticationModel.V1)
            {
                return new GraphServiceClient(
                  new DelegateAuthenticationProvider(
                     async (requestMessage) =>
                     {
                         requestMessage.Headers.Authorization =
                             new AuthenticationHeaderValue(
                                         "bearer",
                                         await ((MicrosoftGraphAuthenticationHelper)Authentication).GetUserTokenAsync(appClientId).ConfigureAwait(false));
                         return;
                     }));
            }
#endif
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
