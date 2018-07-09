// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
