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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model
{
    /// <summary>
    /// Represents a font that can be used to draw strings on the canvas
    /// </summary>
    public class Font
    {
        internal Font(string family, string name, string style, float ascent)
        {
            Family = family;
            Name = name;
            Style = style;
            _ascent = ascent;
        }

        /// <summary>
        /// Gets the family of the font
        /// </summary>
        public string Family { get; }

        /// <summary>
        /// Gets the name of the font
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the style of the font
        /// </summary>
        public string Style { get; }

        internal float Ascent => _ascent;

        private readonly float _ascent;
    }
}
