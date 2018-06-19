// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Services.Twitter
{
    class TwitterCoordinates
    {
        [JsonProperty("coordinates")]
        public long[] Coordinates { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}