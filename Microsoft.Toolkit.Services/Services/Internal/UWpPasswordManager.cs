// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Services.Core;
using Windows.Security.Credentials;
using Windows.Storage;

namespace Microsoft.Toolkit.Services.Internal
{

    /// <summary>
    /// Password Manager
    /// </summary>
    internal class UWpPasswordManager : IPasswordManager
    {
        /// <summary>
        /// Password vault used to store access tokens
        /// </summary>
        private readonly PasswordVault _vault;

        /// <summary>
        /// Initializes a new instance of the <see cref="UWpPasswordManager"/> class.
        /// </summary>
        public UWpPasswordManager()
        {
            _vault = new PasswordVault();
        }


        /// <inheritdoc/>
        public Toolkit.Services.Core.PasswordCredential Get(string key)
        {
#if WINRT
            var crendentials = RetrievePasswordCredential(key);
            if (crendentials == null)
            {
                return null;
            }

            return new Toolkit.Services.Core.PasswordCredential { Password = crendentials.Password, UserName = crendentials.UserName };
#endif

            return null;
        }

        private Windows.Security.Credentials.PasswordCredential RetrievePasswordCredential(string key)
        {
#if WINRT
            var passwordCredentials = _vault.RetrieveAll();
            var temp = passwordCredentials.FirstOrDefault(c => c.Resource == key);

            if (temp == null)
            {
                return null;
            }

            return _vault.Retrieve(temp.Resource, temp.UserName);
#endif

            return null;
        }

        /// <inheritdoc/>
        public void Remove(string key)
        {
            #if WINRT
            _vault.Remove(RetrievePasswordCredential(key));
            #endif
        }

        /// <inheritdoc/>
        public void Store(string resource, Toolkit.Services.Core.PasswordCredential credentials)
        {
            #if WINRT
            var passwordCredential = new Windows.Security.Credentials.PasswordCredential(resource, credentials.UserName, credentials.Password);
            _vault.Add(passwordCredential);
            #endif
        }
    }
}
