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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils
{
    internal abstract class BaseLottieAnimator : ValueAnimator
    {
        public long StartDelay
        {
            get => throw new Exception("LottieAnimator does not support getStartDelay.");
            set => throw new Exception("LottieAnimator does not support setStartDelay.");
        }

        public override long Duration
        {
            set => throw new Exception("LottieAnimator does not support setDuration.");
        }

        public override IInterpolator Interpolator
        {
            set => throw new Exception("LottieAnimator does not support setInterpolator.");
        }

        public event EventHandler<LottieAnimatorStartEventArgs> AnimationStart;

        public event EventHandler<LottieAnimatorEndEventArgs> AnimationEnd;

        public event EventHandler AnimationCancel;

        public event EventHandler AnimationRepeat;

        public class LottieAnimatorStartEventArgs : EventArgs
        {
            public bool IsReverse { get; }

            public LottieAnimatorStartEventArgs(bool isReverse)
            {
                IsReverse = isReverse;
            }
        }

        public class LottieAnimatorEndEventArgs : EventArgs
        {
            public bool IsReverse { get; }

            public LottieAnimatorEndEventArgs(bool isReverse)
            {
                IsReverse = isReverse;
            }
        }

        public virtual void OnAnimationStart(bool isReverse)
        {
            AnimationStart?.Invoke(this, new LottieAnimatorStartEventArgs(isReverse));
        }

        public virtual void OnAnimationRepeat()
        {
            AnimationRepeat?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnAnimationEnd(bool isReverse)
        {
            AnimationEnd?.Invoke(this, new LottieAnimatorEndEventArgs(isReverse));
        }

        public virtual void OnAnimationCancel()
        {
            AnimationCancel?.Invoke(this, EventArgs.Empty);
        }
    }
}
