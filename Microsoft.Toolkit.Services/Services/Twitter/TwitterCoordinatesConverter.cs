// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.Twitter
{
    internal class TwitterCoordinatesConverter : JsonConverter<TwitterCoordinates>
    {
        private readonly JsonEncodedText latitudeName = JsonEncodedText.Encode("Latitude");
        private readonly JsonEncodedText longitudeName = JsonEncodedText.Encode("Longitude");

        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        private readonly JsonConverter<double> doubleConverter;

        public TwitterCoordinatesConverter(JsonSerializerOptions options)
        {
            doubleConverter = options?.GetConverter(typeof(double)) as JsonConverter<double> ?? throw new InvalidOperationException();
        }

        public override TwitterCoordinates Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    return null;
                }

                double latitude = default;
                bool latitudeSet = false;

                double longitude = default;
                bool longitudeSet = false;

                // Get the first property.
                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                if (reader.ValueTextEquals(latitudeName.EncodedUtf8Bytes))
                {
                    latitude = ReadProperty(ref reader, options);
                    latitudeSet = true;
                }
                else if (reader.ValueTextEquals(longitudeName.EncodedUtf8Bytes))
                {
                    longitude = ReadProperty(ref reader, options);
                    longitudeSet = true;
                }
                else
                {
                    throw new JsonException();
                }

                // Get the second property.
                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                if (latitudeSet && reader.ValueTextEquals(longitudeName.EncodedUtf8Bytes))
                {
                    longitude = ReadProperty(ref reader, options);
                }
                else if (longitudeSet && reader.ValueTextEquals(latitudeName.EncodedUtf8Bytes))
                {
                    latitude = ReadProperty(ref reader, options);
                }
                else
                {
                    throw new JsonException();
                }

                reader.Read();

                if (reader.TokenType != JsonTokenType.EndObject)
                {
                    throw new JsonException();
                }

                return new TwitterCoordinates
                {
                    Latitude = latitude,
                    Longitude = longitude
                };
            }
            catch
            {
                return null;
            }
        }

        private double ReadProperty(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            reader.Read();
            return doubleConverter.Read(ref reader, typeof(double), options);
        }

        public override void Write(Utf8JsonWriter writer, TwitterCoordinates value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
