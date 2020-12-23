// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A base model representing a typed keyframe that can be used in XAML.
    /// </summary>
    /// <typeparam name="T">The type of values for the keyframe.</typeparam>
    public abstract class KeyFrame<T>
    {
        /// <summary>
        /// Gets or sets the key time for the current keyframe. This is a normalized
        /// value in the [0, 1] range, relative to the total animation duration.
        /// </summary>
        public double Key { get; set; }

        /// <summary>
        /// Gets or sets the animation value for the current keyframe.
        /// </summary>
        public T? Value { get; set; }
    }
}
