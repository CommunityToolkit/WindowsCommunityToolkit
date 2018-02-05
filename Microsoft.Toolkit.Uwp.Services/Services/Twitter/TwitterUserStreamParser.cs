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
using Newtonsoft.Json.Linq;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Twitter Timeline Parser.
    /// </summary>
    public class TwitterUserStreamParser
    {
        /// <summary>
        /// Parse string data into strongly typed list.
        /// </summary>
        /// <param name="data">Input string.</param>
        /// <returns>List of strongly typed objects.</returns>
        public ITwitterResult Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var obj = (JObject)JsonConvert.DeserializeObject(data);

            var friends = obj.SelectToken("friends", false);
            if (friends != null && friends.HasValues)
            {
                return null;
            }

            var delete = obj.SelectToken("delete", false);
            if (delete != null)
            {
                var deletedStatus = delete.SelectToken("status", false);
                if (deletedStatus != null && deletedStatus.HasValues)
                {
                    return JsonConvert.DeserializeObject<TwitterStreamDeletedEvent>(deletedStatus.ToString());
                }

                var deletedDirectMessage = delete.SelectToken("direct_message", false);
                if (deletedDirectMessage != null && deletedDirectMessage.HasValues)
                {
                    return JsonConvert.DeserializeObject<TwitterStreamDeletedEvent>(deletedDirectMessage.ToString());
                }
            }

            var events = obj.SelectToken("event", false);
            if (events != null)
            {
                var targetobject = obj.SelectToken("target_object", false);
                Tweet endtargetobject = null;
                if (targetobject?.SelectToken("user", false) != null)
                {
                    endtargetobject = JsonConvert.DeserializeObject<Tweet>(targetobject.ToString());
                }

                var endevent = JsonConvert.DeserializeObject<TwitterStreamEvent>(obj.ToString());
                endevent.TargetObject = endtargetobject;
                return endevent;
            }

            var user = obj.SelectToken("user", false);
            if (user != null && user.HasValues)
            {
                return JsonConvert.DeserializeObject<Tweet>(obj.ToString());
            }

            var directMessage = obj.SelectToken("direct_message", false);
            if (directMessage != null && directMessage.HasValues)
            {
                return JsonConvert.DeserializeObject<TwitterDirectMessage>(directMessage.ToString());
            }

            return null;
        }
    }
}
