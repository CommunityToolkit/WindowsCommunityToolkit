// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
