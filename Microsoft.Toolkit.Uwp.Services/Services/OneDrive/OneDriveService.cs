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

using Microsoft.Identity.Client;
using Microsoft.Toolkit.Uwp.Services.OneDrive.Platform;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    ///  Class using OneDrive in a UWP application
    /// </summary>
    public class OneDriveService : Microsoft.Toolkit.Services.OneDrive.OneDriveService
    {
        /// <summary>
        /// Private field for singleton.
        /// </summary>
        private static OneDriveService _instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static new OneDriveService Instance => _instance ?? (_instance = new OneDriveService());

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
        public override bool Initialize<T, U>(string appClientId, string[] scopes, UIParent uiParent = null, string redirectUri = null)
        {

            Microsoft.Toolkit.Services.OneDrive.OneDriveService.ServicePlatformInitializer = new T();

            return base.Initialize<T, U>(appClientId, scopes, uiParent, redirectUri);
        }

        /// <summary>
        /// Intializes OneDrive service.
        /// </summary>
        /// <param name="appClientId">Client Id.</param>
        /// <param name="scopes">Permission scopes.</param>
        /// <param name="uiParent">UiParent instance - required for Android</param>
        /// <param name="redirectUri">Redirect Uri - required for Android</param>
        /// <returns>True or false.</returns>
        public override bool Initialize(string appClientId, string[] scopes, UIParent uiParent = null, string redirectUri = null)
        {
            Microsoft.Toolkit.Services.OneDrive.OneDriveService.ServicePlatformInitializer = new OneDriveServicePlatformInitializer();

            return base.Initialize(appClientId, scopes, uiParent, redirectUri);
        }
    }
}
