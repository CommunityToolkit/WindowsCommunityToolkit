// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Store data in the Local environment (only on the current device)
    /// </summary>
    public class LocalObjectStorageHelper : BaseObjectStorageHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalObjectStorageHelper"/> class.
        /// </summary>
        public LocalObjectStorageHelper()
        {
            Settings = ApplicationData.Current.LocalSettings;
            Folder = ApplicationData.Current.LocalFolder;
        }
    }
}
