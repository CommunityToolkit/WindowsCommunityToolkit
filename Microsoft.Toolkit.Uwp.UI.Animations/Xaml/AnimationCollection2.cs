// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A collection of animations that can be grouped together. This type represents a composite animation
    /// (such as <see cref="Windows.UI.Xaml.Media.Animation.Storyboard"/>) that can be executed on a given element.
    /// </summary>
    [ContentProperty(Name = nameof(Animations))]
    public sealed class AnimationCollection2 : DependencyObject, ITimeline
    {
        /// <summary>
        /// The reference to the parent that owns the current animation collection.
        /// </summary>
        private WeakReference<UIElement>? parent;

        /// <summary>
        /// Raised whenever the current animation is started.
        /// </summary>
        public event EventHandler? Started;

        /// <summary>
        /// Raised whenever the current animation ends.
        /// </summary>
        public event EventHandler? Ended;

        /// <summary>
        /// Raised whenever the current collection changes.
        /// </summary>
        public event EventHandler? CollectionChanged;

        /// <summary>
        /// Gets or sets the list of animations in the current collection.
        /// </summary>
        public IList<Animation> Animations { get; set; } = new List<Animation>();

        /// <summary>
        /// Gets or sets the parent <see cref="UIElement"/> for the current animation collection.
        /// </summary>
        internal UIElement? Parent
        {
            get
            {
                UIElement? element = null;

                _ = this.parent?.TryGetTarget(out element);

                return element;
            }
            set => this.parent = new(value!);
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public void Start()
        {
            _ = StartAsync();
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public Task StartAsync()
        {
            return StartAsync(Parent!);
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public void Start(UIElement element)
        {
            _ = StartAsync(element);
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public async Task StartAsync(UIElement element)
        {
            Started?.Invoke(this, EventArgs.Empty);

            await ((ITimeline)this).AppendToBuilder(new AnimationBuilder()).StartAsync(element);

            Ended?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        AnimationBuilder ITimeline.AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint)
        {
            foreach (ITimeline element in Animations)
            {
                builder = element.AppendToBuilder(builder, delayHint, durationHint);
            }

            return builder;
        }
    }
}
