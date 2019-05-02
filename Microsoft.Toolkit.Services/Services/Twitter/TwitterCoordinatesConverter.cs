// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Toolkit.Services.Twitter
{
    internal class TwitterCoordinatesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                if (reader.TokenType != JsonToken.StartObject)
                {
                    return null;
                }

                var jObject = JObject.Load(reader);
                var jCoordinates = jObject["coordinates"] as JArray;

                if (jCoordinates.Count != 2)
                {
                    return null;
                }

                var twitterCoordinates = new TwitterCoordinates
                {
                    Latitude = (double)jCoordinates[0],
                    Longitude = (double)jCoordinates[1]
                };
                return twitterCoordinates;
            }
            catch
            {
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}