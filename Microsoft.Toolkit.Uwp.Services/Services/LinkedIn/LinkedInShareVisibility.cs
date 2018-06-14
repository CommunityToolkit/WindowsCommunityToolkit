// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.Services.LinkedIn
{
    /// <summary>
    /// List of user related data permissions
    /// </summary>
    [Flags]
    public enum LinkedInShareVisibility
    {
        /// <summary>
        /// Connections only
        /// </summary>
        ConnectionsOnly = 1,

        /// <summary>
        /// Anyone
        /// </summary>
        Anyone = 2
    }
}
