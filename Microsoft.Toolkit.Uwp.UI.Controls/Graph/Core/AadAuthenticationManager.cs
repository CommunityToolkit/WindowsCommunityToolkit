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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Microsoft Graph authentication manager for Microsoft Toolkit Graph controls using Microsoft Authentication Library (MSAL)
    /// </summary>
    public sealed class AadAuthenticationManager : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static AadAuthenticationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new AadAuthenticationManager();
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Gets current application ID.
        /// </summary>
        public string ClientId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets current permission scopes.
        /// </summary>
        public string[] Scopes
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return _isAuthenticated;
            }

            private set
            {
                if (value != _isAuthenticated)
                {
                    _isAuthenticated = value;
                    NotifyPropertyChanged(nameof(IsAuthenticated));
                }
            }
        }

        /// <summary>
        /// Gets current user id.
        /// </summary>
        public string CurrentUserId
        {
            get
            {
                return _currentUserId;
            }

            private set
            {
                if (value != _currentUserId)
                {
                    _currentUserId = value;
                    NotifyPropertyChanged(nameof(CurrentUserId));
                }
            }
        }

        internal bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }

            private set
            {
                if (value != _isInitialized)
                {
                    _isInitialized = value;
                    NotifyPropertyChanged(nameof(IsInitialized));
                }
            }
        }

        private const string GraphApiBaseUrl = "https://graph.microsoft.com/v1.0";
        private static volatile AadAuthenticationManager _instance;
        private static PublicClientApplication _publicClientApp = null;
        private static object _syncRoot = new object();
        private IUser _user;
        private bool _isAuthenticated;
        private bool _isInitialized = false;
        private string _currentUserId;

        private AadAuthenticationManager()
        {
            IsAuthenticated = false;
        }

        /// <summary>
        /// Property changed eventHandler for notification.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initialize for the <see cref="AadAuthenticationManager"/> class
        /// </summary>
        /// <param name="clientId">Application client ID for MSAL v2 endpoints</param>
        /// <param name="scopes">Permission scopes for MSAL v2 endpoints</param>
        public void Initialize(string clientId, params string[] scopes)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (scopes.Length == 0)
            {
                throw new ArgumentNullException(nameof(scopes));
            }

            ClientId = clientId;
            Scopes = scopes.Distinct().ToArray();
            _publicClientApp = new PublicClientApplication(ClientId);
            _user = _publicClientApp.Users?.FirstOrDefault();
            IsInitialized = true;
        }

        internal async Task<bool> ConnectAsync()
        {
            string token = await GetTokenForUserAsync();

            if (!string.IsNullOrEmpty(token))
            {
                GraphServiceClient graphClient = await GetGraphServiceClientAsync();
                var user = await graphClient.Me.Request().GetAsync();
                CurrentUserId = user.Id;
            }

            return IsAuthenticated;
        }

        internal async Task<GraphServiceClient> GetGraphServiceClientAsync()
        {
            GraphServiceClient graphClient = null;

            string token = await GetTokenForUserAsync();

            if (!string.IsNullOrEmpty(token))
            {
                return new GraphServiceClient(
                    GraphApiBaseUrl,
                    new DelegateAuthenticationProvider(
                    (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

                        return Task.FromResult(0);
                    }));
            }

            return graphClient;
        }

        internal void SignOut()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            _user = null;

            if (_publicClientApp.Users != null)
            {
                foreach (var user in _publicClientApp.Users)
                {
                    _publicClientApp.Remove(user);
                }

                IsAuthenticated = false;
            }
        }

        internal async Task<bool> ConnectForAnotherUserAsync()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            if (!string.IsNullOrEmpty(await GetTokenWithPromptAsync()))
            {
                GraphServiceClient graphClient = await GetGraphServiceClientAsync();
                var user = await graphClient.Me.Request().GetAsync();
                CurrentUserId = user.Id;
                return true;
            }

            return false;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task<string> GetTokenForUserAsync()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            string tokenForUser = null;
            AuthenticationResult result = null;

            try
            {
                result = await _publicClientApp.AcquireTokenSilentAsync(Scopes, _user);
                tokenForUser = result.AccessToken;
            }
            catch (MsalUiRequiredException)
            {
                tokenForUser = await GetTokenWithPromptAsync(_user);
            }

            IsAuthenticated = !string.IsNullOrEmpty(tokenForUser);
            return tokenForUser;
        }

        private async Task<string> GetTokenWithPromptAsync(IUser user = null)
        {
            AuthenticationResult result = null;

            try
            {
                result = await _publicClientApp.AcquireTokenAsync(Scopes, user);
                _user = result.User;

                return result.AccessToken;
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

            return null;
        }
    }
}
