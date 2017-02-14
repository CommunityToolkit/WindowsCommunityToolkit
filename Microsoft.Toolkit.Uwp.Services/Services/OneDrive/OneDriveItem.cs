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

using System.IO;
using System.Runtime.Serialization;
using Microsoft.Graph;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// The type OneDriveItem.
    /// </summary>
    [DataContract]
    [JsonConverter(typeof(DerivedTypeConverter))]
    public class OneDriveItem
    {
        /// <summary>
        /// Gets or sets content.
        /// </summary>
        [DataMember(Name = "content", EmitDefaultValue = false, IsRequired = false)]
        public Stream Content { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false, IsRequired = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets folder.
        /// </summary>
        [DataMember(Name = "folder", EmitDefaultValue = false, IsRequired = false)]
        public Microsoft.OneDrive.Sdk.Folder Folder { get; set; }

        /// <summary>
        /// Gets or sets file.
        /// </summary>
        [DataMember(Name = "file", EmitDefaultValue = false, IsRequired = false)]
        public Microsoft.OneDrive.Sdk.File File { get; set; }

        private string _conflictBehavior;

        /// <summary>
        /// Gets or sets ConflictBehavior
        /// </summary>
        [DataMember(Name = "@name.conflictBehavior", EmitDefaultValue = false, IsRequired = false)]
        public string ConflictBehavior
        {
            get { return _conflictBehavior; }
            set { _conflictBehavior = OneDriveHelper.TransformCollisionOptionToConflictBehavior(value); }
        }

        /// <summary>
        /// Serialize the object in json format
        /// </summary>
        /// <returns>json string containing the data</returns>
        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
