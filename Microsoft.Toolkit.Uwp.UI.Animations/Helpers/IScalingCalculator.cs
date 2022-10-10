// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using Windows.Foundation;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Helpers
{
    /// <summary>
    /// Defines methods to support calculating scaling changes.
    /// </summary>
    public interface IScalingCalculator
    {
        /// <summary>
        /// Handler used to calculate the change in the scaling of an element when it is in transition.
        /// </summary>
        /// <param name="source">The source element.</param>
        /// <param name="target">The target element.</param>
        /// <returns>A <see cref="Point"/> whose X value represents the horizontal scaling change and whose Y represents the vertical scaling change.</returns>
        Point GetScaling(UIElement source, UIElement target);
    }
}
