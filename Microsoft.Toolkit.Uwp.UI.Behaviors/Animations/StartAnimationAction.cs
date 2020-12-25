// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors.Animations
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
            "Animation",
            typeof(AnimationSet),
            typeof(StartAnimationAction),
            new PropertyMetadata(null));

        /// <inheritdoc/>
        public object Execute(object sender, object parameter)
        {
            Guard.IsNotNull(Animation, nameof(Animation));

            Animation.Start();

            return null!;
        }
    }
}
