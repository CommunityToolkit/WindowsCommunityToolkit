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
    /// represents Describe Image request result
    /// </summary>
    public class ImageDescription
    {
        /// <summary>
        /// Gets or sets Image Description
        /// </summary>
        [JsonProperty("description")]
        public Description Description { get; set; }

        /// <summary>
        /// Gets or sets Request id
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets Image Metadata
        /// </summary>
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return VisionServiceJsonHelper.JsonSerialize(this);
        }
    }
}
