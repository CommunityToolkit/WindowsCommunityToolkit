// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System.Collections.Generic;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A base model representing a typed animation that can be used in XAML.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type to use for the public <see cref="To"/> and <see cref="From"/> properties.
    /// This can differ from <typeparamref name="TKeyFrame"/> to facilitate XAML parsing.
    /// </typeparam>
    /// <typeparam name="TKeyFrame">The actual type of keyframe values in use.</typeparam>
    [ContentProperty(Name = nameof(KeyFrames))]
    public abstract class Animation<TValue, TKeyFrame> : Animation
        where TKeyFrame : unmanaged
    {
        /// <summary>
        /// Gets or sets the final value for the animation.
        /// </summary>
        public TValue? To { get; set; }

        /// <summary>
        /// Gets or sets the optional starting value for the animation.
        /// </summary>
        public TValue? From { get; set; }

        /// <summary>
        /// Gets or sets the optional keyframe collection for the current animation.
        /// Setting this will overwrite the <see cref="To"/> and <see cref="From"/> values.
        /// </summary>
        public IList<IKeyFrame<TKeyFrame>> KeyFrames { get; set; } = new List<IKeyFrame<TKeyFrame>>();
    }
}
