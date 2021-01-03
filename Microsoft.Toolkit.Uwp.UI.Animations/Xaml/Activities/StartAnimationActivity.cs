// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Toolkit.Diagnostics;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// <see cref="IActivity"/> which Starts the provided <see cref="Animation"/> when invoked.
    /// </summary>
    public class StartAnimationActivity : Activity
    {
        /// <summary>
        /// Gets or sets the linked <see cref="AnimationSet"/> instance to invoke.
        /// </summary>
        public AnimationSet Animation
        {
            get => (AnimationSet)GetValue(AnimationProperty);
            set => SetValue(AnimationProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Animation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimationProperty = DependencyProperty.Register(
            nameof(Animation),
            typeof(AnimationSet),
            typeof(StartAnimationActivity),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the object to start the specified animation on. If not specified, will use the current object the parent animation is running on.
        /// </summary>
        public UIElement TargetObject
        {
            get { return (UIElement)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        /// <summary>
        /// Identifies the <seealso cref="TargetObject"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
            nameof(TargetObject),
            typeof(UIElement),
            typeof(StartAnimationActivity),
            new PropertyMetadata(null));

        /// <inheritdoc/>
        public override async Task InvokeAsync(UIElement element)
        {
            Guard.IsNotNull(Animation, nameof(Animation));

            await base.InvokeAsync(element);

            // If we've specified an explicit target for the animation, we can use that. Otherwise, we can
            // check whether the target animation has an implicit parent. If that's the case, we will use
            // that to invoke the animation, or just use the input (usually the parent) as fallback.
            if (TargetObject is not null)
            {
                await Animation.StartAsync(TargetObject);
            }
            else if (Animation.ParentReference is null)
            {
                await Animation.StartAsync(element);
            }
            else
            {
                await Animation.StartAsync();
            }
        }
    }
}
