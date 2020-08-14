// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Provides an extension which allows lighting.
    /// </summary>
    public static partial class AnimationExtensions
    {
        /// <summary>
        /// Gets a value indicating whether this instance is lighting supported. Deprecated. Will always return <c>true</c>.
        /// </summary>
        /// <value>
        /// Deprecated. Will always returns <c>true</c>.
        /// </value>
        [Obsolete("This method is deprecated and will always return true")]
        public static bool IsLightingSupported => true;
    }
}
