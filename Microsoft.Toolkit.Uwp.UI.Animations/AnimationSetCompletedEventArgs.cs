// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// AnimationSet Completed EventArgs.
    /// </summary>
    public class AnimationSetCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a value indicating whether the animation completed
        /// </summary>
        public bool Completed { get; internal set; }
    }
}