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

using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value
{
    /// <summary>
    /// A <see cref="LottieInterpolatedValue{T}"/> with T as int
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class LottieInterpolatedIntegerValue : LottieInterpolatedValue<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LottieInterpolatedIntegerValue"/> class.
        /// </summary>
        /// <param name="startValue">The starting value of the <see cref="LottieInterpolatedIntegerValue"/></param>
        /// <param name="endValue">The ending value of the <see cref="LottieInterpolatedIntegerValue"/></param>
        public LottieInterpolatedIntegerValue(int startValue, int endValue)
            : base(startValue, endValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LottieInterpolatedIntegerValue"/> class.
        /// </summary>
        /// <param name="startValue">The starting value of the <see cref="LottieInterpolatedIntegerValue"/></param>
        /// <param name="endValue">The ending value of the <see cref="LottieInterpolatedIntegerValue"/></param>
        /// <param name="interpolator">The <see cref="IInterpolator"/> that will interpolate the values between the start value and the end value.</param>
        public LottieInterpolatedIntegerValue(int startValue, int endValue, IInterpolator interpolator)
            : base(startValue, endValue, interpolator)
        {
        }

        /// <inheritdoc/>
        protected override int InterpolateValue(int startValue, int endValue, float progress)
        {
            return MiscUtils.Lerp(startValue, endValue, progress);
        }
    }
}