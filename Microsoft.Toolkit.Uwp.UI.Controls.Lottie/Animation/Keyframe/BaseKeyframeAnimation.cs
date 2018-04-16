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
    // TK = Keyframe type
    // TA = Animation type
    internal abstract class BaseKeyframeAnimation<TK, TA> : IBaseKeyframeAnimation<TK, TA>
    {
        private readonly List<Keyframe<TK>> _keyframes;

        public virtual event EventHandler ValueChanged;

        private bool _isDiscrete;

        private float _progress;

        protected ILottieValueCallback<TA> ValueCallback { get; set; }

        private Keyframe<TK> _cachedKeyframe;

        internal BaseKeyframeAnimation(List<Keyframe<TK>> keyframes)
        {
            _keyframes = keyframes;
        }

        internal virtual void SetIsDiscrete()
        {
            _isDiscrete = true;
        }

        public virtual float Progress
        {
            get => _progress;
            set
            {
                if (value < 0 || float.IsNaN(value))
                {
                    value = 0;
                }

                if (value > 1)
                {
                    value = 1;
                }

                if (value < StartDelayProgress)
                {
                    value = StartDelayProgress;
                }
                else if (value > EndProgress)
                {
                    value = EndProgress;
                }

                if (value == _progress)
                {
                    return;
                }

                _progress = value;

                OnValueChanged();
            }
        }

        public virtual void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private Keyframe<TK> CurrentKeyframe
        {
            get
            {
                if (_keyframes.Count == 0)
                {
                    throw new InvalidOperationException("There are no keyframes");
                }

                if (_cachedKeyframe != null && _cachedKeyframe.ContainsProgress(_progress))
                {
                    return _cachedKeyframe;
                }

                var keyframe = _keyframes[_keyframes.Count - 1];
                if (_progress < keyframe.StartProgress)
                {
                    for (int i = _keyframes.Count - 1; i >= 0; i--)
                    {
                        keyframe = _keyframes[i];
                        if (keyframe.ContainsProgress(_progress))
                        {
                            break;
                        }
                    }
                }

                _cachedKeyframe = keyframe;
                return keyframe;
            }
        }

        /// <summary>
        /// Gets the progress into the current keyframe between 0 and 1. This does not take into account
        /// any interpolation that the keyframe may have.
        /// </summary>
        protected float LinearCurrentKeyframeProgress
        {
            get
            {
                if (_isDiscrete)
                {
                    return 0f;
                }

                var keyframe = CurrentKeyframe;
                if (keyframe.Static)
                {
                    return 0f;
                }

                var progressIntoFrame = _progress - keyframe.StartProgress;
                var keyframeProgress = keyframe.EndProgress - keyframe.StartProgress;
                return progressIntoFrame / keyframeProgress;
            }
        }

        /// <summary>
        /// Gets the value of <see cref="LinearCurrentKeyframeProgress"/> and interpolates it with
        /// the current keyframe's interpolator.
        /// </summary>
        private float InterpolatedCurrentKeyframeProgress
        {
            get
            {
                var keyframe = CurrentKeyframe;
                if (keyframe.Static)
                {
                    return 0f;
                }

                return keyframe.Interpolator.GetInterpolation(LinearCurrentKeyframeProgress);
            }
        }

        private float StartDelayProgress
        {
            get
            {
                var startDelayProgress = _keyframes.Count == 0 ? 0f : _keyframes[0].StartProgress;
                if (startDelayProgress < 0)
                {
                    return 0;
                }

                if (startDelayProgress > 1)
                {
                    return 1;
                }

                return startDelayProgress;
            }
        }

        protected virtual float EndProgress
        {
            get
            {
                var endProgress = _keyframes.Count == 0 ? 1f : _keyframes[_keyframes.Count - 1].EndProgress;
                if (endProgress < 0)
                {
                    return 0;
                }

                if (endProgress > 1)
                {
                    return 1;
                }

                return endProgress;
            }
        }

        public virtual TA Value => GetValue(CurrentKeyframe, InterpolatedCurrentKeyframeProgress);

        public void SetValueCallback(ILottieValueCallback<TA> valueCallback)
        {
            ValueCallback?.SetAnimation(null);
            ValueCallback = valueCallback;
            valueCallback?.SetAnimation(this);
        }

        /// <summary>
        /// keyframeProgress will be [0, 1] unless the interpolator has overshoot in which case, this
        /// should be able to handle values outside of that range.
        /// </summary>
        /// <returns>Returns the </returns>
        public abstract TA GetValue(Keyframe<TK> keyframe, float keyframeProgress);
    }
}