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

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    ///  RootParentReference class
    /// </summary>
    public class OneDriveParentReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveParentReference"/> class.
        /// </summary>
        public OneDriveParentReference()
        {
            Parent = new OneDriveParent();
        }

        /// <summary>
        /// Gets or sets the reference to the parent's item
        /// </summary>
        [JsonProperty("parentReference")]
        public OneDriveParent Parent { get; set; }

        /// <summary>
        /// Gets or sets the item's name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
