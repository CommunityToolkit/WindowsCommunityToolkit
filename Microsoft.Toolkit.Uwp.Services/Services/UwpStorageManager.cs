// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Services.Core;
using Windows.Security.Credentials;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Services
{
    /// <summary>
    /// Uwp specific implementation for IStorageManager using ApplicationData and LocalSettings
    /// </summary>
    public class UwpStorageManager : IStorageManager
    {
        /// <summary>
        /// Read the storage to return the key if exists if not null;
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <returns>Return string value if exists if not null</returns>
        public string Get(string key)
        {
            return ApplicationData.Current.LocalSettings.Values[key]?.ToString();
        }

        /// <summary>
        /// Save the value in the key inside the storage
        /// </summary>
        /// <param name="key">Key name in storage</param>
        /// <param name="value">Value associated to the storage</param>
        public void Set(string key, string value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }
    }
}
