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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;

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
        private IOneDriveClient _oneDriveProvider = null;

        /// <summary>
        /// Store a reference to an instance of the underlying data provider.
        /// </summary>
        private IGraphServiceClient _graphProvider = null;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static OneDriveService Instance => _instance ?? (_instance = new OneDriveService());

        /// <summary>
        /// Gets a reference to an instance of the underlying graph provider.
        /// </summary>
        public IGraphServiceClient GraphProvider
        {
            get
            {
                if (_graphProvider == null)
                {
                    throw new InvalidOperationException("Provider not initialized.");
                }

                return _graphProvider;
            }
        }

        /// <summary>
        /// Gets a reference to an instance of the underlying data provider.
        /// </summary>
        public IOneDriveClient Provider
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

        /// <summary>
        /// Initialize OneDrive Service only for OnlineId Provider
        /// </summary>
        /// <param name="scopes">Scopes represent various permission levels that an app can request from a user</param>
        /// <returns>Success or failure</returns>
        public bool Initialize(OneDriveScopes scopes = OneDriveScopes.ReadWrite | OneDriveScopes.OfflineAccess)
        {
            return Initialize(null, AccountProviderType.OnlineId, scopes);
        }

        /// <summary>
        /// Initialize OneDrive service
        /// </summary>
        /// <param name="appClientId">Application Id Client. Could be null if AccountProviderType.OnlineId is used</param>
        /// <param name="accountProviderType">Account Provider type.
        /// <para>AccountProviderType.OnlineId: If the user is signed into a Windows system with a Microsoft Account, this user will be used for authentication request. Need to associate the App to the store</para>
        /// <para>AccountProviderType.Msa: Authenticate the user with a Microsoft Account. You need to register your app https://apps.dev.microsoft.com in the SDK Live section</para>
        /// <para>AccountProviderType.Adal: Authenticate the user with a Office 365 Account. You need to register your in Azure Active Directory</para></param>
        /// <param name="scopes">Scopes represent various permission levels that an app can request from a user. Could be null if AccountProviderType.Adal is used </param>
        /// <remarks>If AccountProvider</remarks>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string appClientId, AccountProviderType accountProviderType = AccountProviderType.OnlineId, OneDriveScopes scopes = OneDriveScopes.ReadWrite | OneDriveScopes.OfflineAccess)
        {
            if (accountProviderType != AccountProviderType.OnlineId && string.IsNullOrEmpty(appClientId))
            {
                throw new ArgumentNullException(nameof(appClientId));
            }

            _appClientId = appClientId;

            if (accountProviderType == AccountProviderType.Msa)
            {
                _scopes = OneDriveHelper.TransformScopes(scopes);
            }
            else if (accountProviderType == AccountProviderType.Msal)
            {
                _scopes = new string[] { "https://graph.microsoft.com/Files.ReadWrite" };
            }

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
                else if (_accountProviderType == AccountProviderType.Adal)
                {
                    OneDriveAuthenticationHelper.AzureAdContext.TokenCache.Clear();
                }
                else if (_accountProviderType == AccountProviderType.Msal)
                {
                    IUser user = OneDriveAuthenticationHelper.IdentityClient.Users.First();
                    OneDriveAuthenticationHelper.IdentityClient.Remove(user);
                }

                OneDriveAuthenticationHelper.ClearUserInfo();
            }
        }

        /// <summary>
        /// Signs in the user
        /// </summary>
        /// <returns>Returns success or failure of login attempt.</returns>
        public async Task<bool> LoginAsync()
        {
            _isConnected = false;

            if (!_isInitialized)
            {
                throw new InvalidOperationException("Microsoft OneDrive not initialized.");
            }

            string resourceEndpointUri = null;
            if (_accountProviderType == AccountProviderType.Msal)
            {
                _accountProvider = OneDriveAuthenticationHelper.CreateMsalAuthenticationProvider(_appClientId, _scopes);
                await OneDriveAuthenticationHelper.AuthenticateMsalUserAsync(_scopes);
            }

            // Keep this for compatibility reason
            else if (_accountProviderType == AccountProviderType.Adal)
            {
                OneDriveAuthenticationHelper.ResourceUri = "https://graph.microsoft.com/";
                _accountProvider = OneDriveAuthenticationHelper.CreateAdalAuthenticationProvider(_appClientId);
                await OneDriveAuthenticationHelper.AuthenticateAdalUserAsync(true);
            }
            else if (_accountProviderType == AccountProviderType.Msa)
            {
                OneDriveAuthenticationHelper.ResourceUri = "https://api.onedrive.com/v1.0";
                _accountProvider = OneDriveAuthenticationHelper.CreateMSAAuthenticationProvider(_appClientId, _scopes);

                await ((MsaAuthenticationProvider)OneDriveAuthenticationHelper.AuthenticationProvider).RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                resourceEndpointUri = OneDriveAuthenticationHelper.ResourceUri;
            }
            else if (_accountProviderType == AccountProviderType.OnlineId)
            {
                OneDriveAuthenticationHelper.ResourceUri = "https://api.onedrive.com/v1.0";
                _accountProvider = new OnlineIdAuthenticationProvider(_scopes);
                await ((MsaAuthenticationProvider)_accountProvider).RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                resourceEndpointUri = OneDriveAuthenticationHelper.ResourceUri;
            }

            if (_accountProviderType == AccountProviderType.Msal || _accountProviderType == AccountProviderType.Adal)
            {
                _graphProvider = new GraphServiceClient("https://graph.microsoft.com/v1.0/me", _accountProvider);
            }
            else
            {
                _oneDriveProvider = new OneDriveClient(resourceEndpointUri, _accountProvider);
            }

            _isConnected = true;
            return _isConnected;
        }

        /// <summary>
        /// Gets the OneDrive root folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public async Task<IOneDriveStorageFolder> RootFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (_isConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            if (_graphProvider != null)
            {
                var graphRootItem = await _graphProvider.Drive.Root.Request().GetAsync();
                return new GraphOneDriveStorageFolder(_graphProvider, (IBaseRequestBuilder)_graphProvider.Drive.Root, graphRootItem);
            }

            var oneDriveRootItem = await _oneDriveProvider.Drive.Root.Request().GetAsync();

            return new OneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Root, oneDriveRootItem.CopyToDriveItem());
        }

        /// <summary>
        /// Gets the OneDrive app root folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public async Task<IOneDriveStorageFolder> AppRootFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (_isConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            if (_graphProvider != null)
            {
                var graphRootItem = await _graphProvider.Drive.Special.AppRoot.Request().GetAsync();
                return new GraphOneDriveStorageFolder(_graphProvider, (IBaseRequestBuilder)_graphProvider.Drive.Special.AppRoot, graphRootItem);
            }

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.AppRoot.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Special.AppRoot, oneDriveRootItem.CopyToDriveItem());
        }

        /// <summary>
        /// Gets the OneDrive camera roll folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public async Task<IOneDriveStorageFolder> CameraRollFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (_isConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            if (_graphProvider != null)
            {
                throw new NotImplementedException();
            }

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.CameraRoll.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Special.CameraRoll, oneDriveRootItem.CopyToDriveItem());
        }

        /// <summary>
        /// Gets the OneDrive documents folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public async Task<IOneDriveStorageFolder> DocumentsFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (_isConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            if (_graphProvider != null)
            {
                throw new NotImplementedException();
            }

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.Documents.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Special.Documents, oneDriveRootItem.CopyToDriveItem());
        }

        /// <summary>
        /// Gets the OneDrive music folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public async Task<IOneDriveStorageFolder> MusicFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (_isConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            if (_graphProvider != null)
            {
                throw new NotImplementedException();
            }

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.Music.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Special.Music, oneDriveRootItem.CopyToDriveItem());
        }

        /// <summary>
        /// Gets the OneDrive photos folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public async Task<IOneDriveStorageFolder> PhotosFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (_isConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            if (_graphProvider != null)
            {
                throw new NotImplementedException();
            }

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.Photos.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Special.Photos, oneDriveRootItem.CopyToDriveItem());
        }

        /// <summary>
        /// Creates a background download for given OneDriveId
        /// </summary>
        /// <param name="oneDriveId">OneDrive item's Id</param>
        /// <param name="destinationFile">A <see cref="StorageFile"/> to which content will be downloaded</param>
        /// <param name="completionGroup">The <see cref="BackgroundTransferCompletionGroup"/> to which should <see cref="BackgroundDownloader"/> reffer to</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request</param>
        /// <returns>The created <see cref="DownloadOperation"/></returns>
        public async Task<DownloadOperation> CreateBackgroundDownloadForItemAsync(string oneDriveId, StorageFile destinationFile, BackgroundTransferCompletionGroup completionGroup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(oneDriveId))
            {
                throw new ArgumentNullException(nameof(oneDriveId));
            }

            if (destinationFile == null)
            {
                throw new ArgumentNullException(nameof(destinationFile));
            }

            // log the user silently with a Microsoft Account associate to Windows
            if (_isConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            return await Task.Run(
                async () =>
                {
                    var requestMessage = Provider.Drive.Items[oneDriveId].Content.Request().GetHttpRequestMessage();
                    await Provider.AuthenticationProvider.AuthenticateRequestAsync(requestMessage).AsAsyncAction().AsTask(cancellationToken);
                    var downloader = completionGroup == null ? new BackgroundDownloader() : new BackgroundDownloader(completionGroup);
                    foreach (var item in requestMessage.Headers)
                    {
                        downloader.SetRequestHeader(item.Key, item.Value.First());
                    }
                    return downloader.CreateDownload(requestMessage.RequestUri, destinationFile);
                }, cancellationToken);
        }
    }
}
