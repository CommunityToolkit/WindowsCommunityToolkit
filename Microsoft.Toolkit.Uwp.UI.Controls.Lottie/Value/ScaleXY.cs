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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value
{
    /// <summary>
    /// A class that provides the ability to change an animation layer on the X and Y axis, using diferent values for X and Y.
    /// </summary>
    public class ScaleXy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleXy"/> class.
        /// </summary>
        /// <param name="sx">The scale on the X axis</param>
        /// <param name="sy">The scale on the Y axis</param>
        public ScaleXy(float sx, float sy)
        {
            ScaleX = sx;
            ScaleY = sy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleXy"/> class, using 100% of scale (1f).
        /// </summary>
        public ScaleXy()
            : this(1f, 1f)
        {
        }

        internal virtual float ScaleX { get; }

        internal virtual float ScaleY { get; }

        /// <summary>
        /// For debuging purposes.
        /// </summary>
        /// <returns>A formated text to help understand the current KeyPath.</returns>
        public override string ToString()
        {
            return ScaleX + "x" + ScaleY;
        }
    }
}