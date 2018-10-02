// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public class LandingPageLinks
    {
        [JsonProperty("new-section-title")]
        public string NewSectionTitle { get; set; }

        [JsonProperty("new-samples")]
        public string[] NewSamples { get; set; }

        [JsonProperty("resources")]
        public LandingPageResource[] Resources { get; set; }
    }
}
