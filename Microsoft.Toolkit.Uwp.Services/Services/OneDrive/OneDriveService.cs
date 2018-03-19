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
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using Microsoft.Toolkit.Uwp.Services.OneDrive.Platform;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    ///  Class using OneDrive API
    /// </summary>
    [Obsolete("This class is being deprecated. Please use the .NET Standard Library counterpart found in Microsoft.Toolkit.Services.")]
    public class OneDriveService : Toolkit.Services.OneDrive.OneDriveService
    {
        /// <summary>
        /// Private field for singleton.
        /// </summary>
        private static OneDriveService _instance;

        /// <summary>
        /// Gets public singleton - legacy instance that depends on OneDrive SDK - still required for OnlineId support. Consider using GraphInstance.
        /// </summary>
        public static new OneDriveService Instance => _instance ?? (_instance = new OneDriveService());

        /// <summary>
        /// Private field for singleton.
        /// </summary>
        private static Toolkit.Services.OneDrive.OneDriveService _graphInstance;

        /// <summary>
        /// Private field for OnlineIdAuthenticationProvider.PromptType.
        /// </summary>
        private OnlineIdAuthenticationProvider.PromptType _onlineIdPromptType = OnlineIdAuthenticationProvider.PromptType.PromptIfNeeded;

        /// <summary>
        /// Gets public singleton property - new version that depends on Graph service / SDK - only supports ADAL v2 endpoint.
        /// </summary>
        public static Toolkit.Services.OneDrive.OneDriveService GraphInstance
        {
            get
            {
                if (_graphInstance == null)
                {
                    ServicePlatformInitializer = new OneDriveServicePlatformInitializer();
                    _graphInstance = new Toolkit.Services.OneDrive.OneDriveService();
                }

                return _graphInstance;
            }
        }

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
        private IOneDriveClient _oneDriveProvider;

        /// <summary>
        /// Gets a reference to an instance of the underlying data provider.
        /// </summary>
        public new IOneDriveClient Provider
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
        /// <param name="onlineIdPromptType">Specify whether to prompt for credentials. Set to DoNotPrompt if calling from a background task.</param>
        /// <returns>Success or failure</returns>
        public bool Initialize(OnlineIdAuthenticationProvider.PromptType onlineIdPromptType)
        {
            OneDriveScopes scopes = OneDriveScopes.ReadWrite | OneDriveScopes.OfflineAccess;
            _onlineIdPromptType = onlineIdPromptType;
            return Initialize(scopes, _onlineIdPromptType);
        }

        /// <summary>
        /// Initialize OneDrive Service only for OnlineId Provider
        /// </summary>
        /// <param name="scopes">Scopes represent various permission levels that an app can request from a user</param>
        /// <param name="onlineIdPromptType">Specify whether to prompt for credentials. Set to DoNotPrompt if calling from a background task.</param>
        /// <returns>Success or failure</returns>
        public bool Initialize(OneDriveScopes scopes = OneDriveScopes.ReadWrite | OneDriveScopes.OfflineAccess, OnlineIdAuthenticationProvider.PromptType onlineIdPromptType = OnlineIdAuthenticationProvider.PromptType.PromptIfNeeded)
        {
            _onlineIdPromptType = onlineIdPromptType;
            return Initialize(null, AccountProviderType.OnlineId, scopes, _onlineIdPromptType);
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
        /// <param name="onlineIdPromptType">Specify whether to prompt for credentials. Set to DoNotPrompt if calling from a background task.</param>
        /// <remarks>If AccountProvider</remarks>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string appClientId, AccountProviderType accountProviderType = AccountProviderType.OnlineId, OneDriveScopes scopes = OneDriveScopes.ReadWrite | OneDriveScopes.OfflineAccess, OnlineIdAuthenticationProvider.PromptType onlineIdPromptType = OnlineIdAuthenticationProvider.PromptType.PromptIfNeeded)
        {
            _onlineIdPromptType = onlineIdPromptType;

            if (accountProviderType != AccountProviderType.OnlineId && string.IsNullOrEmpty(appClientId))
            {
                throw new ArgumentNullException(nameof(appClientId));
            }

            AppClientId = appClientId;

            if (accountProviderType != AccountProviderType.Adal)
            {
                Scopes = OneDriveHelper.TransformScopes(scopes);
            }

            IsInitialized = true;
            _accountProviderType = accountProviderType;
            return true;
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <returns>success or failure</returns>
        public override async Task LogoutAsync()
        {
            if (!IsInitialized)
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
                    DiscoverySettings.Clear();
                    UserInfoSettings.Clear();
                }
            }
        }

        /// <summary>
        /// Signs in the user
        /// </summary>
        /// <returns>Returns success or failure of login attempt.</returns>
        public override async Task<bool> LoginAsync()
        {
            IsConnected = false;

            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft OneDrive not initialized.");
            }

            string resourceEndpointUri = null;

            if (_accountProviderType == AccountProviderType.Adal)
            {
                DiscoveryService discoveryService = null;
                DiscoverySettings discoverySettings = DiscoverySettings.Load();

                if (discoverySettings == null)
                {
                    // For OneDrive for business only
                    var authDiscoveryResult = await OneDriveAuthenticationHelper.AuthenticateAdalUserForDiscoveryAsync(AppClientId);
                    discoveryService = await OneDriveAuthenticationHelper.GetUserServiceResource(authDiscoveryResult);
                    discoverySettings = new DiscoverySettings { ServiceEndpointUri = discoveryService.ServiceEndpointUri, ServiceResourceId = discoveryService.ServiceResourceId };
                    discoverySettings.Save();
                }

                OneDriveAuthenticationHelper.ResourceUri = discoverySettings.ServiceResourceId;
                _accountProvider = OneDriveAuthenticationHelper.CreateAdalAuthenticationProvider(AppClientId);
                await OneDriveAuthenticationHelper.AuthenticateAdalUserAsync(true);
                resourceEndpointUri = discoverySettings.ServiceEndpointUri;
            }
            else if (_accountProviderType == AccountProviderType.Msa)
            {
                OneDriveAuthenticationHelper.ResourceUri = "https://api.onedrive.com/v1.0";
                _accountProvider = OneDriveAuthenticationHelper.CreateMSAAuthenticationProvider(AppClientId, Scopes);

                await ((MsaAuthenticationProvider)OneDriveAuthenticationHelper.AuthenticationProvider).RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                resourceEndpointUri = OneDriveAuthenticationHelper.ResourceUri;
            }
            else if (_accountProviderType == AccountProviderType.OnlineId)
            {
                OneDriveAuthenticationHelper.ResourceUri = "https://api.onedrive.com/v1.0";
                _accountProvider = new OnlineIdAuthenticationProvider(Scopes, _onlineIdPromptType);
                await ((MsaAuthenticationProvider)_accountProvider).RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                resourceEndpointUri = OneDriveAuthenticationHelper.ResourceUri;
            }

            _oneDriveProvider = new OneDriveClient(resourceEndpointUri, _accountProvider);

            IsConnected = true;
            return IsConnected;
        }

        /// <summary>
        /// Gets the OneDrive root folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public new async Task<OneDriveStorageFolder> RootFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (IsConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            var oneDriveRootItem = await _oneDriveProvider.Drive.Root.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, _oneDriveProvider.Drive.Root, oneDriveRootItem);
        }

        /// <summary>
        /// Gets the OneDrive app root folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public new async Task<OneDriveStorageFolder> AppRootFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (IsConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.AppRoot.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, _oneDriveProvider.Drive.Special.AppRoot, oneDriveRootItem);
        }

        /// <summary>
        /// Gets the OneDrive camera roll folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public new async Task<OneDriveStorageFolder> CameraRollFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (IsConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.CameraRoll.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, _oneDriveProvider.Drive.Special.CameraRoll, oneDriveRootItem);
        }

        /// <summary>
        /// Gets the OneDrive documents folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public new async Task<OneDriveStorageFolder> DocumentsFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (IsConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.Documents.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, _oneDriveProvider.Drive.Special.Documents, oneDriveRootItem);
        }

        /// <summary>
        /// Gets the OneDrive music folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public new async Task<OneDriveStorageFolder> MusicFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (IsConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.Music.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, _oneDriveProvider.Drive.Special.Music, oneDriveRootItem);
        }

        /// <summary>
        /// Gets the OneDrive photos folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public new async Task<OneDriveStorageFolder> PhotosFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (IsConnected == false)
            {
                OneDriveService.Instance.Initialize();
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.Photos.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, _oneDriveProvider.Drive.Special.Photos, oneDriveRootItem);
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
            if (IsConnected == false)
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
