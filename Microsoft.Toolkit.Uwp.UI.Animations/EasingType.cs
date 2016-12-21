// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
        /// Creates an animation that accelerates or decelerates linear.
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
