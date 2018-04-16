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

using System.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal static class ContentModelParser
    {
        internal static IContentModel Parse(JsonReader reader, LottieComposition composition)
        {
            string type = null;

            reader.BeginObject();

            // Unfortunately, for an ellipse, d is before "ty" which means that it will get parsed
            // before we are in the ellipse parser.
            // "d" is 2 for normal and 3 for reversed.
            int d = 2;
            while (reader.HasNext())
            {
                switch (reader.NextName())
                {
                    case "ty":
                        type = reader.NextString();
                        goto typeLoop;
                    case "d":
                        d = reader.NextInt();
                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            typeLoop:

            if (type == null)
            {
                return null;
            }

            IContentModel model = null;
            switch (type)
            {
                case "gr":
                    model = ShapeGroupParser.Parse(reader, composition);
                    break;
                case "st":
                    model = ShapeStrokeParser.Parse(reader, composition);
                    break;
                case "gs":
                    model = GradientStrokeParser.Parse(reader, composition);
                    break;
                case "fl":
                    model = ShapeFillParser.Parse(reader, composition);
                    break;
                case "gf":
                    model = GradientFillParser.Parse(reader, composition);
                    break;
                case "tr":
                    model = AnimatableTransformParser.Parse(reader, composition);
                    break;
                case "sh":
                    model = ShapePathParser.Parse(reader, composition);
                    break;
                case "el":
                    model = CircleShapeParser.Parse(reader, composition, d);
                    break;
                case "rc":
                    model = RectangleShapeParser.Parse(reader, composition);
                    break;
                case "tm":
                    model = ShapeTrimPathParser.Parse(reader, composition);
                    break;
                case "sr":
                    model = PolystarShapeParser.Parse(reader, composition);
                    break;
                case "mm":
                    model = MergePathsParser.Parse(reader);
                    composition.AddWarning("Animation contains merge paths. Merge paths " +
                        "must be manually enabled by calling " +
                        "EnableMergePaths().");
                    break;
                case "rp":
                    model = RepeaterParser.Parse(reader, composition);
                    break;
                default:
                    Debug.WriteLine("Unknown shape type " + type, LottieLog.Tag);
                    break;
            }

            while (reader.HasNext())
            {
                reader.SkipValue();
            }

            reader.EndObject();

            return model;
        }
    }
}
