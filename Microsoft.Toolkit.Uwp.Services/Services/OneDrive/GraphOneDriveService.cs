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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// GraphOneDriveService type
    /// </summary>
    public class GraphOneDriveService : IOneDriveService
    {
        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static IOneDriveService _instance;

        /// <summary>
        /// Field to store Azure AD Application clientId
        /// </summary>
        private string _appClientId;

        /// <summary>
        /// Field for tracking initialization status.
        /// </summary>
        private bool _isInitialized;

        /// <summary>
        /// Field for tracking if the user is connected.
        /// </summary>
        private bool _isConnected;

        /// <summary>
        /// Fields to store the permission levels than an app can request from a user
        /// </summary>
        private string[] _scopes;

        /// <summary>
        /// Fields to store the account provider
        /// </summary>
        private IAuthenticationProvider _accountProvider;

        /// <summary>
        /// Fields to store the account provider
        /// </summary>
        private AccountProviderType _accountProviderType;

        /// <summary>
        /// Store a reference to an instance of the underlying data provider.
        /// </summary>
        private IGraphServiceClient _oneDriveProvider = null;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static IOneDriveService Instance => _instance ?? (_instance = new OneDriveService());

        /// <summary>
        /// Gets a reference to an instance of the underlying data provider.
        /// </summary>
        public IBaseClient Provider
        {
            get
            {
                if (_oneDriveProvider == null)
                {
                    throw new InvalidOperationException("Provider not initialized.");
                }

                return _oneDriveProvider;
            }
        }

        /// <inheritdoc/>
        public bool Initialize(OneDriveScopes scopes = OneDriveScopes.OfflineAccess | OneDriveScopes.ReadWrite)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Initialize(string appClientId, OneDriveEnums.AccountProviderType accountProviderType = AccountProviderType.Msal, OneDriveScopes scopes = OneDriveScopes.OfflineAccess | OneDriveScopes.ReadWrite)
        {
            if (accountProviderType == AccountProviderType.OnlineId || accountProviderType == AccountProviderType.Msa)
            {
                throw new ArgumentException("Authentication with OnlineId or Msa are not supported");
            }

            if (string.IsNullOrEmpty(appClientId))
            {
                throw new ArgumentNullException(nameof(appClientId));
            }

            _appClientId = appClientId;

            if (accountProviderType == AccountProviderType.Msal)
            {
                _scopes = new string[] { "https://graph.microsoft.com/Files.ReadWrite" };
            }

            _isInitialized = true;
            _accountProviderType = accountProviderType;
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> LoginAsync()
        {
            _isConnected = false;

            if (!_isInitialized)
            {
                throw new InvalidOperationException("Microsoft OneDrive not initialized.");
            }

            if (_accountProviderType == AccountProviderType.Msal)
            {
                _accountProvider = GraphOneDriveAuthenticationHelper.CreateMsalAuthenticationProvider(_appClientId, _scopes);
                await GraphOneDriveAuthenticationHelper.AuthenticateMsalUserAsync(_scopes);
            }
            else
            {
                GraphOneDriveAuthenticationHelper.ResourceUri = "https://graph.microsoft.com/";
                _accountProvider = GraphOneDriveAuthenticationHelper.CreateAdalAuthenticationProvider(_appClientId);
                await GraphOneDriveAuthenticationHelper.AuthenticateAdalUserAsync();
            }

            _oneDriveProvider = new GraphServiceClient("https://graph.microsoft.com/v1.0/me", _accountProvider);

            _isConnected = true;
            return _isConnected;
        }

        /// <inheritdoc/>
        public async Task LogoutAsync()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Microsoft OneDrive not initialized.");
            }

            if (_accountProvider != null)
            {
                if (_accountProviderType == AccountProviderType.Adal)
                {
                    GraphOneDriveAuthenticationHelper.AzureAdContext.TokenCache.Clear();
                }
                else if (_accountProviderType == AccountProviderType.Msal)
                {
                    IUser user = GraphOneDriveAuthenticationHelper.IdentityClient.Users.First();
                    GraphOneDriveAuthenticationHelper.IdentityClient.Remove(user);
                }
            }
        }

        /// <inheritdoc/>
        public async Task<IOneDriveStorageFolder> AppRootFolderAsync()
        {
            if (_isConnected == false)
            {
                throw new Exception("You are not authenticate.");
            }

            var graphRootItem = await _oneDriveProvider.Drive.Special.AppRoot.Request().GetAsync();
            return new GraphOneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Special.AppRoot, graphRootItem);
        }

        /// <inheritdoc/>
        public Task<IOneDriveStorageFolder> CameraRollFolderAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IOneDriveStorageFolder> DocumentsFolderAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IOneDriveStorageFolder> MusicFolderAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IOneDriveStorageFolder> PhotosFolderAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<IOneDriveStorageFolder> RootFolderAsync()
        {
            if (_isConnected == false)
            {
                    throw new Exception("You are not authenticate.");
            }

            var graphRootItem = await _oneDriveProvider.Drive.Root.Request().GetAsync();
            return new GraphOneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Root, graphRootItem);
        }
    }
}
