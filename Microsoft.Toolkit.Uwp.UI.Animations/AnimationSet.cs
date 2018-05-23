// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
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
        private List<AnimationSet> _animationSets;

        private Compositor _compositor;
        private CompositionScopedBatch _batch;
        private Dictionary<string, CompositionAnimation> _compositionAnimations;
        private List<EffectAnimationDefinition> _compositionEffectAnimations;
        private Dictionary<string, object> _directCompositionPropertyChanges;
        private List<EffectDirectPropertyChangeDefinition> _directCompositionEffectPropertyChanges;

        private Storyboard _storyboard;
        private Dictionary<string, Timeline> _storyboardAnimations;

        private List<AnimationTask> _animationTasks;

        private TaskCompletionSource<bool> _animationTCS;

        private bool _storyboardCompleted;
        private bool _compositionCompleted;

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
        /// Gets the current state of the AnimationSet
        /// </summary>
        public AnimationSetState State { get; private set; }

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
            State = AnimationSetState.NotStarted;
            _compositor = Visual.Compositor;

            _compositionAnimations = new Dictionary<string, CompositionAnimation>();
            _compositionEffectAnimations = new List<EffectAnimationDefinition>();
            _directCompositionPropertyChanges = new Dictionary<string, object>();
            _directCompositionEffectPropertyChanges = new List<EffectDirectPropertyChangeDefinition>();
            _animationSets = new List<AnimationSet>();
            _storyboard = new Storyboard();
            _storyboardAnimations = new Dictionary<string, Timeline>();
            _animationTasks = new List<AnimationTask>();
        }

        /// <summary>
        /// Occurs when all animations have completed
        /// </summary>
        public event EventHandler<AnimationSetCompletedEventArgs> Completed;

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
        public async Task<bool> StartAsync()
        {
            if (_animationTCS == null || _animationTCS.Task.IsCompleted)
            {
                if (_animationTCS != null && _animationTCS.Task.IsCompleted)
                {
                    foreach (var set in _animationSets)
                    {
                        set.State = AnimationSetState.NotStarted;
                        set._animationTCS = null;
                    }
                }

                State = AnimationSetState.Running;
                _animationTCS = new TaskCompletionSource<bool>();
            }
            else
            {
                return await _animationTCS.Task;
            }

            foreach (var set in _animationSets)
            {
                if (set.State != AnimationSetState.Completed)
                {
                    var completed = await set.StartAsync();

                    if (!completed)
                    {
                        // the animation was stopped
                        return await _animationTCS.Task;
                    }
                }
            }

            foreach (var task in _animationTasks)
            {
                if (task.Task != null && !task.Task.IsCompleted)
                {
                    await task.Task;
                }

                // if _animationSet is stopped while task was running
                if (State == AnimationSetState.Stopped)
                {
                    return await _animationTCS.Task;
                }
            }

            _animationTasks.Clear();

            foreach (var property in _directCompositionPropertyChanges)
            {
                typeof(Visual).GetProperty(property.Key).SetValue(Visual, property.Value);
            }

            foreach (var definition in _directCompositionEffectPropertyChanges)
            {
                definition.EffectBrush.Properties.InsertScalar(definition.PropertyName, definition.Value);
            }

            if (_compositionAnimations.Count > 0 || _compositionEffectAnimations.Count > 0)
            {
                if (_batch != null)
                {
                    if (!_batch.IsEnded)
                    {
                        _batch.End();
                    }

                    _batch.Completed -= Batch_Completed;
                }

                _batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                _batch.Completed += Batch_Completed;

                foreach (var anim in _compositionAnimations)
                {
                    Visual.StartAnimation(anim.Key, anim.Value);
                }

                foreach (var effect in _compositionEffectAnimations)
                {
                    effect.EffectBrush.StartAnimation(effect.PropertyName, effect.Animation);
                }

                _compositionCompleted = false;
                _batch.End();
            }
            else
            {
                _compositionCompleted = true;
            }

            if (_storyboardAnimations.Count > 0)
            {
                _storyboardCompleted = false;

                _storyboard.Completed -= Storyboard_Completed;
                _storyboard.Completed += Storyboard_Completed;

                _storyboard.Begin();
            }
            else
            {
                _storyboardCompleted = true;
            }

            HandleCompleted();

            return await _animationTCS.Task;
        }

        /// <summary>
        /// Stops all animations.
        /// </summary>
        public void Stop()
        {
            foreach (var set in _animationSets)
            {
                if (set.State != AnimationSetState.Completed)
                {
                    set.Stop();
                }
            }

            if (_batch != null)
            {
                if (!_batch.IsEnded)
                {
                    _batch.End();
                }

                _batch.Completed -= Batch_Completed;
            }

            foreach (var anim in _compositionAnimations)
            {
                Visual.StopAnimation(anim.Key);
            }

            foreach (var effect in _compositionEffectAnimations)
            {
                effect.EffectBrush.StopAnimation(effect.PropertyName);
            }

            _storyboard.Pause();

            HandleCompleted(true);
            _animationTCS = null;
        }

        /// <summary>
        /// Wait for existing animations to complete before running new animations
        /// </summary>
        /// <returns>AnimationSet to allow chaining</returns>
        public AnimationSet Then()
        {
            var savedAnimationSet = new AnimationSet(Element);
            savedAnimationSet._compositionAnimations = _compositionAnimations;
            savedAnimationSet._compositionEffectAnimations = _compositionEffectAnimations;
            savedAnimationSet._directCompositionPropertyChanges = _directCompositionPropertyChanges;
            savedAnimationSet._directCompositionEffectPropertyChanges = _directCompositionEffectPropertyChanges;
            savedAnimationSet._storyboard = _storyboard;
            savedAnimationSet._storyboardAnimations = _storyboardAnimations;

            _animationTasks.ForEach(t => t.AnimationSet = savedAnimationSet);
            savedAnimationSet._animationTasks = _animationTasks;

            _animationSets.Add(savedAnimationSet);

            _compositionAnimations = new Dictionary<string, CompositionAnimation>();
            _compositionEffectAnimations = new List<EffectAnimationDefinition>();
            _directCompositionPropertyChanges = new Dictionary<string, object>();
            _directCompositionEffectPropertyChanges = new List<EffectDirectPropertyChangeDefinition>();
            _storyboard = new Storyboard();
            _storyboardAnimations = new Dictionary<string, Timeline>();
            _animationTasks = new List<AnimationTask>();

            return this;
        }

        /// <summary>
        /// Overwrites the duration on all animations after last Then()
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
        /// Overwrites the duration on all animations after last Then()
        /// to the specified value
        /// </summary>
        /// <param name="duration"><see cref="TimeSpan"/> for the duration</param>
        /// <returns>AnimationSet to allow chaining</returns>
        public AnimationSet SetDuration(TimeSpan duration)
        {
            foreach (var task in _animationTasks)
            {
                task.Duration = duration;
            }

            foreach (var anim in _compositionAnimations)
            {
                var animation = anim.Value as KeyFrameAnimation;
                if (animation != null)
                {
                    animation.Duration = duration;
                }
            }

            foreach (var effect in _compositionEffectAnimations)
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
        /// Overwrites the duration on all animations to the specified value
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
        /// Overwrites the duration on all animations to the specified value
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
        /// Overwrites the delay time on all animations after last Then()
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
        /// Overwrites the delay time on all animations after last Then()
        /// to the specified value
        /// </summary>
        /// <param name="delayTime"><see cref="TimeSpan"/> for how much to delay</param>
        /// <returns>AnimationSet to allow chaining</returns>
        public AnimationSet SetDelay(TimeSpan delayTime)
        {
            foreach (var task in _animationTasks)
            {
                task.Delay = delayTime;
            }

            foreach (var anim in _compositionAnimations)
            {
                var animation = anim.Value as KeyFrameAnimation;
                if (animation != null)
                {
                    animation.DelayTime = delayTime;
                }
            }

            foreach (var effect in _compositionEffectAnimations)
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
        /// Overwrites the delay time on all animations to the specified value
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
        /// Overwrites the delay time on all animations to the specified value
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
            _compositionAnimations[propertyName] = animation;
        }

        /// <summary>
        /// Removes a composition animation from being run on <see cref="Visual"/> property
        /// </summary>
        /// <param name="propertyName">The property that no longer needs to be animated</param>
        public void RemoveCompositionAnimation(string propertyName)
        {
            if (_compositionAnimations.ContainsKey(propertyName))
            {
                _compositionAnimations.Remove(propertyName);
            }
        }

        /// <summary>
        /// Adds a composition effect animation to be run on backing <see cref="Visual"/>
        /// </summary>
        /// <param name="effectBrush">The <see cref="CompositionEffectBrush"/> that will have a property animated</param>
        /// <param name="animation">The animation to be applied</param>
        /// <param name="propertyName">The property of the effect to be animated</param>
        public void AddCompositionEffectAnimation(CompositionObject effectBrush, CompositionAnimation animation, string propertyName)
        {
            var effect = new EffectAnimationDefinition()
            {
                EffectBrush = effectBrush,
                Animation = animation,
                PropertyName = propertyName
            };

            _compositionEffectAnimations.Add(effect);
        }

        /// <summary>
        /// Adds a composition property that will change instantaneously
        /// </summary>
        /// <param name="propertyName">The property to be animated on the backing Visual</param>
        /// <param name="value">The value to be applied</param>
        public void AddCompositionDirectPropertyChange(string propertyName, object value)
        {
            _directCompositionPropertyChanges[propertyName] = value;
        }

        /// <summary>
        /// Removes a composition property change
        /// </summary>
        /// <param name="propertyName">The property that no longer needs to be changed</param>
        public void RemoveCompositionDirectPropertyChange(string propertyName)
        {
            if (_directCompositionPropertyChanges.ContainsKey(propertyName))
            {
                _directCompositionPropertyChanges.Remove(propertyName);
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
            _animationTCS = null;
        }

        /// <summary>
        /// Adds a <see cref="AnimationTask"/> to the AnimationSet that
        /// will run add an animation once completed. Useful when an animation
        /// needs to do asynchronous initialization before running
        /// </summary>
        /// <param name="animationTask">The <see cref="AnimationTask"/> to be added</param>
        internal void AddAnimationThroughTask(AnimationTask animationTask)
        {
            _animationTasks.Add(animationTask);
        }

        /// <summary>
        /// Adds an effect property change to be run on <see cref="StartAsync"/>
        /// </summary>
        /// <param name="effectBrush">The <see cref="CompositionObject"/> that will have a property changed</param>
        /// <param name="value">The value to be applied</param>
        /// <param name="propertyName">The property of the effect to be animated</param>
        internal void AddEffectDirectPropertyChange(CompositionObject effectBrush, float value, string propertyName)
        {
            var definition = new EffectDirectPropertyChangeDefinition()
            {
                EffectBrush = effectBrush,
                Value = value,
                PropertyName = propertyName
            };

            _directCompositionEffectPropertyChanges.Add(definition);
        }

        private void Storyboard_Completed(object sender, object e)
        {
            _storyboardCompleted = true;
            _storyboard.Completed -= Storyboard_Completed;
            HandleCompleted();
        }

        private void Batch_Completed(object sender, CompositionBatchCompletedEventArgs args)
        {
            _compositionCompleted = true;
            _batch.Completed -= Batch_Completed;
            HandleCompleted();
        }

        private void HandleCompleted(bool stopped = false)
        {
            var completed = _storyboardCompleted && _compositionCompleted;

            if (!completed && !stopped)
            {
                return;
            }

            if (_storyboardCompleted && _compositionCompleted)
            {
                State = AnimationSetState.Completed;
            }
            else
            {
                State = AnimationSetState.Stopped;
            }

            if (_animationTCS != null && !_animationTCS.Task.IsCompleted)
            {
                Completed?.Invoke(this, new AnimationSetCompletedEventArgs() { Completed = _storyboardCompleted && _compositionCompleted });
                _animationTCS.SetResult(State == AnimationSetState.Completed);
            }
        }
    }
}
