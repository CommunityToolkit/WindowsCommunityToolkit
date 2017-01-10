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
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    ///  Class using OneDrive API
    /// </summary>
    public class OneDriveService
    {
        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static OneDriveService _instance;

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

        // TODO : change to private after debug
        /// <summary>
        /// Store a reference to an instance of the underlying data provider.
        /// </summary>
        public IOneDriveClient _oneDriveProvider;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static OneDriveService Instance => _instance ?? (_instance = new OneDriveService());

        /// <summary>
        /// Initialize OneDrive
        /// </summary>        
        /// <param name="appClientId">An App Id Client get from https://apps.dev.microsoft.com/</param>
        /// <param name="scopes">Scopes represent the various permission levels that an app can request from a user</param>
        /// <param name="accountProviderType">Account Provider</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string appClientId, string[] scopes, AccountProviderType accountProviderType = AccountProviderType.Msa)
        {

            if (accountProviderType != AccountProviderType.OnlineId && string.IsNullOrEmpty(appClientId))
            {
                throw new ArgumentNullException(nameof(appClientId));
            }

            if (accountProviderType != AccountProviderType.Adal && (scopes == null || scopes.Length == 0))
            {
                scopes = new string[] { "onedrive.readwrite" };
            }

            if (accountProviderType == AccountProviderType.OnlineId)
            {
                _accountProvider = new OnlineIdAuthenticationProvider(scopes);
            }

            if (accountProviderType == AccountProviderType.Msa)
            {
                _accountProvider = new MsaAuthenticationProvider(appClientId, "urn:ietf:wg:oauth:2.0:oob", scopes, new CredentialVault(appClientId));
            }

            _oneDriveProvider = new OneDriveClient("https://api.onedrive.com/v1.0", _accountProvider);

            _scopes = scopes;
            _isInitialized = true;
            _accountProviderType = accountProviderType;
            return true;
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <returns>success or failure</returns>
        public async Task LogoutAsync()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Microsoft OneDrive not initialized.");
            }

            if (_accountProvider != null)
            {
                if (_accountProviderType == AccountProviderType.OnlineId || _accountProviderType == AccountProviderType.Msa)
                {

                    await ((MsaAuthenticationProvider)_accountProvider).SignOutAsync();
                }
            }
        }

        /// <summary>
        /// Signs in the user
        /// </summary>
        /// <remarks></remarks>
        /// <returns>Returns success or failure of login attempt.</returns>
        public async Task<bool> LoginAsync()
        {
            _isConnected = false;

            if (!_isInitialized)
            {
                throw new InvalidOperationException("Microsoft OneDrive not initialized.");
            }

            if (_accountProviderType == AccountProviderType.Adal)
            {

                throw new NotImplementedException();
            }

            if (_accountProviderType == AccountProviderType.OnlineId || _accountProviderType == AccountProviderType.Msa)
            {
                await ((MsaAuthenticationProvider)_accountProvider).RestoreMostRecentFromCacheOrAuthenticateUserAsync();

                //await ((MsaAuthenticationProvider)_accountProvider).AuthenticateUserAsync();

            }

            _isConnected = true;
            return _isConnected;
        }

        /// <summary>
        /// Gets the OneDrive root folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public async Task<OneDriveStorageFolder> RootFolderAsync()
        {
           
            var oneDriveRootItem = await _oneDriveProvider.Drive.Root.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, _oneDriveProvider.Drive.Root, oneDriveRootItem);
        }
    }
}
