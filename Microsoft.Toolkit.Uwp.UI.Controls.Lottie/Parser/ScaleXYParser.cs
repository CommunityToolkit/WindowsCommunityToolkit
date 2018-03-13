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

using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal class ScaleXyParser : IValueParser<ScaleXy>
    {
        public static readonly ScaleXyParser Instance = new ScaleXyParser();

        public ScaleXy Parse(JsonReader reader, float scale)
        {
            bool isArray = reader.Peek() == JsonToken.StartArray;
            if (isArray)
            {
                reader.BeginArray();
            }

            float sx = reader.NextDouble();
            float sy = reader.NextDouble();
            while (reader.HasNext())
            {
                reader.SkipValue();
            }

            if (isArray)
            {
                reader.EndArray();
            }

            return new ScaleXy(sx / 100f * scale, sy / 100f * scale);
        }
    }
}
