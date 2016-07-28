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

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Defines a color in Hue/Saturation/Value (HSV) space.
    /// </summary>
    public struct HsvColor
    {
        /// <summary>
        /// The Hue in 0..360 range.
        /// </summary>
        public double H;

        /// <summary>
        /// The Saturation in 0..1 range.
        /// </summary>
        public double S;

        /// <summary>
        /// The Value in 0..1 range.
        /// </summary>
        public double V;

        /// <summary>
        /// The Alpha/opacity in 0..1 range.
        /// </summary>
        public double A;
    }
}
