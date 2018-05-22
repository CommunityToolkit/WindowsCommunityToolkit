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
using Windows.Security.Credentials;
using Windows.Storage;

namespace Microsoft.Toolkit.Services.OneDrive.Uwp
{
    /// <summary>
    /// UserInfoSettings type
    /// </summary>
    public class UserInfoSettings
    {
        /// <summary>
        /// Storage key name for access token.
        /// </summary>
        private static readonly string USERINFOSETTINGS = "UserInfoSettings";

        /// <summary>
        /// Storage key name for access token.
        /// </summary>
        private static readonly string STORAGEKEYACCESSTOKEN = "AccessToken";

        /// <summary>
        /// Storage key name for token expiration.
        /// </summary>
        private static readonly string STORAGEKEYEXPIRATION = "Expiration";

        /// <summary>
        /// Storage key name for user name.
        /// </summary>
        private static readonly string STORAGEKEYUSER = "User";

        /// <summary>
        /// Password vault used to store access tokens
        /// </summary>
        private static PasswordVault _vault;

        /// <summary>
        /// Password Credential
        /// </summary>
        private static PasswordCredential _passwordCredential;

        /// <summary>
        /// Application data container
        /// </summary>
        private static ApplicationDataContainer _container = null;

        /// <summary>
        /// Gets or sets user's UPN
        /// </summary>
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// Gets or sets expiration of access token
        /// </summary>
        public DateTimeOffset Expiration { get; set; }

        /// <summary>
        /// Gets or sets user's access token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoSettings"/> class.
        /// </summary>
        public UserInfoSettings()
        {
            _container = ApplicationData.Current.LocalSettings.CreateContainer(USERINFOSETTINGS, ApplicationDataCreateDisposition.Always);
        }

        /// <summary>
        /// Load user's settings if it exists
        /// </summary>
        /// <returns>User's settings or null</returns>
        public static UserInfoSettings Load()
        {
            _vault = new PasswordVault();
            _container = ApplicationData.Current.LocalSettings.CreateContainer(USERINFOSETTINGS, ApplicationDataCreateDisposition.Always);
            if (_container.Values.ContainsKey(STORAGEKEYEXPIRATION) && _container.Values.ContainsKey(STORAGEKEYUSER))
            {
                var expiration = (DateTimeOffset)_container.Values[STORAGEKEYEXPIRATION];
                var userPrincipal = _container.Values[STORAGEKEYUSER].ToString();
                _passwordCredential = _vault.Retrieve(STORAGEKEYACCESSTOKEN, userPrincipal);
                return new UserInfoSettings { UserPrincipalName = userPrincipal, Expiration = expiration, AccessToken = _passwordCredential.Password };
            }

            return null;
        }

        /// <summary>
        /// Clear user's settings
        /// </summary>
        internal static void Clear()
        {
            _container.Values[STORAGEKEYEXPIRATION] = null;
            _container.Values[STORAGEKEYUSER] = null;
            if (_vault != null && _passwordCredential != null)
            {
                _vault.Remove(_passwordCredential);
            }
        }

        /// <summary>
        /// Save user's settings
        /// </summary>
        internal void Save()
        {
            _container.Values[STORAGEKEYEXPIRATION] = Expiration;
            _container.Values[STORAGEKEYUSER] = UserPrincipalName;
            _passwordCredential = new PasswordCredential(STORAGEKEYACCESSTOKEN, UserPrincipalName, AccessToken);
            _vault.Add(_passwordCredential);
        }
    }
}
