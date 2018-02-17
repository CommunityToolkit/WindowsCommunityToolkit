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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Brushes
{
    /// <summary>
    /// Specifies how to draw the gradient outside a gradient brush's gradient vector or space.
    /// </summary>
    public enum GradientSpreadMethod
    {
        /// <summary>
        /// Default value. The color values at the ends of the gradient vector fill the remaining space.
        /// </summary>
        Pad = 0, // Win2D Clamp

        /// <summary>
        /// The gradient is repeated in the reverse direction until the space is filled.
        /// </summary>
        Reflect = 2, // Win2D Mirror Note: We change the numerical value from WPF so that we can directly interop via int conversion to Win2D.

        /// <summary>
        /// The gradient is repeated in the original direction until the space is filled.
        /// </summary>
        Repeat = 1, // Win2D Wrap
    }
}
