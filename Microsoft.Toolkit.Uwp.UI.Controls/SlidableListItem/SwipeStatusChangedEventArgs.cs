// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Event args for a SwipeStatus changing event
    /// </summary>
    [Obsolete("The SwipeStatusChangedEventArgs will be removed alongside SlidableListItem in a future major release. Please use the SwipeControl available in the Fall Creators Update")]
    public class SwipeStatusChangedEventArgs
    {
        /// <summary>
        /// Gets the old value.
        /// </summary>
        public SwipeStatus OldValue { get; internal set; }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        public SwipeStatus NewValue { get; internal set; }
    }
}
