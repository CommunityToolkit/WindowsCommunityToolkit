// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    public class TwitterPlacePoint
    {
        [JsonExtensionData]
        public object TheDaters { get; set; }
    }
}