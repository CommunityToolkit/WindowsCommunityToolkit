// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A base model representing a typed animation that can be used in XAML.
    /// </summary>
    /// <typeparam name="T">The type of values for the animation.</typeparam>
    public abstract class TypedAnimation<T> : Animation
    {
        /// <summary>
        /// Gets or sets the final value for the animation.
        /// </summary>
        public T? To { get; set; }

        /// <summary>
        /// Gets or sets the optional starting value for the animation.
        /// </summary>
        public T? From { get; set; }
    }
}
