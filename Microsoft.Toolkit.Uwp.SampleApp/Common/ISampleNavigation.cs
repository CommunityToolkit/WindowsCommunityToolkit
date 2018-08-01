// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    /// <summary>
    /// Sample LifeCycle Events.
    /// </summary>
    public interface ISampleNavigation
    {
        /// <summary>
        /// Callback to Clean up Resources while Navigating away.
        /// </summary>
        void NavigatingAway();
    }
}