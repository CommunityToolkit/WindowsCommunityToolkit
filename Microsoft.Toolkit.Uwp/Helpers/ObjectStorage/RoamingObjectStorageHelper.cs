// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Store data in the Roaming environment (shared around all user devices)
    /// </summary>
    public class RoamingObjectStorageHelper : BaseObjectStorageHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoamingObjectStorageHelper"/> class.
        /// </summary>
        public RoamingObjectStorageHelper()
        {
            Settings = ApplicationData.Current.RoamingSettings;
            Folder = ApplicationData.Current.RoamingFolder;
        }
    }
}
