// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A collection of animations that can be grouped together.
    /// </summary>
    public sealed class AnimationCollection2 : ObservableCollection<Animation>, ITimeline
    {
        /// <summary>
        /// The reference to the parent that owns the current animation collection.
        /// </summary>
        private WeakReference<UIElement>? parent;

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
            set => parent = new(value!);
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public void Start()
        {
            ((ITimeline)this).AppendToBuilder(new AnimationBuilder()).Start(Parent!);
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public Task StartAsync()
        {
            return ((ITimeline)this).AppendToBuilder(new AnimationBuilder()).StartAsync(Parent!);
        }

        /// <inheritdoc/>
        AnimationBuilder ITimeline.AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint)
        {
            foreach (ITimeline element in this)
            {
                builder = element.AppendToBuilder(builder, delayHint, durationHint);
            }

            return builder;
        }
    }
}
