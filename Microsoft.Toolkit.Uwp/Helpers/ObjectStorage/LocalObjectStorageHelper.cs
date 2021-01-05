// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.Serialization;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Store data in the Local environment (only on the current device).
    /// </summary>
    public class LocalObjectStorageHelper : BaseObjectStorageHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalObjectStorageHelper"/> class,
        /// which can read and write data using the provided <see cref="IObjectSerializer"/>;
        /// if none is provided, a default Json serializer will be used (based on <see cref="DataContractSerializer"/>).
        /// In 6.1 and older the default Serializer was based on Newtonsoft.Json and the new default Serializer may behave differently.
        /// To implement a <see cref="IObjectSerializer"/> based on Newtonsoft.Json or System.Text.Json see https://aka.ms/wct/storagehelper-migration
        /// </summary>
        /// <param name="objectSerializer">The serializer to use.</param>
        public LocalObjectStorageHelper(IObjectSerializer objectSerializer = null)
            : base(objectSerializer)
        {
            Settings = ApplicationData.Current.LocalSettings;
            Folder = ApplicationData.Current.LocalFolder;
        }
    }
}
