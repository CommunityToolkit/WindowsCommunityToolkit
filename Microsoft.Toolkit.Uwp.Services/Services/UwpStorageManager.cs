// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Services.Core;
using Windows.Security.Credentials;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Services
{

    public class UwpStorageManager : IStorageManager
    {
        public string Get(string key)
        {
            return ApplicationData.Current.LocalSettings.Values[key]?.ToString();
        }

        public void Set(string key, string value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }
    }

}
