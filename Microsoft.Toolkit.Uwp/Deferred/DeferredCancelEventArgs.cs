// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Deferred
{
    /// <summary>
    /// <see cref="DeferredEventArgs"/> which can also be Cancelled.
    /// </summary>
    public class DeferredCancelEventArgs : DeferredEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the event should be cancelled.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
