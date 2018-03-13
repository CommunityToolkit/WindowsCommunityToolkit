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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Animatable;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal static class ShapeStrokeParser
    {
        internal static ShapeStroke Parse(JsonReader reader, LottieComposition composition)
        {
            string name = null;
            AnimatableColorValue color = null;
            AnimatableFloatValue width = null;
            AnimatableIntegerValue opacity = null;
            ShapeStroke.LineCapType capType = ShapeStroke.LineCapType.Unknown;
            ShapeStroke.LineJoinType joinType = ShapeStroke.LineJoinType.Round;
            AnimatableFloatValue offset = null;

            List<AnimatableFloatValue> lineDashPattern = new List<AnimatableFloatValue>();

            while (reader.HasNext())
            {
                switch (reader.NextName())
                {
                    case "nm":
                        name = reader.NextString();
                        break;
                    case "c":
                        color = AnimatableValueParser.ParseColor(reader, composition);
                        break;
                    case "w":
                        width = AnimatableValueParser.ParseFloat(reader, composition);
                        break;
                    case "o":
                        opacity = AnimatableValueParser.ParseInteger(reader, composition);
                        break;
                    case "lc":
                        capType = (ShapeStroke.LineCapType)(reader.NextInt() - 1);
                        break;
                    case "lj":
                        joinType = (ShapeStroke.LineJoinType)(reader.NextInt() - 1);
                        break;
                    case "d":
                        reader.BeginArray();
                        while (reader.HasNext())
                        {
                            string n = null;
                            AnimatableFloatValue val = null;

                            reader.BeginObject();
                            while (reader.HasNext())
                            {
                                switch (reader.NextName())
                                {
                                    case "n":
                                        n = reader.NextString();
                                        break;
                                    case "v":
                                        val = AnimatableValueParser.ParseFloat(reader, composition);
                                        break;
                                    default:
                                        reader.SkipValue();
                                        break;
                                }
                            }

                            reader.EndObject();

                            switch (n)
                            {
                                case "o":
                                    offset = val;
                                    break;
                                case "d":
                                case "g":
                                    lineDashPattern.Add(val);
                                    break;
                            }
                        }

                        reader.EndArray();

                        if (lineDashPattern.Count == 1)
                        {
                            // If there is only 1 value then it is assumed to be equal parts on and off.
                            lineDashPattern.Add(lineDashPattern[0]);
                        }

                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            return new ShapeStroke(name, offset, lineDashPattern, color, opacity, width, capType, joinType);
        }
    }
}
