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
    internal class IntegerKeyframeAnimation : KeyframeAnimation<int?>
    {
        internal IntegerKeyframeAnimation(List<Keyframe<int?>> keyframes)
            : base(keyframes)
        {
        }

        public override int? GetValue(Keyframe<int?> keyframe, float keyframeProgress)
        {
            if (keyframe.StartValue == null || keyframe.EndValue == null)
            {
                throw new System.InvalidOperationException("Missing values for keyframe.");
            }

            if (ValueCallback != null)
            {
                return ValueCallback.GetValueInternal(keyframe.StartFrame.Value, keyframe.EndFrame.Value, keyframe.StartValue, keyframe.EndValue, keyframeProgress, LinearCurrentKeyframeProgress, Progress);
            }

            return (int?)MathExt.Lerp(keyframe.StartValue.Value, keyframe.EndValue.Value, keyframeProgress);
        }
    }
}