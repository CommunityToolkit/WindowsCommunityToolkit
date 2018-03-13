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
    internal static class ShapeTrimPathParser
    {
        internal static ShapeTrimPath Parse(JsonReader reader, LottieComposition composition)
        {
            string name = null;
            ShapeTrimPath.Type type = ShapeTrimPath.Type.Simultaneously;
            AnimatableFloatValue start = null;
            AnimatableFloatValue end = null;
            AnimatableFloatValue offset = null;

            while (reader.HasNext())
            {
                switch (reader.NextName())
                {
                    case "s":
                        start = AnimatableValueParser.ParseFloat(reader, composition, false);
                        break;
                    case "e":
                        end = AnimatableValueParser.ParseFloat(reader, composition, false);
                        break;
                    case "o":
                        offset = AnimatableValueParser.ParseFloat(reader, composition, false);
                        break;
                    case "nm":
                        name = reader.NextString();
                        break;
                    case "m":
                        type = (ShapeTrimPath.Type)reader.NextInt();
                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            return new ShapeTrimPath(name, type, start, end, offset);
        }
    }
}
