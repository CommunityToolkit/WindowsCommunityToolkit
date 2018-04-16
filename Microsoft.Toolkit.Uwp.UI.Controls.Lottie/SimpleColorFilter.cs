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

using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// A color filter with a predefined transfer mode that applies the specified color on top of the
    /// original color. As there are many other transfer modes, please take a look at the definition
    /// of PorterDuff.Mode.SRC_ATOP to find one that suits your needs.
    /// This site has a great explanation of Porter/Duff compositing algebra as well as a visual
    /// representation of many of the transfer modes:
    /// http://ssp.impulsetrain.com/porterduff.html
    /// </summary>
    public class SimpleColorFilter : PorterDuffColorFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleColorFilter"/> class.
        /// This <see cref="ColorFilter"/> always uses the <see cref="PorterDuff.Mode.SrcAtop"/>.
        /// </summary>
        /// <param name="color">The color that this <see cref="ColorFilter"/> will use to blend the colors of it's target.</param>
        public SimpleColorFilter(Color color)
            : base(color, PorterDuff.Mode.SrcAtop)
        {
        }
    }
}