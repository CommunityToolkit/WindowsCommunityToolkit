// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public class LandingPageResource
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("links")]
        public LandingPageLink[] Links { get; set; }
    }
}