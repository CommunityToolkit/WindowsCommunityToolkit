// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A base model representing an animation that can be used in XAML.
    /// </summary>
    public abstract class Animation : DependencyObject, ITimeline
    {
        /// <summary>
        /// Gets or sets the optional initial delay for the animation.
        /// </summary>
        public TimeSpan? Delay
        {
            get => (TimeSpan?)GetValue(DelayProperty);
            set => SetValue(DelayProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Delay"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(
            nameof(Delay),
            typeof(TimeSpan?),
            typeof(Animation),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the animation duration.
        /// </summary>
        public TimeSpan? Duration
        {
            get => (TimeSpan?)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Duration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
            nameof(Duration),
            typeof(TimeSpan?),
            typeof(Animation),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the optional easing function type for the animation.
        /// </summary>
        public EasingType? EasingType
        {
            get => (EasingType?)GetValue(EasingTypeProperty);
            set => SetValue(EasingTypeProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="EasingType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EasingTypeProperty = DependencyProperty.Register(
            nameof(EasingType),
            typeof(EasingType?),
            typeof(Animation),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the optional easing function mode for the animation.
        /// </summary>
        public EasingMode? EasingMode
        {
            get => (EasingMode?)GetValue(EasingModeProperty);
            set => SetValue(EasingModeProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="EasingMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EasingModeProperty = DependencyProperty.Register(
            nameof(EasingMode),
            typeof(EasingMode?),
            typeof(Animation),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the repeat option for the animation.
        /// </summary>
        public RepeatOption Repeat
        {
            get => (RepeatOption)GetValue(RepeatOptionProperty);
            set => SetValue(RepeatOptionProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Repeat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RepeatOptionProperty = DependencyProperty.Register(
            nameof(Repeat),
            typeof(RepeatOption),
            typeof(Animation),
            new PropertyMetadata(RepeatOption.Once));

        /// <inheritdoc/>
        public abstract AnimationBuilder AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint);
    }
}
