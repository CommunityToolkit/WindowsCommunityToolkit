// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;

namespace Microsoft.Toolkit.Services.Twitter
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

            var obj = JsonDocument.Parse(data);

            if (obj.RootElement.TryGetProperty("friends", out var friends) && friends.GetArrayLength() > 0)
            {
                return null;
            }

            if (obj.RootElement.TryGetProperty("delete", out var delete))
            {
                if (delete.TryGetProperty("status", out var deletedStatus) && deletedStatus.GetArrayLength() > 0)
                {
                    return JsonSerializer.Deserialize<TwitterStreamDeletedEvent>(deletedStatus.ToString());
                }

                if (delete.TryGetProperty("direct_message", out var deletedDirectMessage) && deletedDirectMessage.GetArrayLength() > 0)
                {
                    return JsonSerializer.Deserialize<TwitterStreamDeletedEvent>(deletedDirectMessage.ToString());
                }
            }

            if (obj.RootElement.TryGetProperty("event", out var events))
            {
                Tweet endTargetObject = null;
                if (obj.RootElement.TryGetProperty("target_object", out var targetObject) && targetObject.TryGetProperty("user", out _))
                {
                    endTargetObject = JsonSerializer.Deserialize<Tweet>(targetObject.ToString());
                }

                var endEvent = JsonSerializer.Deserialize<TwitterStreamEvent>(obj.ToString());
                endEvent.TargetObject = endTargetObject;
                return endEvent;
            }

            if (obj.RootElement.TryGetProperty("user", out var user) && user.GetArrayLength() > 0)
            {
                return JsonSerializer.Deserialize<Tweet>(obj.ToString());
            }

            if (obj.RootElement.TryGetProperty("direct_message", out var directMessage) && directMessage.GetArrayLength() > 0)
            {
                return JsonSerializer.Deserialize<TwitterDirectMessage>(directMessage.ToString());
            }

            return null;
        }
    }
}
