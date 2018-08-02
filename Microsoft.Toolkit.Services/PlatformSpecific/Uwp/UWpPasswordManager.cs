// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Services.Core;
using Windows.Security.Credentials;

namespace Microsoft.Toolkit.Services.PlatformSpecific.Uwp
{
    /// <summary>
    /// Password Manager
    /// </summary>
    internal class UwpPasswordManager : IPasswordManager
    {
        /// <summary>
        /// Password vault used to store access tokens
        /// </summary>
        private readonly PasswordVault _vault;

        /// <summary>
        /// Initializes a new instance of the <see cref="UwpPasswordManager"/> class.
        /// </summary>
        public UwpPasswordManager()
        {
            _vault = new PasswordVault();
        }

        /// <inheritdoc/>
        public Toolkit.Services.Core.PasswordCredential Get(string key)
        {
            var crendentials = RetrievePasswordCredential(key);
            if (crendentials == null)
            {
                return null;
            }

            return new Toolkit.Services.Core.PasswordCredential { Password = crendentials.Password, UserName = crendentials.UserName };
        }

        private Windows.Security.Credentials.PasswordCredential RetrievePasswordCredential(string key)
        {
            var passwordCredentials = _vault.RetrieveAll();
            var temp = passwordCredentials.FirstOrDefault(c => c.Resource == key);

            if (temp == null)
            {
                return null;
            }

            return _vault.Retrieve(temp.Resource, temp.UserName);
        }

        /// <inheritdoc/>
        public void Remove(string key)
        {
            _vault.Remove(RetrievePasswordCredential(key));
        }

        /// <inheritdoc/>
        public void Store(string resource, Toolkit.Services.Core.PasswordCredential credentials)
        {
            var passwordCredential = new Windows.Security.Credentials.PasswordCredential(resource, credentials.UserName, credentials.Password);
            _vault.Add(passwordCredential);
        }
    }
}
