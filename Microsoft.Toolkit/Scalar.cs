﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit
{
    /// <summary>
    /// Class containing some constants
    /// represented as Floating point numbers.
    /// </summary>
    public static class Scalar
    {
        // Pi related floating point constants

        /// <summary>
        /// (float)Math.PI radians ( or 180 degrees).
        /// </summary>
        public const float Pi = (float)Math.PI;

        /// <summary>
        /// Two times (float)Math.PI radians ( or 360 degrees).
        /// </summary>
        public const float TwoPi = 2f * Pi;

        /// <summary>
        /// Half of (float)Math.PI radians ( or 90 degrees).
        /// </summary>
        public const float PiByTwo = Pi / 2f;

        /// <summary>
        /// One third of (float)Math.PI radians ( or 60 degrees).
        /// </summary>
        public const float PiByThree = Pi / 3f;

        /// <summary>
        /// One fourth of (float)Math.PI radians ( or 45 degrees).
        /// </summary>
        public const float PiByFour = Pi / 4f;

        /// <summary>
        /// One sixth of (float)Math.PI radians ( or 30 degrees).
        /// </summary>
        public const float PiBySix = Pi / 6f;

        /// <summary>
        /// Three times half of (float)Math.PI radians ( or 270 degrees).
        /// </summary>
        public const float ThreePiByTwo = 3f * Pi / 2f;

        // Conversion constants

        /// <summary>
        /// 1 degree in radians.
        /// </summary>
        public const float DegreesToRadians = Pi / 180f;

        /// <summary>
        /// 1 radian in degrees.
        /// </summary>
        public const float RadiansToDegrees = 180f / Pi;
    }
}
