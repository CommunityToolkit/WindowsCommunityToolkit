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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe
{
    internal class ScaleKeyframeAnimation : KeyframeAnimation<ScaleXy>
    {
        internal ScaleKeyframeAnimation(List<Keyframe<ScaleXy>> keyframes)
            : base(keyframes)
        {
        }

        public override ScaleXy GetValue(Keyframe<ScaleXy> keyframe, float keyframeProgress)
        {
            if (keyframe.StartValue == null || keyframe.EndValue == null)
            {
                throw new InvalidOperationException("Missing values for keyframe.");
            }

            var startTransform = keyframe.StartValue;
            var endTransform = keyframe.EndValue;

            if (ValueCallback != null)
            {
                return ValueCallback.GetValueInternal(keyframe.StartFrame.Value, keyframe.EndFrame.Value, startTransform, endTransform, keyframeProgress, LinearCurrentKeyframeProgress, Progress);
            }

            return new ScaleXy(MathExt.Lerp(startTransform.ScaleX, endTransform.ScaleX, keyframeProgress), MathExt.Lerp(startTransform.ScaleY, endTransform.ScaleY, keyframeProgress));
        }
    }
}