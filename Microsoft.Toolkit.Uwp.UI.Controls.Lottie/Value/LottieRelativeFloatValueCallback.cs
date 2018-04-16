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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value
{
    /// <summary>
    /// <see cref="Value.LottieValueCallback{T}"/> that provides a value offset from the original animation
    ///  rather than an absolute value.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LottieRelativeFloatValueCallback : LottieValueCallback<float?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LottieRelativeFloatValueCallback"/> class.
        /// </summary>
        public LottieRelativeFloatValueCallback()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LottieRelativeFloatValueCallback"/> class.
        /// </summary>
        /// <param name="staticValue">A static <see cref="float"/> to be used as a static value.</param>
        public LottieRelativeFloatValueCallback(float staticValue)
            : base(staticValue)
        {
        }

        /// <inheritdoc/>
        public override float? GetValue(LottieFrameInfo<float?> frameInfo)
        {
            float originalValue = MiscUtils.Lerp(
                frameInfo.StartValue.Value,
                frameInfo.EndValue.Value,
                frameInfo.InterpolatedKeyframeProgress);
            float offset = GetOffset(frameInfo);
            return originalValue + offset;
        }

        /// <summary>
        /// Gets the offset for a specified <see cref="LottieFrameInfo{T}"/>
        /// </summary>
        /// <param name="frameInfo">The <see cref="LottieFrameInfo{T}"/> that the offset should get the value from</param>
        /// <returns>Returns the value of the provided <see cref="LottieFrameInfo{T}"/></returns>
        public float GetOffset(LottieFrameInfo<float?> frameInfo)
        {
            if (Value == null)
            {
                throw new ArgumentException("You must provide a static value in the constructor " +
                                                   ", call setValue, or override getValue.");
            }

            return Value.Value;
        }
    }
}
