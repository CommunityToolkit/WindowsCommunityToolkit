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
    /// AadAuthenticationManager
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
        /// Gets or sets ClientId
        /// </summary>
        public string ClientId
        {
            get
            {
                return _clientId;
            }

            set
            {
                if (value != _clientId)
                {
                    _clientId = value;
                    InitializeClientApp();
                    NotifyPropertyChanged(nameof(ClientId));
                }
            }
        }

        /// <summary>
        /// Gets or sets Scopes
        /// </summary>
        public string Scopes
        {
            get
            {
                return _scopes;
            }

            set
            {
                if (value != _scopes)
                {
                    _scopes = value;
                    InitializeClientApp();
                    NotifyPropertyChanged(nameof(Scopes));
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsAuthenticated
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
        /// Gets CurrentUserId
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

        private const string GraphAPIBaseUrl = "https://graph.microsoft.com/v1.0";
        private static volatile AadAuthenticationManager _instance;
        private static object _syncRoot = new object();
        private static PublicClientApplication _publicClientApp = null;
        private DateTimeOffset _expiration;
        private string _tokenForUser;
        private string _clientId;
        private string _scopes;
        private bool _isAuthenticated;
        private string _currentUserId;

        private IEnumerable<string> ScopesArray
        {
            get
            {
                return Scopes?.Split(',').Select(p => p.Trim());
            }
        }

        private AadAuthenticationManager()
        {
        }

        private void InitializeClientApp()
        {
            _publicClientApp = new PublicClientApplication(ClientId);
            IsAuthenticated = false;
        }

        /// <summary>
        /// Property changed eventHandler for notification.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="clientId">clientId</param>
        /// <param name="scopes">scopes</param>
        public void Initialize(string clientId, params string[][] scopes)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("1");
            }

            if (scopes.Length == 0)
            {
                throw new ArgumentException("1");
            }

            ClientId = clientId;
            Scopes = string.Join(", ", scopes.SelectMany(i => i).Distinct());
        }

        /// <summary>
        /// GetGraphServiceClientAsync
        /// </summary>
        /// <returns>GraphServiceClient</returns>
        public async Task<GraphServiceClient> GetGraphServiceClientAsync()
        {
            GraphServiceClient graphClient = null;

            if (IsAuthenticated)
            {
                string token = await GetTokenForUserAsync();

                graphClient = new GraphServiceClient(
                    GraphAPIBaseUrl,
                    new DelegateAuthenticationProvider(
                    (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

                        return Task.FromResult(0);
                    }));
            }

            return graphClient;
        }

        internal async Task<bool> SignInAsync()
        {
            if (!string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(Scopes))
            {
                string token = await GetTokenForUserAsync();

                if (!string.IsNullOrEmpty(token))
                {
                    IsAuthenticated = true;
                    GraphServiceClient graphClient = await GetGraphServiceClientAsync();
                    var user = await graphClient.Me.Request().GetAsync();
                    CurrentUserId = user.Id;
                }
            }

            return IsAuthenticated;
        }

        internal void SignOut()
        {
            if (_publicClientApp != null && _publicClientApp.Users != null)
            {
                foreach (var user in _publicClientApp.Users)
                {
                    _publicClientApp.Remove(user);
                }

                IsAuthenticated = false;
            }
        }

        private async Task<string> GetTokenForUserAsync()
        {
            if (_tokenForUser == null)
            {
                try
                {
                    AuthenticationResult authResult = await _publicClientApp.AcquireTokenSilentAsync(ScopesArray, _publicClientApp.Users.First());
                    _tokenForUser = authResult.AccessToken;
                    _expiration = authResult.ExpiresOn;
                }
                catch
                {
                    await GetTokenWithPromptAsync();
                }
            }
            else if (_expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
            {
                AuthenticationResult authResult = await _publicClientApp.AcquireTokenSilentAsync(ScopesArray, _publicClientApp.Users.First());
                _tokenForUser = authResult.AccessToken;
                _expiration = authResult.ExpiresOn;
            }

            return _tokenForUser;
        }

        private async Task<string> GetTokenWithPromptAsync()
        {
            try
            {
                AuthenticationResult authResult = await _publicClientApp.AcquireTokenAsync(ScopesArray);
                _tokenForUser = authResult.AccessToken;
                _expiration = authResult.ExpiresOn;
            }
            catch
            {
            }

            return _tokenForUser;
        }
    }
}
