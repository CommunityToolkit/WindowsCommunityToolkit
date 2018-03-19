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
using System.Threading;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// Base class for value animators
    /// </summary>
    public abstract partial class ValueAnimator : Animator, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAnimator"/> class.
        /// </summary>
        protected internal ValueAnimator()
        {
            _interpolator = new AccelerateDecelerateInterpolator();
        }

        /// <summary>
        /// The <seealso cref="EventArgs"/> implementation for the <seealso cref="Update"/> event.
        /// </summary>
        public class ValueAnimatorUpdateEventArgs : EventArgs
        {
            /// <summary>
            /// Gets the <see cref="ValueAnimator"/> that this <see cref="ValueAnimatorUpdateEventArgs"/> is associated with.
            /// </summary>
            public ValueAnimator Animation { get; }

            internal ValueAnimatorUpdateEventArgs(ValueAnimator animation)
            {
                Animation = animation;
            }
        }

        /// <summary>
        /// This event will be invoked whenever the <seealso cref="DoFrame"/> method is executed.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// This event will be invoked whenever the frame of the animation changes.
        /// </summary>
        public event EventHandler<ValueAnimatorUpdateEventArgs> Update;

        /// <summary>
        /// Clears the <seealso cref="Update"/> event handler.
        /// </summary>
        public void RemoveAllUpdateListeners()
        {
            Update = null;
        }

        /// <summary>
        /// Clears the <seealso cref="ValueChanged"/> event handler.
        /// </summary>
        public void RemoveAllListeners()
        {
            ValueChanged = null;
        }

        private IInterpolator _interpolator;
        private Timer _timer;

        /// <summary>
        /// Gets or sets the current frame rate that this animation is being executed
        /// </summary>
        public abstract float FrameRate { get; set; }

        /// <summary>
        /// Method that invokes the <seealso cref="ValueChanged"/> event.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets or sets the repeat count of this <see cref="ValueAnimator"/>
        /// </summary>
        public int RepeatCount { get; set; }

        /// <summary>
        /// Gets the number of times that this animation finished since the last call to <seealso cref="PlayAnimation"/>.
        /// </summary>
        public abstract int TimesRepeated { get; }

        /// <summary>
        /// Starts the animation from the beginning
        /// </summary>
        public abstract void PlayAnimation();

        /// <summary>
        /// Gets or sets the <see cref="RepeatMode"/> of this <see cref="ValueAnimator"/>
        /// </summary>
        public RepeatMode RepeatMode { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ValueAnimator"/> is running or not
        /// </summary>
        public override bool IsRunning => _timer != null;

        /// <summary>
        /// Gets or sets the current <see cref="IInterpolator"/> associated with this <see cref="ValueAnimator"/>. The default one is a linear interpolation.
        /// </summary>
        public virtual IInterpolator Interpolator
        {
            get => _interpolator;
            set
            {
                if (value == null)
                {
                    value = new LinearInterpolator();
                }

                _interpolator = value;
            }
        }

        /// <summary>
        /// Gets the current value of the currently playing animation.
        /// </summary>
        public abstract float AnimatedFraction { get; }

        /// <summary>
        /// Method that invokes the <seealso cref="Update"/> event.
        /// </summary>
        protected void OnAnimationUpdate()
        {
            Update?.Invoke(this, new ValueAnimatorUpdateEventArgs(this));
        }

        /// <summary>
        /// Starts the animation, by creating an internal Timer, if it doesn't already exists.
        /// </summary>
        protected internal void PrivateStart()
        {
            if (_timer == null)
            {
                _timer = new Timer(TimerCallback, null, TimeSpan.Zero, GetTimerInterval());
            }
        }

        /// <summary>
        /// Changes the current Timer interval, if it is already instantiated.
        /// </summary>
        protected internal void UpdateTimerInterval()
        {
            _timer?.Change(TimeSpan.Zero, GetTimerInterval());
        }

        private TimeSpan GetTimerInterval()
        {
            return TimeSpan.FromTicks((long)Math.Floor(TimeSpan.TicksPerSecond / (decimal)FrameRate));
        }

        /// <summary>
        /// Clears and disposes the internal timer, stoping the animation.
        /// </summary>
        protected internal virtual void RemoveFrameCallback()
        {
            _timer?.Dispose();
            _timer = null;
        }

        private void TimerCallback(object state)
        {
            DoFrame();
        }

        /// <summary>
        /// Progress the current frame of this animation, based on the amount of time that elapsed since it's last execution
        /// </summary>
        public virtual void DoFrame()
        {
            OnValueChanged();
        }

        internal static long SystemnanoTime()
        {
            long nano = 10000L * DateTime.Now.Ticks;
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }

        private void Dispose(bool disposing)
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ValueAnimator"/> class.
        /// </summary>
        ~ValueAnimator()
        {
            Dispose(false);
        }
    }
}