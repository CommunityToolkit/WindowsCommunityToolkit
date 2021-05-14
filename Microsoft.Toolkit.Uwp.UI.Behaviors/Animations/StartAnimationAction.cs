// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// An <see cref="IAction"/> implementation that can trigger a target <see cref="AnimationSet"/> instance.
    /// </summary>
    public sealed class StartAnimationAction : DependencyObject, IAction
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
            typeof(StartAnimationAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the object to start the specified animation on. If not specified, will use the current object the parent animation is running on.
        /// </summary>
        public UIElement TargetObject
        {
            get => (UIElement)GetValue(TargetObjectProperty);
            set => SetValue(TargetObjectProperty, value);
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
        public object Execute(object sender, object parameter)
        {
            if (Animation is null)
            {
                ThrowArgumentNullException();
            }

            if (TargetObject is not null)
            {
                Animation.Start(TargetObject);
            }
            else
            {
                Animation.Start();
            }

            return null!;

            static void ThrowArgumentNullException() => throw new ArgumentNullException(nameof(Animation));
        }
    }
}