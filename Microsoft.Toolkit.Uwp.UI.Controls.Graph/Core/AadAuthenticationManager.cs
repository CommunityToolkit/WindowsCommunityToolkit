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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using static Microsoft.Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Microsoft Graph authentication manager for Microsoft Toolkit Graph controls using Microsoft Authentication Library (MSAL)
    /// </summary>
    public sealed class AadAuthenticationManager : INotifyPropertyChanged
    {
        private static PublicClientApplication _publicClientApp = null;
        private static AadAuthenticationManager _instance;
        private readonly SemaphoreSlim _readLock = new SemaphoreSlim(1, 1);
        private MicrosoftGraphServiceAdapter _base = MicrosoftGraphServiceAdapter.Instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static AadAuthenticationManager Instance => _instance ?? (_instance = new AadAuthenticationManager());

        private AadAuthenticationManager()
        {
        }

        /// <summary>
        /// Gets current application ID.
        /// </summary>
        public string ClientId => _base.ClientId;

        /// <summary>
        /// Gets current permission scopes.
        /// </summary>
        public string[] Scopes => _base.Scopes;

        /// <summary>
        /// Property changed eventHandler for notification.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get => _isAuthenticated;

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
            get => _currentUserId;

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

        internal GraphServiceClient GraphProvider => _base.GraphProvider;

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

            _base.AuthenticationModel = AuthenticationModel.V2;

            _base.Initialize(clientId, ServicesToInitialize.UserProfile, scopes);
        }

        internal async Task<GraphServiceClient> GetGraphServiceClientAsync()
        {
            if (IsAuthenticated)
            {
                return GraphProvider;
            }
            else
            {
                try
                {
                    await _readLock.WaitAsync();
                    if (await ConnectAsync())
                    {
                        return GraphProvider;
                    }
                }
                finally
                {
                    _readLock.Release();
                }

                return null;
            }
        }

        internal async Task<bool> ConnectAsync()
        {
            try
            {
                IsAuthenticated = await _base.LoginAsync();
                if (IsAuthenticated)
                {
                    CurrentUserId = (await _base.User.GetProfileAsync(new MicrosoftGraphUserFields[1] { MicrosoftGraphUserFields.Id })).Id;
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

        internal async void SignOut()
        {
            await _base.Logout();
            CurrentUserId = string.Empty;
            IsAuthenticated = false;
        }

        internal async Task<bool> ConnectForAnotherUserAsync()
        {
            if (!_base.IsInitialized)
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

                await _base.LoginAsync();
                CurrentUserId = (await _base.User.GetProfileAsync(new MicrosoftGraphUserFields[1] { MicrosoftGraphUserFields.Id })).Id;

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