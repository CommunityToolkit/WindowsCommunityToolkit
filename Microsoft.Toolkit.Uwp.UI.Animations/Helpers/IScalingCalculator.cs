// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System.Numerics;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Helpers
{
    /// <summary>
    /// Defines methods to support calculating scaling changes.
    /// </summary>
    public interface IScalingCalculator
    {
        /// <summary>
        /// Gets the scaling changes when the source element transitions to the target element.
        /// </summary>
        /// <param name="source">The source element.</param>
        /// <param name="target">The target element.</param>
        /// <returns>A <see cref="Vector2"/> whose X value represents the horizontal scaling change and whose Y represents the vertical scaling change.</returns>
        Vector2 GetScaling(UIElement source, UIElement target);
    }
}
