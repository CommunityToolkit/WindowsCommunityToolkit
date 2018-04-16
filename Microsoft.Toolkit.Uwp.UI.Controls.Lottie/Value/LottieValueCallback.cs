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

using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value
{
    /// <summary>
    /// Allows you to set a callback on a resolved <see cref="Model.KeyPath"/> to modify its animation values at runtime.
    /// </summary>
    /// <typeparam name="T">The type that the callback should return.</typeparam>
    public class LottieValueCallback<T> : ILottieValueCallback<T>
    {
        private readonly LottieFrameInfo<T> _frameInfo = new LottieFrameInfo<T>();

        private IBaseKeyframeAnimation _animation;

        /// <summary>
        /// Gets or sets the value of this callback. This can be set with <see cref="SetValue(T)"/> to use a value instead of deferring
        /// to the callback.
        /// </summary>
        protected T Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LottieValueCallback{T}"/> class.
        /// </summary>
        public LottieValueCallback()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LottieValueCallback{T}"/> class based on a static value.
        /// </summary>
        /// <param name="staticValue">The static value to initialize the <see cref="LottieValueCallback{T}"/></param>
        public LottieValueCallback(T staticValue)
        {
            Value = staticValue;
        }

        /// <inheritdoc/>
        public virtual T GetValue(LottieFrameInfo<T> frameInfo)
        {
            return Value;
        }

        /// <summary>
        /// Sets a value that the callback will store and potentially use
        /// </summary>
        /// <param name="value">The value to be set for the callback</param>
        public void SetValue(T value)
        {
            Value = value;
            if (_animation != null)
            {
                _animation.OnValueChanged();
            }
        }

        /// <inheritdoc/>
        public T GetValueInternal(
            float startFrame,
            float endFrame,
            T startValue,
            T endValue,
            float linearKeyframeProgress,
            float interpolatedKeyframeProgress,
            float overallProgress)
        {
            return GetValue(
                _frameInfo.Set(
                    startFrame,
                    endFrame,
                    startValue,
                    endValue,
                    linearKeyframeProgress,
                    interpolatedKeyframeProgress,
                    overallProgress));
        }

        /// <inheritdoc/>
        public void SetAnimation(IBaseKeyframeAnimation animation)
        {
            _animation = animation;
        }
    }
}