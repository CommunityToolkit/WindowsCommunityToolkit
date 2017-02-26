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
        public ITwitterStreamResult Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            JObject obj = (JObject)JsonConvert.DeserializeObject(data);

            //var friends = obj.SelectToken("friends", false);
            //if (friends != null)
            //{
            //    //if (friendsCallback != null && friends.HasValues)
            //    //{
            //    //    friendsCallback(JsonConvert.DeserializeObject<TwitterIdCollection>(ConvertJTokenToString(friends)));
            //    //}
            //    return null;
            //}

            //var delete = obj.SelectToken("delete", false);
            //if (delete != null)
            //{
            //    var deletedStatus = delete.SelectToken("status", false);
            //    if (deletedStatus != null)
            //    {
            //        //if (statusDeletedCallback != null && deletedStatus.HasValues)
            //        //{
            //        //    statusDeletedCallback(JsonConvert.DeserializeObject<TwitterStreamDeletedEvent>(ConvertJTokenToString(deletedStatus)));
            //        //}
            //        return null;
            //    }

            //    var deletedDirectMessage = delete.SelectToken("direct_message", false);
            //    if (deletedDirectMessage != null)
            //    {
            //        //if (directMessageDeletedCallback != null && deletedDirectMessage.HasValues)
            //        //{
            //        //    directMessageDeletedCallback(JsonConvert.DeserializeObject<TwitterStreamDeletedEvent>(ConvertJTokenToString(deletedDirectMessage)));
            //        //}
            //        return;
            //    }
            //}

            var events = obj.SelectToken("event", false);
            if (events != null)
            {
                var targetobject = obj.SelectToken("target_object", false);
                Tweet endtargetobject = null;
                if (targetobject != null)
                {
                    if (targetobject.SelectToken("subscriber_count", false) != null)
                    {
                        //endtargetobject = JsonConvert.DeserializeObject<TwitterList>(targetobject.ToString());
                    }
                    else if (targetobject.SelectToken("user", false) != null)
                    {
                        endtargetobject = JsonConvert.DeserializeObject<Tweet>(targetobject.ToString());
                    }
                }

                var endevent = JsonConvert.DeserializeObject<TwitterStreamEvent>(obj.ToString());
                endevent.TargetObject = endtargetobject;
                return endevent;
            }

            var user = obj.SelectToken("user", false);
            if (user != null && user.HasValues)
            {
                return JsonConvert.DeserializeObject<TwitterUserStream>(obj.ToString());
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
