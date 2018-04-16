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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal static class ShapeGroupParser
    {
        internal static ShapeGroup Parse(JsonReader reader, LottieComposition composition)
        {
            string name = null;
            List<IContentModel> items = new List<IContentModel>();

            while (reader.HasNext())
            {
                switch (reader.NextName())
                {
                    case "nm":
                        name = reader.NextString();
                        break;
                    case "it":
                        reader.BeginArray();
                        while (reader.HasNext())
                        {
                            IContentModel newItem = ContentModelParser.Parse(reader, composition);
                            if (newItem != null)
                            {
                                items.Add(newItem);
                            }
                        }

                        reader.EndArray();
                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            return new ShapeGroup(name, items);
        }
    }
}
