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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal static class FontCharacterParser
    {
        internal static FontCharacter Parse(JsonReader reader, LottieComposition composition)
        {
            char character = '\0';
            int size = 0;
            double width = 0;
            string style = null;
            string fontFamily = null;
            List<ShapeGroup> shapes = new List<ShapeGroup>();

            reader.BeginObject();
            while (reader.HasNext())
            {
                switch (reader.NextName())
                {
                    case "ch":
                        character = reader.NextString()[0];
                        break;
                    case "size":
                        size = reader.NextInt();
                        break;
                    case "w":
                        width = reader.NextDouble();
                        break;
                    case "style":
                        style = reader.NextString();
                        break;
                    case "fFamily":
                        fontFamily = reader.NextString();
                        break;
                    case "data":
                        reader.BeginObject();
                        while (reader.HasNext())
                        {
                            if ("shapes".Equals(reader.NextName()))
                            {
                                reader.BeginArray();
                                while (reader.HasNext())
                                {
                                    shapes.Add((ShapeGroup)ContentModelParser.Parse(reader, composition));
                                }

                                reader.EndArray();
                            }
                            else
                            {
                                reader.SkipValue();
                            }
                        }

                        reader.EndObject();
                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            reader.EndObject();

            return new FontCharacter(shapes, character, size, width, style, fontFamily);
        }
    }
}
