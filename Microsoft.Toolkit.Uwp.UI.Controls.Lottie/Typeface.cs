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

using Windows.UI.Text;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// Represents a Typeface to be used on text rendering
    /// </summary>
    public class Typeface
    {
        private Typeface(string fontFamily, FontStyle style, FontWeight weight)
        {
            FontFamily = fontFamily;
            Style = style;
            Weight = weight;
        }

        /// <summary>
        /// Gets the font family of this font.
        /// </summary>
        public string FontFamily { get; }

        /// <summary>
        /// Gets the <see cref="FontStyle"/> of this font.
        /// </summary>
        public FontStyle Style { get; }

        /// <summary>
        /// Gets the <see cref="FontWeight"/> of this font.
        /// </summary>
        public FontWeight Weight { get; }

        /// <summary>
        /// Creates a <see cref="Typeface"/> using another <see cref="Typeface"/>, but changing it's <see cref="Style"/> and <see cref="Weight"/>
        /// </summary>
        /// <param name="typeface">The <see cref="Typeface"/> to be based on.</param>
        /// <param name="style">The new <see cref="FontStyle"/></param>
        /// <param name="weight">The new <see cref="FontWeight"/></param>
        /// <returns>Returns a new typeface based on the <see cref="FontFamily"/> of the given <see cref="Typeface"/> and it's <see cref="Style"/> and <see cref="Weight"/></returns>
        public static Typeface Create(Typeface typeface, FontStyle style, FontWeight weight)
        {
            return new Typeface(typeface.FontFamily, style, weight);
        }

        /// <summary>
        /// Creates a <see cref="Typeface"/> from an asset file
        /// </summary>
        /// <param name="path">The path to the <see cref="Typeface"/> file that will be loaded. It can be anything that the underlying <see cref="Graphics.Canvas.Text.CanvasTextFormat"/> supports.</param>
        /// <returns>Return the <see cref="Typeface"/> with the corresponding path.</returns>
        public static Typeface CreateFromAsset(string path)
        {
            return new Typeface(path, FontStyle.Normal, FontWeights.Normal);
        }
    }
}