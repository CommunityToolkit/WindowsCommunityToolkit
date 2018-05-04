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
using Microsoft.Identity.Client;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Microsoft.Toolkit.Services.MicrosoftGraph.Platform;
using Microsoft.Toolkit.Services.OneDrive.Platform;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    ///  Class using OneDrive API
    /// </summary>
    public class OneDriveService
    {
        /// <summary>
        /// Gets or sets platform initializer.
        /// </summary>
        public static IOneDriveServicePlatformInitializer ServicePlatformInitializer { get; set; }

        /// <summary>
        /// Gets or sets platform implementation of background download service.
        /// </summary>
        public IOneDriveServicePlatform ServicePlatformService { get; set; }

        /// <summary>
        /// Private field for singleton.
        /// </summary>
        private static OneDriveService _instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static OneDriveService Instance => _instance ?? (_instance = new OneDriveService());

        /// <summary>
        /// Gets or sets AppClientId.
        /// </summary>
        protected string AppClientId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether service is initialized.
        /// </summary>
        protected static bool IsInitialized { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user is connected.
        /// </summary>
        protected bool IsConnected { get; set; }

        /// <summary>
        /// Gets or sets permission scopes.
        /// </summary>
        protected string[] Scopes { get; set; }

        /// <summary>
        /// Gets a reference to an instance of the underlying data provider.
        /// </summary>
        public MicrosoftGraphService Provider
        {
            get
            {
                if (MicrosoftGraphService.Instance == null)
                {
                    throw new InvalidOperationException("Provider not initialized.");
                }

                return MicrosoftGraphService.Instance;
            }
        }

        /// <summary>
        /// Intializes OneDrive service.
        /// </summary>
        /// <typeparam name="T">Concrete instance of type IOneDriveServicePlatformInitializer</typeparam>
        /// <typeparam name="U">Concrete instance of type IMicrosoftGraphUserServicePhotos</typeparam>
        /// <param name="appClientId">Client Id.</param>
        /// <param name="scopes">Permission scopes.</param>
        /// <param name="uiParent">UiParent instance - required for Android</param>
        /// <param name="redirectUri">Redirect Uri - required for Android</param>
        /// <returns>True or false.</returns>
        public virtual bool Initialize<T, U>(string appClientId, string[] scopes, UIParent uiParent = null, string redirectUri = null)
            where T : IOneDriveServicePlatformInitializer, new()
            where U : IMicrosoftGraphUserServicePhotos, new()
        {
            ServicePlatformInitializer = new T();
            ServicePlatformService = ServicePlatformInitializer.CreateOneDriveServicePlatformInstance(this);

            AppClientId = appClientId;
            Scopes = scopes;
            IsInitialized = true;

            Provider.Initialize<U>(appClientId, MicrosoftGraphEnums.ServicesToInitialize.OneDrive, scopes, uiParent, redirectUri);

            if (Provider.Authentication == null)
            {
                Provider.Authentication = new MicrosoftGraphAuthenticationHelper(Scopes);
            }

            return true;
        }

        /// <summary>
        /// Intializes OneDrive service.
        /// </summary>
        /// <param name="appClientId">Client Id.</param>
        /// <param name="scopes">Permission scopes.</param>
        /// <param name="uiParent">UiParent instance - required for Android</param>
        /// <param name="redirectUri">Redirect Uri - required for Android</param>
        /// <returns>True or false.</returns>
        public virtual bool Initialize(string appClientId, string[] scopes, UIParent uiParent = null, string redirectUri = null)
        {
            ServicePlatformService = ServicePlatformInitializer.CreateOneDriveServicePlatformInstance(this);

            AppClientId = appClientId;
            Scopes = scopes;
            IsInitialized = true;

            Provider.Initialize(appClientId, MicrosoftGraphEnums.ServicesToInitialize.OneDrive, scopes, uiParent, redirectUri);

            if (Provider.Authentication == null)
            {
                Provider.Authentication = new MicrosoftGraphAuthenticationHelper(Scopes);
            }

            return true;
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <returns>success or failure</returns>
        public virtual async Task LogoutAsync()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft OneDrive not initialized.");
            }

            if (Provider != null)
            {
                await Provider.Logout();
            }
        }

        /// <summary>
        /// Signs in the user
        /// </summary>
        /// <returns>Returns success or failure of login attempt.</returns>
        public virtual async Task<bool> LoginAsync()
        {
            IsConnected = false;

            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft OneDrive not initialized.");
            }

            await Provider.LoginAsync();

            IsConnected = true;
            return IsConnected;
        }

        /// <summary>
        /// Gets the OneDrive root folder for Me
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public virtual async Task<OneDriveStorageFolder> RootFolderForMeAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (IsConnected == false)
            {
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            var oneDriveRootItem = await Provider.GraphProvider.Me.Drive.Root.Request().GetAsync();
            var oneDriveItem = await Provider.GraphProvider.Me.Drive.Root.Children.Request().GetAsync();

            return new OneDriveStorageFolder(Provider.GraphProvider, Provider.GraphProvider.Me.Drive.Root, oneDriveRootItem);
        }

        /// <summary>
        /// Gets the OneDrive app root folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public virtual async Task<OneDriveStorageFolder> AppRootFolderAsync()
        {
            // log the user silently with a Microsoft Account associate to Windows
            if (IsConnected == false)
            {
                if (!await OneDriveService.Instance.LoginAsync())
                {
                    throw new Exception("Unable to sign in");
                }
            }

            var oneDriveRootItem = await Provider.GraphProvider.Drive.Special.AppRoot.Request().GetAsync();
            return new OneDriveStorageFolder(Provider.GraphProvider, Provider.GraphProvider.Drive.Special.AppRoot, oneDriveRootItem);
        }

        /// <summary>
        /// Gets the OneDrive camera roll folder - not yet supported.
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public virtual Task<OneDriveStorageFolder> CameraRollFolderAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the OneDrive documents folder - not yet supported.
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public virtual Task<OneDriveStorageFolder> DocumentsFolderAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the OneDrive music folder - not yet supported.
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public virtual Task<OneDriveStorageFolder> MusicFolderAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the OneDrive photos folder - not yet supported.
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public virtual Task<OneDriveStorageFolder> PhotosFolderAsync()
        {
            throw new NotImplementedException();
        }
    }
}
