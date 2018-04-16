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
    /// Data class for use with <see cref="LottieValueCallback{T}"/>.
    /// You should* not* hold a reference to the frame info parameter passed to your callback. It will be reused.
    /// </summary>
    /// <typeparam name="T">The type that the value of the LottieFrameInfo refers to.</typeparam>
    public class LottieFrameInfo<T>
    {
        internal LottieFrameInfo<T> Set(
            float startFrame,
            float endFrame,
            T startValue,
            T endValue,
            float linearKeyframeProgress,
            float interpolatedKeyframeProgress,
            float overallProgress)
        {
            StartFrame = startFrame;
            EndFrame = endFrame;
            StartValue = startValue;
            EndValue = endValue;
            LinearKeyframeProgress = linearKeyframeProgress;
            InterpolatedKeyframeProgress = interpolatedKeyframeProgress;
            OverallProgress = overallProgress;
            return this;
        }

        /// <summary>
        /// Gets the start frame of the <see cref="LottieFrameInfo{T}"/>
        /// </summary>
        public float StartFrame { get; private set; }

        /// <summary>
        /// Gets the end frame of the <see cref="LottieFrameInfo{T}"/>
        /// </summary>
        public float EndFrame { get; private set; }

        /// <summary>
        /// Gets the start value of the <see cref="LottieFrameInfo{T}"/>
        /// </summary>
        public T StartValue { get; private set; }

        /// <summary>
        /// Gets the end value of the <see cref="LottieFrameInfo{T}"/>
        /// </summary>
        public T EndValue { get; private set; }

        /// <summary>
        /// Gets the linear keyframe progress of the <see cref="LottieFrameInfo{T}"/>
        /// </summary>
        public float LinearKeyframeProgress { get; private set; }

        /// <summary>
        /// Gets the interpolated keyframe progress of the <see cref="LottieFrameInfo{T}"/>
        /// </summary>
        public float InterpolatedKeyframeProgress { get; private set; }

        /// <summary>
        /// Gets the overall progress of the <see cref="LottieFrameInfo{T}"/>
        /// </summary>
        public float OverallProgress { get; private set; }
    }
}
