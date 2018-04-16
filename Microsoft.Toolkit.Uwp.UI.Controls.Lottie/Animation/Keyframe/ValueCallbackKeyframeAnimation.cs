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

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe
{
    internal class ValueCallbackKeyframeAnimation<TK, TA> : BaseKeyframeAnimation<TK, TA>
    {
        private readonly LottieFrameInfo<TA> _frameInfo = new LottieFrameInfo<TA>();

        internal ValueCallbackKeyframeAnimation(ILottieValueCallback<TA> valueCallback)
            : base(new List<Keyframe<TK>>())
        {
            SetValueCallback(valueCallback);
        }

        /// <summary>
        /// Gets the end progress of the animation. If this doesn't return 1, then <see cref="BaseKeyframeAnimation{TK, TA}.Progress"/> will always clamp the progress
        /// to 0.
        /// </summary>
        protected override float EndProgress => 1f;

        public override void OnValueChanged()
        {
            if (ValueCallback != null)
            {
                base.OnValueChanged();
            }
        }

        public override TA Value => ValueCallback.GetValueInternal(0f, 0f, default(TA), default(TA), Progress, Progress, Progress);

        public override TA GetValue(Keyframe<TK> keyframe, float keyframeProgress)
        {
            return Value;
        }
    }
}