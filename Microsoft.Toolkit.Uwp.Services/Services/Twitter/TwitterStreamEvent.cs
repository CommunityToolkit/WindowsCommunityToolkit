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
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Twitter User type.
    /// </summary>
    public class TwitterStreamEvent : ITwitterResult
    {
        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        [JsonProperty(PropertyName = "event")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TwitterStreamEventType EventType { get; set; }

        /// <summary>
        /// Gets or sets the source of the event. This is always the user who initiated the event.
        /// </summary>
        /// <value>The source.</value>
        public TwitterUser Source { get; set; }

        /// <summary>
        /// Gets or sets the target of the event. This is the user who was affected, or who owns the affected object.
        /// </summary>
        /// <value>The source.</value>
        public TwitterUser Target { get; set; }

        /// <summary>
        /// Gets or sets the target object.
        /// </summary>
        /// <value>The target object.</value>
        public Tweet TargetObject { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets the creation date
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                DateTime dt;
                if (!DateTime.TryParseExact(CreatedAt, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    dt = DateTime.Today;
                }

                return dt;
            }
        }
    }
}
