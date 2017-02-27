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

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Twitter User type.
    /// </summary>
    public class TwitterStreamDeletedEvent : ITwitterResult
    {
        /// <summary>
        /// Gets or sets the user id of the event. This is always the user who initiated the event.
        /// </summary>
        /// <value>The user Id.</value>
        [JsonProperty(PropertyName = "user_id_str")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the id of the event. This is the tweet that was affected.
        /// </summary>
        /// <value>The tweet Id.</value>
        [JsonProperty(PropertyName = "id_str")]
        public string Id { get; set; }
    }
}
