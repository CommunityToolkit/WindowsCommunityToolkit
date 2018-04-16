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

using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Animatable;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal static class AnimatableTextPropertiesParser
    {
        public static AnimatableTextProperties Parse(JsonReader reader, LottieComposition composition)
        {
            AnimatableTextProperties anim = null;

            reader.BeginObject();
            while (reader.HasNext())
            {
                switch (reader.NextName())
                {
                    case "a":
                        anim = ParseAnimatableTextProperties(reader, composition);
                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            reader.EndObject();
            if (anim == null)
            {
                // Not sure if this is possible.
                return new AnimatableTextProperties(null, null, null, null);
            }

            return anim;
        }

        private static AnimatableTextProperties ParseAnimatableTextProperties(JsonReader reader, LottieComposition composition)
        {
            AnimatableColorValue color = null;
            AnimatableColorValue stroke = null;
            AnimatableFloatValue strokeWidth = null;
            AnimatableFloatValue tracking = null;

            reader.BeginObject();
            while (reader.HasNext())
            {
                switch (reader.NextName())
                {
                    case "fc":
                        color = AnimatableValueParser.ParseColor(reader, composition);
                        break;
                    case "sc":
                        stroke = AnimatableValueParser.ParseColor(reader, composition);
                        break;
                    case "sw":
                        strokeWidth = AnimatableValueParser.ParseFloat(reader, composition);
                        break;
                    case "t":
                        tracking = AnimatableValueParser.ParseFloat(reader, composition);
                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            reader.EndObject();

            return new AnimatableTextProperties(color, stroke, strokeWidth, tracking);
        }
    }
}
