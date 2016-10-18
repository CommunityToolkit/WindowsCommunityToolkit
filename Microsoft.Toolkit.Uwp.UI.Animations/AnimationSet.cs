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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Defines an object for storing and managing CompositionAnimations for an element
    /// </summary>
    public class AnimationSet : IDisposable
    {
        private Dictionary<string, CompositionAnimation> _animations;
        private List<EffectAnimationDefinition> _effectAnimations;
        private Dictionary<string, object> _directPropertyChanges;
        private List<EffectDirectPropertyChangeDefinition> _directEffectPropertyChanges;
        private List<AnimationSet> _animationSets;
        private Storyboard _storyboard;
        private Dictionary<string, Timeline> _storyboardAnimations;
        private Compositor _compositor;
        private CompositionScopedBatch _batch;
        private ManualResetEvent _manualResetEvent;

        /// <summary>
        /// Gets or sets a value indicating whether composition must be use even on SDK > 10586
        /// </summary>
        public static bool UseComposition { get; set; }

        /// <summary>
        /// Gets the <see cref="Visual"/> object that backs the XAML element
        /// </summary>
        public Visual Visual { get; private set; }

        /// <summary>
        /// Gets the <see cref="UIElement"/>
        /// </summary>
        public UIElement Element { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationSet"/> class.
        /// </summary>
        /// <param name="element">The associated element</param>
        public AnimationSet(UIElement element)
        {
            if (element == null)
            {
                throw new NullReferenceException("Element must not be null");
            }

            var visual = ElementCompositionPreview.GetElementVisual(element);

            if (visual == null)
            {
                throw new NullReferenceException("Visual must not be null");
            }

            Visual = visual;
            if (Visual.Compositor == null)
            {
                throw new NullReferenceException("Visual must have a compositor");
            }

            Element = element;
            _compositor = Visual.Compositor;
            _animations = new Dictionary<string, CompositionAnimation>();
            _effectAnimations = new List<EffectAnimationDefinition>();
            _manualResetEvent = new ManualResetEvent(false);
            _directPropertyChanges = new Dictionary<string, object>();
            _directEffectPropertyChanges = new List<EffectDirectPropertyChangeDefinition>();
            _animationSets = new List<AnimationSet>();
            _storyboard = new Storyboard();
            _storyboardAnimations = new Dictionary<string, Timeline>();
        }

        /// <summary>
        /// Occurs when all animations have completed
        /// </summary>
        public event EventHandler Completed;

        /// <summary>
        /// Stats all animations. This method is not awaitable.
        /// </summary>
        public async void Start()
        {
            await StartAsync();
        }

        /// <summary>
        /// Starts all animations and returns an awaitable task.
        /// </summary>
        /// <returns>A <see cref="Task"/> that can be awaited until all animations have completed</returns>
        public async Task StartAsync()
        {
            foreach (var set in _animationSets)
            {
                await set.StartAsync();
            }

            if (_batch != null)
            {
                if (!_batch.IsEnded)
                {
                    _batch.End();
                }

                _batch.Completed -= Batch_Completed;
            }

            foreach (var property in _directPropertyChanges)
            {
                typeof(Visual).GetProperty(property.Key).SetValue(Visual, property.Value);
            }

            foreach (var definition in _directEffectPropertyChanges)
            {
                definition.EffectBrush.Properties.InsertScalar(definition.PropertyName, definition.Value);
            }

            List<Task> tasks = new List<Task>();

            if (_animations.Count > 0 || _effectAnimations.Count > 0)
            {
                _batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                _batch.Completed += Batch_Completed;

                foreach (var anim in _animations)
                {
                    Visual.StartAnimation(anim.Key, anim.Value);
                }

                foreach (var effect in _effectAnimations)
                {
                    effect.EffectBrush.StartAnimation(effect.PropertyName, effect.Animation);
                }

                Task compositionTask = Task.Run(() =>
                {
                    _manualResetEvent.Reset();
                    _manualResetEvent.WaitOne();
                });

                _batch.End();

                tasks.Add(compositionTask);
            }

            tasks.Add(_storyboard.BeginAsync());

            await Task.WhenAll(tasks);
            Completed?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Stops all animations.
        /// </summary>
        public void Stop()
        {
            foreach (var set in _animationSets)
            {
                set.Stop();
            }

            if (_batch != null)
            {
                if (!_batch.IsEnded)
                {
                    _batch.End();
                }

                _batch.Completed -= Batch_Completed;
            }

            foreach (var anim in _animations)
            {
                Visual.StopAnimation(anim.Key);
            }

            foreach (var effect in _effectAnimations)
            {
                effect.EffectBrush.StopAnimation(effect.PropertyName);
            }

            _storyboard.Pause();
        }

        /// <summary>
        /// Wait for existing animations to complete before running new animations
        /// </summary>
        /// <returns>AnimationSet to allow chaining</returns>
        public AnimationSet Then()
        {
            var savedAnimationSet = new AnimationSet(Element);
            savedAnimationSet._animations = _animations;
            savedAnimationSet._effectAnimations = _effectAnimations;
            savedAnimationSet._directPropertyChanges = _directPropertyChanges;
            savedAnimationSet._directEffectPropertyChanges = _directEffectPropertyChanges;
            savedAnimationSet._storyboard = _storyboard;
            savedAnimationSet._storyboardAnimations = _storyboardAnimations;

            _animationSets.Add(savedAnimationSet);

            _animations = new Dictionary<string, CompositionAnimation>();
            _effectAnimations = new List<EffectAnimationDefinition>();
            _directPropertyChanges = new Dictionary<string, object>();
            _directEffectPropertyChanges = new List<EffectDirectPropertyChangeDefinition>();
            _storyboard = new Storyboard();
            _storyboardAnimations = new Dictionary<string, Timeline>();

            return this;
        }

        /// <summary>
        /// Ovewrites the duration on all animations after last Then()
        /// to the specified value
        /// </summary>
        /// <param name="duration">The duration in milliseconds</param>
        /// <returns>AnimationSet to allow chaining</returns>
        public AnimationSet SetDuration(double duration)
        {
            if (duration <= 0)
            {
                duration = 1;
            }

            return SetDuration(TimeSpan.FromMilliseconds(duration));
        }

        /// <summary>
        /// Ovewrites the duration on all animations after last Then()
        /// to the specified value
        /// </summary>
        /// <param name="duration"><see cref="TimeSpan"/> for the duration</param>
        /// <returns>AnimationSet to allow chaining</returns>
        public AnimationSet SetDuration(TimeSpan duration)
        {
            foreach (var anim in _animations)
            {
                var animation = anim.Value as KeyFrameAnimation;
                if (animation != null)
                {
                    animation.Duration = duration;
                }
            }

            foreach (var effect in _effectAnimations)
            {
                var animation = effect.Animation as KeyFrameAnimation;
                if (animation != null)
                {
                    animation.Duration = duration;
                }
            }

            foreach (var timeline in _storyboardAnimations)
            {
                var animation = timeline.Value as DoubleAnimation;
                if (animation != null)
                {
                    animation.Duration = duration;
                }
            }

            return this;
        }

        /// <summary>
        /// Ovewrites the duration on all animations to the specified value
        /// </summary>
        /// <param name="duration">The duration in milliseconds</param>
        /// <returns>AnimationSet to allow chaining</returns>
        public AnimationSet SetDurationForAll(double duration)
        {
            foreach (var set in _animationSets)
            {
                set.SetDuration(duration);
            }

            return SetDuration(duration);
        }

        /// <summary>
        /// Ovewrites the duration on all animations to the specified value
        /// </summary>
        /// <param name="duration"><see cref="TimeSpan"/> for the duration</param>
        /// <returns>AnimationSet to allow chaining</returns>
        public AnimationSet SetDurationForAll(TimeSpan duration)
        {
            foreach (var set in _animationSets)
            {
                set.SetDuration(duration);
            }

            return SetDuration(duration);
        }

        /// <summary>
        /// Ovewrites the delay time on all animations after last Then()
        /// to the specified value
        /// </summary>
        /// <param name="delayTime">The delay time in milliseconds</param>
        /// <returns>AnimationSet to allow chaining</returns>
        public AnimationSet SetDelay(double delayTime)
        {
            if (delayTime < 0)
            {
                delayTime = 0;
            }

            return SetDelay(TimeSpan.FromMilliseconds(delayTime));
        }

        /// <summary>
        /// Ovewrites the delay time on all animations after last Then()
        /// to the specified value
        /// </summary>
        /// <param name="delayTime"><see cref="TimeSpan"/> for how much to delay</param>
        /// <returns>AnimationSet to allow chaining</returns>
        public AnimationSet SetDelay(TimeSpan delayTime)
        {
            foreach (var anim in _animations)
            {
                var animation = anim.Value as KeyFrameAnimation;
                if (animation != null)
                {
                    animation.DelayTime = delayTime;
                }
            }

            foreach (var effect in _effectAnimations)
            {
                var animation = effect.Animation as KeyFrameAnimation;
                if (animation != null)
                {
                    animation.DelayTime = delayTime;
                }
            }

            foreach (var timeline in _storyboardAnimations)
            {
                var animation = timeline.Value as DoubleAnimation;
                if (animation != null)
                {
                    animation.BeginTime = delayTime;
                }
            }

            return this;
        }

        /// <summary>
        /// Ovewrites the delay time on all animations to the specified value
        /// </summary>
        /// <param name="delayTime">The delay time in milliseconds</param>
        /// <returns>AnimationSet to allow chaining</returns>
        public AnimationSet SetDelayForAll(double delayTime)
        {
            foreach (var set in _animationSets)
            {
                set.SetDelay(delayTime);
            }

            return SetDelay(delayTime);
        }

        /// <summary>
        /// Ovewrites the delay time on all animations to the specified value
        /// </summary>
        /// <param name="delayTime"><see cref="TimeSpan"/> for how much to delay</param>
        /// <returns>AnimationSet to allow chaining</returns>
        public AnimationSet SetDelayForAll(TimeSpan delayTime)
        {
            foreach (var set in _animationSets)
            {
                set.SetDelay(delayTime);
            }

            return SetDelay(delayTime);
        }

        /// <summary>
        /// Adds a composition animation to be run on <see cref="StartAsync"/>
        /// </summary>
        /// <param name="propertyName">The property to be animated on the backing Visual</param>
        /// <param name="animation">The <see cref="CompositionAnimation"/> to be applied</param>
        public void AddCompositionAnimation(string propertyName, CompositionAnimation animation)
        {
            _animations[propertyName] = animation;
        }

        /// <summary>
        /// Removes a composition animation from being run on <see cref="Visual"/> property
        /// </summary>
        /// <param name="propertyName">The property that no longer needs to be animated</param>
        public void RemoveCompositionAnimation(string propertyName)
        {
            if (_animations.ContainsKey(propertyName))
            {
                _animations.Remove(propertyName);
            }
        }

        /// <summary>
        /// Adds a composition effect animation to be run on backing <see cref="Visual"/>
        /// </summary>
        /// <param name="effectBrush">The <see cref="CompositionEffectBrush"/> that will have a property animated</param>
        /// <param name="animation">The animation to be applied</param>
        /// <param name="propertyName">The property of the effect to be animated</param>
        public void AddCompositionEffectAnimation(CompositionEffectBrush effectBrush, CompositionAnimation animation, string propertyName)
        {
            var effect = new EffectAnimationDefinition()
            {
                EffectBrush = effectBrush,
                Animation = animation,
                PropertyName = propertyName
            };

            _effectAnimations.Add(effect);
        }

        /// <summary>
        /// Adds a composition property that will change instantaneously
        /// </summary>
        /// <param name="propertyName">The property to be animated on the backing Visual</param>
        /// <param name="value">The value to be applied</param>
        public void AddCompositionDirectPropertyChange(string propertyName, object value)
        {
            _directPropertyChanges[propertyName] = value;
        }

        /// <summary>
        /// Removes a composition property change
        /// </summary>
        /// <param name="propertyName">The property that no longer needs to be changed</param>
        public void RemoveCompositionDirectPropertyChange(string propertyName)
        {
            if (_directPropertyChanges.ContainsKey(propertyName))
            {
                _directPropertyChanges.Remove(propertyName);
            }
        }

        /// <summary>
        /// Adds a storyboard animation to be run
        /// </summary>
        /// <param name="propertyPath">The property to be animated with Storyboards</param>
        /// <param name="timeline">The timeline object to be added to storyboard</param>
        public void AddStoryboardAnimation(string propertyPath, Timeline timeline)
        {
            if (_storyboardAnimations.ContainsKey(propertyPath))
            {
                var previousAnimation = _storyboardAnimations[propertyPath];
                _storyboard.Children.Remove(previousAnimation);
                _storyboardAnimations.Remove(propertyPath);
            }

            _storyboardAnimations.Add(propertyPath, timeline);
            _storyboard.Children.Add(timeline);

            Storyboard.SetTarget(timeline, Element);
            Storyboard.SetTargetProperty(timeline, propertyPath);
        }

        /// <summary>
        /// Dispose resources.
        /// </summary>
        public void Dispose()
        {
            _manualResetEvent?.Dispose();
        }

        /// <summary>
        /// Adds an effect propety change to be run on <see cref="StartAsync"/>
        /// </summary>
        /// <param name="effectBrush">The <see cref="CompositionEffectBrush"/> that will have a property changed</param>
        /// <param name="value">The value to be applied</param>
        /// <param name="propertyName">The property of the effect to be animated</param>
        internal void AddEffectDirectPropertyChange(CompositionEffectBrush effectBrush, float value, string propertyName)
        {
            var definition = new EffectDirectPropertyChangeDefinition()
            {
                EffectBrush = effectBrush,
                Value = value,
                PropertyName = propertyName
            };

            _directEffectPropertyChanges.Add(definition);
        }

        private void Batch_Completed(object sender, CompositionBatchCompletedEventArgs args)
        {
            _manualResetEvent.Set();
        }
    }
}
