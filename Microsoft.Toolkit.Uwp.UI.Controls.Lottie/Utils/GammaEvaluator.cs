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
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils
{
    /// <summary>
    /// Use this instead of ArgbEvaluator because it interpolates through the gamma color
    /// space which looks better to us humans.
    /// <para>
    /// Writted by Romain Guy and Francois Blavoet.
    /// https://androidstudygroup.slack.com/archives/animation/p1476461064000335
    /// </para>
    /// </summary>
    internal static class GammaEvaluator
    {
        // Opto-electronic conversion function for the sRGB color space
        // Takes a gamma-encoded sRGB value and converts it to a linear sRGB value
        private static float OECF_sRGB(float linear)
        {
            // IEC 61966-2-1:1999
            return linear <= 0.0031308f ? linear * 12.92f : (float)((Math.Pow(linear, 1.0f / 2.4f) * 1.055f) - 0.055f);
        }

        // Electro-optical conversion function for the sRGB color space
        // Takes a linear sRGB value and converts it to a gamma-encoded sRGB value
        private static float EOCF_sRGB(float srgb)
        {
            // IEC 61966-2-1:1999
            return srgb <= 0.04045f ? srgb / 12.92f : (float)Math.Pow((srgb + 0.055f) / 1.055f, 2.4f);
        }

        internal static Color Evaluate(float fraction, Color startColor, Color endColor)
        {
            return Evaluate(fraction, startColor.A / 255.0f, startColor.R / 255.0f, startColor.G / 255.0f, startColor.B / 255.0f, endColor.A / 255.0f, endColor.R / 255.0f, endColor.G / 255.0f, endColor.B / 255.0f);
        }

        // internal static int evaluate(float fraction, int startInt, int endInt)
        // {
        //    float startA = ((startInt >> 24) & 0xff) / 255.0f;
        //    float startR = ((startInt >> 16) & 0xff) / 255.0f;
        //    float startG = ((startInt >> 8) & 0xff) / 255.0f;
        //    float startB = (startInt & 0xff) / 255.0f;

        // float endA = ((endInt >> 24) & 0xff) / 255.0f;
        //    float endR = ((endInt >> 16) & 0xff) / 255.0f;
        //    float endG = ((endInt >> 8) & 0xff) / 255.0f;
        //    float endB = (endInt & 0xff) / 255.0f;

        // return evaluate(fraction, startA, startR, startG, startB, endA, endR, endG, endB);
        // }
        private static Color Evaluate(float fraction, float startA, float startR, float startG, float startB, float endA, float endR, float endG, float endB)
        {
            // convert from sRGB to linear
            startR = EOCF_sRGB(startR);
            startG = EOCF_sRGB(startG);
            startB = EOCF_sRGB(startB);

            endR = EOCF_sRGB(endR);
            endG = EOCF_sRGB(endG);
            endB = EOCF_sRGB(endB);

            // compute the interpolated color in linear space
            var a = startA + (fraction * (endA - startA));
            var r = startR + (fraction * (endR - startR));
            var g = startG + (fraction * (endG - startG));
            var b = startB + (fraction * (endB - startB));

            // convert back to sRGB in the [0..255] range
            a = a * 255.0f;
            r = OECF_sRGB(r) * 255.0f;
            g = OECF_sRGB(g) * 255.0f;
            b = OECF_sRGB(b) * 255.0f;

            return Color.FromArgb((byte)Math.Round(a), (byte)Math.Round(r), (byte)Math.Round(g), (byte)Math.Round(b));
        }
    }
}