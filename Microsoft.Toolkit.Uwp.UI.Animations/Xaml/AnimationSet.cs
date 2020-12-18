// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A set of animations that can be grouped together.
    /// </summary>
    [ContentProperty(Name = nameof(Elements))]
    public class AnimationSet : ITimeline
    {
        /// <summary>
        /// Gets or sets the collection of <see cref="ITimeline"/> items for the current set.
        /// </summary>
        public IList<ITimeline> Elements { get; set; } = new List<ITimeline>();

        /// <inheritdoc/>
        AnimationBuilder ITimeline.AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint)
        {
            foreach (ITimeline element in Elements)
            {
                builder = element.AppendToBuilder(builder, delayHint, durationHint);
            }

            return builder;
        }
    }
}
