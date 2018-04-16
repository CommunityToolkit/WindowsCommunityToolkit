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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal static class ShapeFillParser
    {
        internal static ShapeFill Parse(JsonReader reader, LottieComposition composition)
        {
            AnimatableColorValue color = null;
            bool fillEnabled = false;
            AnimatableIntegerValue opacity = null;
            string name = null;
            int fillTypeInt = 1;

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
                    case "o":
                        opacity = AnimatableValueParser.ParseInteger(reader, composition);
                        break;
                    case "fillEnabled":
                        fillEnabled = reader.NextBoolean();
                        break;
                    case "r":
                        fillTypeInt = reader.NextInt();
                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            var fillType = fillTypeInt == 1 ? PathFillType.Winding : PathFillType.EvenOdd;
            return new ShapeFill(name, fillEnabled, fillType, color, opacity);
        }
    }
}
