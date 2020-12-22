// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Toolkit.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A collection of animations that can be grouped together. This type represents a composite animation
    /// (such as <see cref="Windows.UI.Xaml.Media.Animation.Storyboard"/>) that can be executed on a given element.
    /// </summary>
    [ContentProperty(Name = nameof(Animations))]
    public sealed class AnimationCollection2 : DependencyObject
    {
        /// <summary>
        /// Raised whenever the current animation is started.
        /// </summary>
        public event EventHandler? Started;

        /// <summary>
        /// Raised whenever the current animation ends.
        /// </summary>
        public event EventHandler? Ended;

        /// <summary>
        /// Gets or sets the list of animations in the current collection.
        /// </summary>
        public IList<ITimeline> Animations { get; set; } = new List<ITimeline>();

        /// <summary>
        /// Gets or sets the weak reference to the parent that owns the current animation collection.
        /// </summary>
        internal WeakReference<UIElement>? ParentReference { get; set; }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public async void Start()
        {
            // Here we're using an async void method on purpose, in order to be able to await
            // the completion of the animation and rethrow exceptions. We can't just use the
            // synchronous AnimationBuilder.Start method here, as we also need to await for the
            // animation to complete in either case in order to raise the Ended event when that
            // happens. So we add an async state machine here to work around this.
            await StartAsync();
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public Task StartAsync()
        {
            UIElement? parent = null;

            if (ParentReference?.TryGetTarget(out parent) != true)
            {
                ThrowHelper.ThrowInvalidOperationException("The current animation collection isn't bound to a parent UIElement instance.");
            }

            return StartAsync(parent!);
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public async void Start(UIElement element)
        {
            await StartAsync(element);
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public async Task StartAsync(UIElement element)
        {
            Started?.Invoke(this, EventArgs.Empty);

            var builder = AnimationBuilder.Create();

            foreach (ITimeline animation in Animations)
            {
                builder = animation.AppendToBuilder(builder);
            }

            await builder.StartAsync(element);

            Ended?.Invoke(this, EventArgs.Empty);
        }
    }
}
