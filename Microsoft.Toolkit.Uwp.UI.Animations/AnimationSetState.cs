// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// States of AnimationSet.
    /// </summary>
    public enum AnimationSetState
    {
        /// <summary>
        /// The animation has not been started
        /// </summary>
        NotStarted,

        /// <summary>
        /// The animation has been started and is in progress
        /// </summary>
        Running,

        /// <summary>
        /// The animation has been started and is stopped
        /// </summary>
        Stopped,

        /// <summary>
        /// The animation had completed
        /// </summary>
        Completed
    }
}
