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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Defines an object for storing and managing CompositionAnimations for an element
    /// </summary>
    public class AnimationManager
    {
        private Dictionary<string, CompositionAnimation> _animations;
        private Compositor _compositor;
        private CompositionScopedBatch _batch;
        private System.Threading.ManualResetEvent _manualResetEvent;

        /// <summary>
        /// Gets the <see cref="Visual"/> object that backs the XAML element
        /// </summary>
        public Visual Visual { get; private set; }

        /// <summary>
        /// Gets the <see cref="UIElement"/>
        /// </summary>
        public UIElement Element { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationManager"/> class.
        /// </summary>
        /// <param name="element">The associated element</param>
        public AnimationManager(UIElement element)
        {
            if (element == null)
            {
                throw new NullReferenceException("element must not be null");
            }

            var visual = ElementCompositionPreview.GetElementVisual(element);

            if (visual == null)
            {
                throw new NullReferenceException("visual must not be null");
            }

            Visual = visual;
            if (Visual.Compositor == null)
            {
                throw new NullReferenceException("Visual must have a compositor");
            }

            Element = element;
            _compositor = Visual.Compositor;
            _animations = new Dictionary<string, CompositionAnimation>();
            _manualResetEvent = new System.Threading.ManualResetEvent(false);
        }

        /// <summary>
        /// Occurs when all animations have completed
        /// </summary>
        public event EventHandler AnimationsCompleted;

        /// <summary>
        /// Starts all animations on the backing Visual.
        /// </summary>
        /// <returns>A <see cref="Task"/> that can be awaited until all animations have completed</returns>
        public Task StartAsync()
        {
            if (_batch != null)
            {
                _batch.End();
                _batch.Completed -= Batch_Completed;
            }

            _batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            _batch.Completed += Batch_Completed;

            foreach (var anim in _animations)
            {
                Visual.StartAnimation(anim.Key, anim.Value);
            }

            Task t = Task.Run(() =>
            {
                _manualResetEvent.Reset();
                _manualResetEvent.WaitOne();
            });

            _batch.End();

            return t;
        }

        /// <summary>
        /// Stops all animations on the backing Visual.
        /// </summary>
        public void Stop()
        {
            if (_batch != null)
            {
                _batch.End();
                _batch.Completed -= Batch_Completed;
            }

            foreach (var anim in _animations)
            {
                Visual.StopAnimation(anim.Key);
            }
        }

        /// <summary>
        /// Ovewrites the duration on all animations to the specified value
        /// </summary>
        /// <param name="duration">The duration in seconds</param>
        public void SetDurationForAll(double duration)
        {
            foreach (var anim in _animations)
            {
                var animation = anim.Value as KeyFrameAnimation;
                if (animation != null)
                {
                    animation.Duration = TimeSpan.FromSeconds(duration);
                }
            }
        }

        /// <summary>
        /// Ovewrites the delay time on all animations to the specified value
        /// </summary>
        /// <param name="delayTime">The delay time in seconds</param>
        public void SetDelayForAll(double delayTime)
        {
            foreach (var anim in _animations)
            {
                var animation = anim.Value as KeyFrameAnimation;
                if (animation != null)
                {
                    animation.DelayTime = TimeSpan.FromSeconds(delayTime);
                }
            }
        }

        /// <summary>
        /// Adds an animation to be run on <see cref="StartAsync"/>
        /// </summary>
        /// <param name="propertyName">The property to be animated on the backing Visual</param>
        /// <param name="animation">The animation to be applied</param>
        public void AddAnimation(string propertyName, CompositionAnimation animation)
        {
            _animations[propertyName] = animation;
        }

        /// <summary>
        /// Removes an animation from being run on <see cref="StartAsync"/>
        /// </summary>
        /// <param name="propertyName">The property that no longer needs to be animated</param>
        public void RemoveAnimation(string propertyName)
        {
            if (_animations.ContainsKey(propertyName))
            {
                _animations.Remove(propertyName);
            }
        }

        private void Batch_Completed(object sender, CompositionBatchCompletedEventArgs args)
        {
            _manualResetEvent.Set();
            AnimationsCompleted?.Invoke(this, new EventArgs());
        }
    }
}
