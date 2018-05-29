// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public class GitHubRelease
    {
        [JsonProperty("published_at")]
        public DateTime Published { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("tag_name")]
        public string Tag { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public string FullName => $"Version {Name.Replace("v", string.Empty)} notes";

        [JsonProperty("draft")]
        public bool IsDraft { get; set; }

        [JsonProperty("html_url")]
        public string Url { get; set; }
    }
}
