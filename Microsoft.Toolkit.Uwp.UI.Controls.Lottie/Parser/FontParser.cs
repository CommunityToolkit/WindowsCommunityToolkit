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

using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal static class FontParser
    {
        internal static Font Parse(JsonReader reader)
        {
            string family = null;
            string name = null;
            string style = null;
            float ascent = 0;

            reader.BeginObject();
            while (reader.HasNext())
            {
                switch (reader.NextName())
                {
                    case "fFamily":
                        family = reader.NextString();
                        break;
                    case "fName":
                        name = reader.NextString();
                        break;
                    case "fStyle":
                        style = reader.NextString();
                        break;
                    case "ascent":
                        ascent = reader.NextDouble();
                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            reader.EndObject();

            return new Font(family, name, style, ascent);
        }
    }
}
