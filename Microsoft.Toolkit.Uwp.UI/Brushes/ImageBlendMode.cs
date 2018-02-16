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
// Composition supported version of http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_BlendEffectMode.htm.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Brushes
{
    /// <summary>
    /// Blend mode to use when compositing effects.  See http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_BlendEffectMode.htm.
    /// </summary>
    public enum ImageBlendMode
    {
        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member - see http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_BlendEffectMode.htm.
        Multiply = 0,
        Screen = 1,
        Darken = 2,
        Lighten = 3,
        ////Dissolve = 4, // Not Supported
        ColorBurn = 5,
        LinearBurn = 6,
        DarkerColor = 7,
        LighterColor = 8,
        ColorDodge = 9,
        LinearDodge = 10,
        Overlay = 11,
        SoftLight = 12,
        HardLight = 13,
        VividLight = 14,
        LinearLight = 15,
        PinLight = 16,
        HardMix = 17,
        Difference = 18,
        Exclusion = 19,
        Hue = 20, // Says Not Supported, but works
        Saturation = 21, // Says Not Supported, but works
        Color = 22, // Says Not Supported, but works
        Luminosity = 23, // Says Not Supported, but works
        Subtract = 24,
        Division = 25,
        #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
