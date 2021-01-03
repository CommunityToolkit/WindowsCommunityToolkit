// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.Toolkit.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A collection of animations that can be grouped together. This type represents a composite animation
    /// (such as <see cref="Windows.UI.Xaml.Media.Animation.Storyboard"/>) that can be executed on a given element.
    /// </summary>
    [ContentProperty(Name = nameof(Nodes))]
    public sealed class AnimationSet : DependencyObject
    {
        /// <summary>
        /// An interface representing a node in an <see cref="AnimationSet"/> instance.
        /// </summary>
        public interface INode
        {
        }

        /// <summary>
        /// Raised whenever the current animation is started.
        /// </summary>
        public event EventHandler? Started;

        /// <summary>
        /// Raised whenever the current animation ends.
        /// </summary>
        public event EventHandler? Ended;

        /// <summary>
        /// Gets or sets the list of nodes in the current collection.
        /// </summary>
        public IList<INode> Nodes { get; set; } = new List<INode>();

        /// <summary>
        /// Gets or sets a value indicating whether top level animation nodes in this collection are invoked
        /// sequentially. This applies to both <see cref="AnimationScope"/> nodes (which will still trigger
        /// contained animations at the same time), and other top level animation nodes. The default value
        /// is <see langword="false"/>, which means that all contained animations will start at the same time.
        /// <para>
        /// Note that this property will also cause a change in behavior for the animation. With the default
        /// configuration, with all animations starting at the same time, it's not possible to use multiple
        /// animations targeting the same property (as they'll cause a conflict and be ignored when on the
        /// composition layer, or cause a crash when on the XAML layer). When animations are started sequentially
        /// instead, each sequential block will be able to share target properties with animations from other
        /// sequential blocks, without issues. Note that especially for simple scenarios (eg. an opacity animation
        /// that just transitions to a state and then back, or between two states), it is recommended to use a single
        /// keyframe animation instead, which will result in less overhead when creating and starting the animation.
        /// </para>
        /// </summary>
        public bool IsSequential { get; set; }

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
            return StartAsync(GetParent());
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

            if (IsSequential)
            {
                foreach (INode node in Nodes)
                {
                    if (node is ITimeline timeline)
                    {
                        var builder = AnimationBuilder.Create();

                        timeline.AppendToBuilder(builder);

                        await builder.StartAsync(element);
                    }
                    else if (node is IActivity activity)
                    {
                        await activity.InvokeAsync(element);
                    }
                }
            }
            else
            {
                var builder = AnimationBuilder.Create();

                foreach (INode node in Nodes)
                {
                    switch (node)
                    {
                        case ITimeline timeline:
                            builder = timeline.AppendToBuilder(builder);
                            break;
                        case IActivity activity:
                            _ = activity.InvokeAsync(element);
                            break;
                    }
                }

                await builder.StartAsync(element);
            }

            Ended?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the current parent <see cref="UIElement"/> instance.
        /// </summary>
        /// <returns>The <see cref="UIElement"/> reference from <see cref="ParentReference"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if there is no parent available.</exception>
        [Pure]
        private UIElement GetParent()
        {
            UIElement? parent = null;

            if (ParentReference?.TryGetTarget(out parent) != true)
            {
                ThrowHelper.ThrowInvalidOperationException("The current animation collection isn't bound to a parent UIElement instance.");
            }

            return parent!;
        }
    }
}
