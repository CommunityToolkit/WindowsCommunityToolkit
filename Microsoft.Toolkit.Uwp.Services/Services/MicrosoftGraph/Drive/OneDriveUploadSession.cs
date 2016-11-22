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
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    /// The type OneDriveUploadSession.
    /// </summary>
    public class OneDriveUploadSession
    {
        /// <summary>
        /// Gets or sets additional data.
        /// </summary>
        [JsonExtensionData(ReadData = true)]
        public IDictionary<string, object> AdditionalData { get; set; }

        /// <summary>
        /// Gets or sets expirationDateTime.
        /// </summary>
        [DataMember(Name = "expirationDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTimeOffset? ExpirationDateTime { get; set; }

        /// <summary>
        /// Gets or sets nextExpectedRanges.
        /// </summary>
        [DataMember(Name = "nextExpectedRanges", EmitDefaultValue = false, IsRequired = false)]
        public IEnumerable<string> NextExpectedRanges { get; set; }

        /// <summary>
        /// Gets or sets uploadUrl.
        /// </summary>
        [DataMember(Name = "uploadUrl", EmitDefaultValue = false, IsRequired = false)]
        public string UploadUrl { get; set; }
    }
}
