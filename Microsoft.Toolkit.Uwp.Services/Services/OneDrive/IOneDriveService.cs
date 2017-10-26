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

using System.Threading.Tasks;
using Microsoft.Graph;
using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// IOneDriveService
    /// </summary>
    public interface IOneDriveService
    {
        /// <summary>
        /// Gets a reference to an instance of the underlying data provider.
        /// </summary>
        IBaseClient Provider { get; }

        /// <summary>
        /// Signs in the user
        /// </summary>
        /// <returns>Returns success or failure of login attempt.</returns>
        Task<bool> LoginAsync();

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <returns>success or failure</returns>
        Task LogoutAsync();

        /// <summary>
        /// Gets the OneDrive root folder
        /// </summary>
        /// <returns>When this method completes, it returns a IOneDriveStorageFolder</returns>
        Task<IOneDriveStorageFolder> RootFolderAsync();

        /// <summary>
        /// Gets the OneDrive app root folder
        /// </summary>
        /// <returns>When this method completes, it returns a IOneDriveStorageFolder</returns>
        Task<IOneDriveStorageFolder> AppRootFolderAsync();

        /// <summary>
        /// Gets the OneDrive camera roll folder
        /// </summary>
        /// <returns>When this method completes, it returns a IOneDriveStorageFolder</returns>
        Task<IOneDriveStorageFolder> CameraRollFolderAsync();

        /// <summary>
        /// Gets the OneDrive documents folder
        /// </summary>
        /// <returns>When this method completes, it returns a IOneDriveStorageFolder</returns>
        Task<IOneDriveStorageFolder> DocumentsFolderAsync();

        /// <summary>
        /// Gets the OneDrive music folder
        /// </summary>
        /// <returns>When this method completes, it returns a IOneDriveStorageFolder</returns>
        Task<IOneDriveStorageFolder> MusicFolderAsync();

        /// <summary>
        /// Gets the OneDrive photos folder
        /// </summary>
        /// <returns>When this method completes, it returns a IOneDriveStorageFolder</returns>
        Task<IOneDriveStorageFolder> PhotosFolderAsync();

        /// <summary>
        /// Initialize OneDrive Service only for OnlineId Provider
        /// </summary>
        /// <param name="scopes">Scopes represent various permission levels that an app can request from a user</param>
        /// <returns>Success or failure</returns>
        bool Initialize(OneDriveScopes scopes = OneDriveScopes.ReadWrite | OneDriveScopes.OfflineAccess);

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
        bool Initialize(string appClientId, AccountProviderType accountProviderType, OneDriveScopes scopes = OneDriveScopes.ReadWrite | OneDriveScopes.OfflineAccess);
    }
}
