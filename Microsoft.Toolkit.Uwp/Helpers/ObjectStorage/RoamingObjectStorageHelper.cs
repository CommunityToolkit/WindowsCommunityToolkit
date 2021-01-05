// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.Serialization;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Store data in the Roaming environment (shared across all user devices).
    /// </summary>
    public class RoamingObjectStorageHelper : BaseObjectStorageHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoamingObjectStorageHelper"/> class,
        /// which can read and write data using the provided <see cref="IObjectSerializer"/>;
        /// if none is provided, a default Json serializer will be used (based on <see cref="DataContractSerializer"/>).
        /// In 6.1 and older the defualt Serizlizer was based on Newtownsoft Json and the new default Serizlizer may behave diffrenly.
        /// To implement a <see cref="IObjectSerializer"/> based on Newtonsoft Json or System.Text.Json see https://aka.ms/wct/storagehelper-migration
        /// </summary>
        /// <param name="objectSerializer">The serializer to use.</param>
        public RoamingObjectStorageHelper(IObjectSerializer objectSerializer = null)
            : base(objectSerializer)
        {
            Settings = ApplicationData.Current.RoamingSettings;
            Folder = ApplicationData.Current.RoamingFolder;
        }
    }
}
