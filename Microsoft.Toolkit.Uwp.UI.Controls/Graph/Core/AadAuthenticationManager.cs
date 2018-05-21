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
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Toolkit.Services.MicrosoftGraph;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Microsoft Graph authentication manager for Microsoft Toolkit Graph controls using Microsoft Authentication Library (MSAL)
    /// </summary>
    public sealed class AadAuthenticationManager : MicrosoftGraphService, INotifyPropertyChanged
    {
        private static PublicClientApplication _publicClientApp = null;
        private static AadAuthenticationManager _instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static new AadAuthenticationManager Instance => _instance ?? (_instance = new AadAuthenticationManager());

        private AadAuthenticationManager()
        {
        }

        /// <summary>
        /// Gets current application ID.
        /// </summary>
        public string ClientId
        {
            get
            {
                return AppClientId;
            }
        }

        /// <summary>
        /// Gets current permission scopes.
        /// </summary>
        public string[] Scopes
        {
            get
            {
                return DelegatedPermissionScopes;
            }
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

        private bool _isAuthenticated = false;

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

        private string _currentUserId;

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

            AuthenticationModel = MicrosoftGraphEnums.AuthenticationModel.V2;

            Initialize(clientId, MicrosoftGraphEnums.ServicesToInitialize.UserProfile, scopes);
        }

        internal async Task<bool> ConnectAsync()
        {
            try
            {
                IsAuthenticated = await LoginAsync();
                if (IsAuthenticated)
                {
                    CurrentUserId = (await User.GetProfileAsync(new MicrosoftGraphUserFields[1] { MicrosoftGraphUserFields.Id })).Id;
                }
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

            return IsAuthenticated;
        }

        internal Task<GraphServiceClient> GetGraphServiceClientAsync()
        {
            return Task.FromResult(GraphProvider);
        }

        internal async void SignOut()
        {
            await Logout();
            CurrentUserId = string.Empty;
            IsAuthenticated = false;
        }

        internal async Task<bool> ConnectForAnotherUserAsync()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            try
            {
                _publicClientApp = new PublicClientApplication(ClientId);
                AuthenticationResult result = await _publicClientApp.AcquireTokenAsync(Scopes);

                var signedUser = result.User;

                foreach (var user in _publicClientApp.Users)
                {
                    if (user.Identifier != signedUser.Identifier)
                    {
                        _publicClientApp.Remove(user);
                    }
                }

                await LoginAsync();
                CurrentUserId = (await User.GetProfileAsync(new MicrosoftGraphUserFields[1] { MicrosoftGraphUserFields.Id })).Id;

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

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
