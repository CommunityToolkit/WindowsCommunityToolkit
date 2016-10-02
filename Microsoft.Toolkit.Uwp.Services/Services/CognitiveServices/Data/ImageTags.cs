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

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    /// <summary>
    /// Represets the result of Vision Service tag image request
    /// </summary>
    public class ImageTags
    {
        /// <summary>
        /// Gets or sets image tags
        /// </summary>
        [JsonProperty("tags")]
        public Tag[] Tags { get; set; }

        /// <summary>
        /// Gets or sets request id
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets request meta data
        /// </summary>
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return VisionServiceJsonHelper.JsonSerialize(this);
        }
    }

    /// <summary>
    /// Image tag request meta data
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// Gets or sets image width
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets image height
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets image format
        /// </summary>
        [JsonProperty("format")]
        public string Format { get; set; }
    }

    /// <summary>
    /// Represents image tag
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets tag name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets tag confidence
        /// </summary>
        [JsonProperty("confidence")]
        public float Confidence { get; set; }
    }
}
