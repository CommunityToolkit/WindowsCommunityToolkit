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

using System.Numerics;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal class PointFParser : IValueParser<Vector2?>
    {
        internal static readonly PointFParser Instance = new PointFParser();

        private PointFParser()
        {
        }

        public Vector2? Parse(JsonReader reader, float scale)
        {
            JsonToken token = reader.Peek();
            if (token == JsonToken.StartArray)
            {
                return JsonUtils.JsonToPoint(reader, scale);
            }

            if (token == JsonToken.StartObject)
            {
                return JsonUtils.JsonToPoint(reader, scale);
            }

            if (token == JsonToken.Integer || token == JsonToken.Float)
            {
                // This is the case where the static value for a property is an array of numbers.
                // We begin the array to see if we have an array of keyframes but it's just an array
                // of static numbers instead.
                var point = new Vector2(reader.NextDouble() * scale, reader.NextDouble() * scale);
                while (reader.HasNext())
                {
                    reader.SkipValue();
                }

                return point;
            }

            throw new System.ArgumentException("Cannot convert json to point. Next token is " + token);
        }
    }
}