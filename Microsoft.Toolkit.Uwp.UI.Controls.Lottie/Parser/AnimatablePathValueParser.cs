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
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Animatable;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal static class AnimatablePathValueParser
    {
        public static AnimatablePathValue Parse(JsonReader reader, LottieComposition composition)
        {
            List<Keyframe<Vector2?>> keyframes = new List<Keyframe<Vector2?>>();
            if (reader.Peek() == JsonToken.StartArray)
            {
                reader.BeginArray();
                while (reader.HasNext())
                {
                    keyframes.Add(PathKeyframeParser.Parse(reader, composition));
                }

                reader.EndArray();
                KeyframesParser.SetEndFrames<Keyframe<Vector2?>, Vector2?>(keyframes);
            }
            else
            {
                keyframes.Add(new Keyframe<Vector2?>(JsonUtils.JsonToPoint(reader, Utils.Utils.DpScale())));
            }

            return new AnimatablePathValue(keyframes);
        }

        // Returns either an <see cref="AnimatablePathValue"/> or an <see cref="AnimatableSplitDimensionPathValue"/>.
        internal static IAnimatableValue<Vector2?, Vector2?> ParseSplitPath(JsonReader reader, LottieComposition composition)
        {
            AnimatablePathValue pathAnimation = null;
            AnimatableFloatValue xAnimation = null;
            AnimatableFloatValue yAnimation = null;

            bool hasExpressions = false;

            reader.BeginObject();
            while (reader.Peek() != JsonToken.EndObject)
            {
                switch (reader.NextName())
                {
                    case "k":
                        pathAnimation = Parse(reader, composition);
                        break;
                    case "x":
                        if (reader.Peek() == JsonToken.String)
                        {
                            hasExpressions = true;
                            reader.SkipValue();
                        }
                        else
                        {
                            xAnimation = AnimatableValueParser.ParseFloat(reader, composition);
                        }

                        break;
                    case "y":
                        if (reader.Peek() == JsonToken.String)
                        {
                            hasExpressions = true;
                            reader.SkipValue();
                        }
                        else
                        {
                            yAnimation = AnimatableValueParser.ParseFloat(reader, composition);
                        }

                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            reader.EndObject();

            if (hasExpressions)
            {
                composition.AddWarning("Lottie doesn't support expressions.");
            }

            if (pathAnimation != null)
            {
                return pathAnimation;
            }

            return new AnimatableSplitDimensionPathValue(xAnimation, yAnimation);
        }
    }
}
