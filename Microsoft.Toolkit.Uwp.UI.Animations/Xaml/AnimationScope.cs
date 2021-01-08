// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A container of <see cref="ITimeline"/> elements that can be used to conceptually group animations
    /// together and to assign shared properties to be applied to all the contained items automatically.
    /// </summary>
    [ContentProperty(Name = nameof(Animations))]
    public sealed class AnimationScope : Animation
    {
        /// <summary>
        /// Gets or sets the list of animations in the current scope.
        /// </summary>
        public IList<Animation> Animations
        {
            get
            {
                if (GetValue(AnimationsProperty) is not IList<Animation> animations)
                {
                    animations = new List<Animation>();

                    SetValue(AnimationsProperty, animations);
                }

                return animations;
            }
            set => SetValue(AnimationsProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Animations"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimationsProperty = DependencyProperty.Register(
            nameof(Animations),
            typeof(IList<Animation>),
            typeof(AnimationScope),
            new PropertyMetadata(null));

        /// <inheritdoc/>
        public override AnimationBuilder AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
        {
            foreach (ITimeline element in Animations)
            {
                builder = element.AppendToBuilder(builder, Delay ?? delayHint, Duration ?? durationHint, EasingType ?? easingTypeHint, EasingMode ?? easingModeHint);
            }

            return builder;
        }
    }
}
