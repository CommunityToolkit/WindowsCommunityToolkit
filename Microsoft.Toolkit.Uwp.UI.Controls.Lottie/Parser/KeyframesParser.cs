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

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal static class KeyframesParser
    {
        internal static List<Keyframe<T>> Parse<T>(JsonReader reader, LottieComposition composition, float scale, IValueParser<T> valueParser)
        {
            List<Keyframe<T>> keyframes = new List<Keyframe<T>>();

            if (reader.Peek() == JsonToken.String)
            {
                composition.AddWarning("Lottie doesn't support expressions.");
                return keyframes;
            }

            reader.BeginObject();
            while (reader.HasNext())
            {
                switch (reader.NextName())
                {
                    case "k":
                        if (reader.Peek() == JsonToken.StartArray)
                        {
                            reader.BeginArray();

                            if (reader.Peek() == JsonToken.Integer || reader.Peek() == JsonToken.Float)
                            {
                                // For properties in which the static value is an array of numbers.
                                keyframes.Add(KeyframeParser.Parse(reader, composition, scale, valueParser, false));
                            }
                            else
                            {
                                while (reader.HasNext())
                                {
                                    keyframes.Add(KeyframeParser.Parse(reader, composition, scale, valueParser, true));
                                }
                            }

                            reader.EndArray();
                        }
                        else
                        {
                            keyframes.Add(KeyframeParser.Parse(reader, composition, scale, valueParser, false));
                        }

                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            reader.EndObject();

            SetEndFrames<Keyframe<T>, T>(keyframes);
            return keyframes;
        }

        // The json doesn't include end frames. The data can be taken from the start frame of the next
        // keyframe though.
        public static void SetEndFrames<TU, TV>(List<TU> keyframes)
            where TU : Keyframe<TV>
        {
            int size = keyframes.Count;
            for (int i = 0; i < size - 1; i++)
            {
                // In the json, the keyframes only contain their starting frame.
                keyframes[i].EndFrame = keyframes[i + 1].StartFrame;
            }

            var lastKeyframe = keyframes[size - 1];
            if (lastKeyframe.StartValue == null)
            {
                // The only purpose the last keyframe has is to provide the end frame of the previous
                // keyframe.
                keyframes.Remove(lastKeyframe);
            }
        }
    }
}
