// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// EasingType is used to describe how the animation interpolates between keyframes.
    /// </summary>
    public enum EasingType
    {
        /// <summary>
        /// Creates an animation that accelerates with the default EasingType which is specified in AnimationExtensions.DefaultEasingType which is by default Cubic.
        /// </summary>
        Default,

        /// <summary>
        /// Creates an animation that accelerates or decelerates linearly.
        /// </summary>
        Linear,

        /// <summary>
        /// Creates an animation that accelerates or decelerates using the formula f(t) = t3.
        /// </summary>
        Cubic,

        /// <summary>
        /// Retracts the motion of an animation slightly before it begins to animate in the path indicated.
        /// </summary>
        Back,

        /// <summary>
        /// Creates a bouncing effect.
        /// </summary>
        Bounce,

        /// <summary>
        /// Creates an animation that resembles a spring oscillating back and forth until it comes to rest.
        /// </summary>
        Elastic,

        /// <summary>
        ///  Creates an animation that accelerates or decelerates using a circular function.
        /// </summary>
        Circle,

        /// <summary>
        /// Creates an animation that accelerates or decelerates using the formula f(t) = t2.
        /// </summary>
        Quadratic,

        /// <summary>
        /// Creates an animation that accelerates or decelerates using the formula f(t) = t4.
        /// </summary>
        Quartic,

        /// <summary>
        /// Create an animation that accelerates or decelerates using the formula f(t) = t5.
        /// </summary>
        Quintic,

        /// <summary>
        ///  Creates an animation that accelerates or decelerates using a sine formula.
        /// </summary>
        Sine
    }
}
