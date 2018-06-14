// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Abstract class providing common dependency properties for composition animations
    /// </summary>
    [ContentProperty(Name = nameof(KeyFrames))]
    public abstract class AnimationBase : DependencyObject
    {
        /// <summary>
        /// Identifies the <see cref="Target"/> property
        /// </summary>
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register(nameof(Target), typeof(string), typeof(AnimationBase), new PropertyMetadata(null, OnAnimationPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="Duration"/> property
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration), typeof(TimeSpan), typeof(AnimationBase), new PropertyMetadata(TimeSpan.FromMilliseconds(400), OnAnimationPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="KeyFrames"/> property
        /// </summary>
        public static readonly DependencyProperty KeyFramesProperty =
            DependencyProperty.Register(nameof(KeyFrames), typeof(KeyFrameCollection), typeof(AnimationBase), new PropertyMetadata(null, OnAnimationPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="ImplicitTarget"/> property
        /// </summary>
        public static readonly DependencyProperty ImplicitTargetProperty =
            DependencyProperty.Register(nameof(ImplicitTarget), typeof(string), typeof(AnimationBase), new PropertyMetadata(null, OnAnimationPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="Delay"/> property
        /// </summary>
        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register(nameof(Delay), typeof(TimeSpan), typeof(AnimationBase), new PropertyMetadata(TimeSpan.Zero, OnAnimationPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="SetInitialValueBeforeDelay"/> property
        /// </summary>
        public static readonly DependencyProperty SetInitialValueBeforeDelayProperty =
            DependencyProperty.Register(nameof(SetInitialValueBeforeDelay), typeof(bool), typeof(AnimationBase), new PropertyMetadata(false, OnAnimationPropertyChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationBase"/> class.
        /// </summary>
        public AnimationBase()
        {
            if (KeyFrames == null)
            {
                KeyFrames = new KeyFrameCollection();
            }
        }

        /// <summary>
        /// Raised when a property changes
        /// </summary>
        public event EventHandler AnimationChanged;

        /// <summary>
        /// Gets or sets the duration of the animation
        /// </summary>
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="KeyFrameCollection"/> of the animations
        /// </summary>
        public KeyFrameCollection KeyFrames
        {
            get { return (KeyFrameCollection)GetValue(KeyFramesProperty); }
            set { SetValue(KeyFramesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the target property to be animated
        /// </summary>
        public string Target
        {
            get { return (string)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the property that should start the implicit animation
        /// </summary>
        public string ImplicitTarget
        {
            get { return (string)GetValue(ImplicitTargetProperty); }
            set { SetValue(ImplicitTargetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the delay of the animation
        /// </summary>
        public TimeSpan Delay
        {
            get { return (TimeSpan)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the value at keyframe 0 should be set before the delay
        /// </summary>
        public bool SetInitialValueBeforeDelay
        {
            get { return (bool)GetValue(SetInitialValueBeforeDelayProperty); }
            set { SetValue(SetInitialValueBeforeDelayProperty, value); }
        }

        /// <summary>
        /// Starts the animation on the specified element
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to be animated</param>
        public void StartAnimation(UIElement element)
        {
            if (element == null)
            {
                return;
            }

            var visual = ElementCompositionPreview.GetElementVisual(element);
            var compositor = visual.Compositor;

            if (Target.Contains("Translation"))
            {
                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
            }

            var compositionAnimation = GetCompositionAnimation(compositor);
            visual.StartAnimation(Target, compositionAnimation);
        }

        /// <summary>
        /// Gets a <see cref="CompositionAnimation"/> that can be used on the Composition layer
        /// </summary>
        /// <param name="compositor">The <see cref="Compositor"/> to use to create the animation</param>
        /// <returns><see cref="CompositionAnimation"/></returns>
        public abstract CompositionAnimation GetCompositionAnimation(Compositor compositor);

        /// <summary>
        /// Called when any property of the animation changes
        /// </summary>
        protected void OnAnimationChanged()
        {
            AnimationChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when any property of the animation changes
        /// </summary>
        /// <param name="d">The animation where a property has changed</param>
        /// <param name="e">The details about the property change</param>
        private static void OnAnimationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AnimationBase).OnAnimationChanged();
        }
    }
}
