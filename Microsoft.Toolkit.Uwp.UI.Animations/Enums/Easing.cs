// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Indicates an easing function for an animation.
    /// </summary>
    public enum Easing
    {
        /// <summary>
        /// The linear easing, with [0, 0] and [1, 1] as control points.
        /// </summary>
        Linear,

        /// <summary>
        /// The sine in easing, with [0.4, 0] and [1, 1] as control points.
        /// </summary>
        SineEaseIn,

        /// <summary>
        /// The sine out easing, with [0, 0] and [0.6, 1] as control points.
        /// </summary>
        SineEaseOut,

        /// <summary>
        /// The sine in out easing, with [0.4, 0] and [0.6, 1] as control points.
        /// </summary>
        SineEaseInOut,

        /// <summary>
        /// The quadratic in easing, with [0.8, 0] and [1, 1] as control points.
        /// </summary>
        QuadraticEaseIn,

        /// <summary>
        /// The quadratic out easing, with [0, 0] and [0.2, 1] as control points.
        /// </summary>
        QuadraticEaseOut,

        /// <summary>
        /// The quadratic in out easing, with [0.8, 0] and [0.2, 1] as control points.
        /// </summary>
        QuadraticEaseInOut,

        /// <summary>
        /// The circle in easing, with [1, 0] and [1, 0.8] as control points.
        /// </summary>
        CircleEaseIn,

        /// <summary>
        /// The circle out easing, with [0, 0.3] and [0, 1] as control points.
        /// </summary>
        CircleEaseOut,

        /// <summary>
        /// The circle in out easing, with [0.9, 0] and [0.1, 1] as control points.
        /// </summary>
        CircleEaseInOut
    }
}
