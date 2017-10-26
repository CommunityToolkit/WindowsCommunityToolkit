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
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    ///  Class using OneDrive API
    /// </summary>
    public class OneDriveService : IOneDriveService
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
        private IOneDriveClient _oneDriveProvider = null;

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
            return Initialize(null, AccountProviderType.OnlineId, scopes);
        }

        /// <inheritdoc/>
        public bool Initialize(string appClientId, AccountProviderType accountProviderType = AccountProviderType.OnlineId, OneDriveScopes scopes = OneDriveScopes.OfflineAccess | OneDriveScopes.ReadWrite)
        {
            if (accountProviderType == AccountProviderType.Adal || accountProviderType == AccountProviderType.Msal)
            {
                _instance = new GraphOneDriveService();
                return _instance.Initialize(appClientId, accountProviderType, scopes);
            }

            if (accountProviderType != AccountProviderType.OnlineId && string.IsNullOrEmpty(appClientId))
            {
                throw new ArgumentNullException(nameof(appClientId));
            }

            _appClientId = appClientId;

            if (accountProviderType == AccountProviderType.Msa)
            {
                _scopes = OneDriveHelper.TransformScopes(scopes);
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

            OneDriveAuthenticationHelper.ResourceUri = "https://api.onedrive.com/v1.0";
            if (_accountProviderType == AccountProviderType.Msa)
            {
                _accountProvider = OneDriveAuthenticationHelper.CreateMSAAuthenticationProvider(_appClientId, _scopes);
                await OneDriveAuthenticationHelper.AuthenticateMsaUserAsync();
            }
            else if (_accountProviderType == AccountProviderType.OnlineId)
            {
                _accountProvider = new OnlineIdAuthenticationProvider(_scopes);
                await ((OnlineIdAuthenticationProvider)_accountProvider).RestoreMostRecentFromCacheOrAuthenticateUserAsync();
            }

            _oneDriveProvider = new OneDriveClient(OneDriveAuthenticationHelper.ResourceUri, _accountProvider);
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
                if (_accountProviderType == AccountProviderType.OnlineId || _accountProviderType == AccountProviderType.Msa)
                {
                    await ((MsaAuthenticationProvider)_accountProvider).SignOutAsync();
                }
            }
        }

        /// <inheritdoc/>
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

            var oneDriveRootItem = await _oneDriveProvider.Drive.Root.Request().GetAsync();

            return new OneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Root, oneDriveRootItem.CopyToDriveItem());
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
                    var requestMessage = ((IOneDriveClient)Provider).Drive.Items[oneDriveId].Content.Request().GetHttpRequestMessage();
                    await Provider.AuthenticationProvider.AuthenticateRequestAsync(requestMessage).AsAsyncAction().AsTask(cancellationToken);
                    var downloader = completionGroup == null ? new BackgroundDownloader() : new BackgroundDownloader(completionGroup);
                    foreach (var item in requestMessage.Headers)
                    {
                        downloader.SetRequestHeader(item.Key, item.Value.First());
                    }
                    return downloader.CreateDownload(requestMessage.RequestUri, destinationFile);
                }, cancellationToken);
        }

        /// <inheritdoc/>
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

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.AppRoot.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Special.AppRoot, oneDriveRootItem.CopyToDriveItem());
        }

        /// <inheritdoc/>
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

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.CameraRoll.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Special.CameraRoll, oneDriveRootItem.CopyToDriveItem());
        }

        /// <inheritdoc/>
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

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.Documents.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Special.Documents, oneDriveRootItem.CopyToDriveItem());
        }

        /// <inheritdoc/>
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

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.Music.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Special.Music, oneDriveRootItem.CopyToDriveItem());
        }

        /// <inheritdoc/>
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

            var oneDriveRootItem = await _oneDriveProvider.Drive.Special.Photos.Request().GetAsync();
            return new OneDriveStorageFolder(_oneDriveProvider, (IBaseRequestBuilder)_oneDriveProvider.Drive.Special.Photos, oneDriveRootItem.CopyToDriveItem());
        }
    }
}
