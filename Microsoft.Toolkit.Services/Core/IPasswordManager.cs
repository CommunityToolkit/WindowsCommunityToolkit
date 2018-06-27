// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.Core
{
    /// <summary>
    /// This interface gets a PasswordCredential, store the credential and remove the key.
    /// </summary>
    public interface IPasswordManager
    {
        /// <summary>
        /// Gets the user credentials.
        /// </summary>
        /// <param name="key"> Receive the storage key user and the access token </param>
        /// <returns> Returns user credential.</returns>
        PasswordCredential Get(string key);

        /// <summary>
        /// Store users credential.
        /// </summary>
        /// <param name="resource"> Resource</param>
        /// <param name="credential"> Username and password.</param>
        void Store(string resource, PasswordCredential credential);

        /// <summary>
        /// Remove users credential.
        /// </summary>
        /// <param name="key"> Credential unique key</param>
        void Remove(string key);
    }
}
