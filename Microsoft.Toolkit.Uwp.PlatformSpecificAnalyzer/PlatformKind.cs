// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer
{
    /// <summary>
    /// Platform kind enum
    /// </summary>
    public enum PlatformKind
    {
        /// <summary>
        /// .NET and Pre-UWP WinRT
        /// </summary>
        Unchecked,

        /// <summary>
        /// Core UWP platform
        /// </summary>
        Uwp,

        /// <summary>
        /// Desktop, Mobile, IOT, Xbox extension SDK
        /// </summary>
        ExtensionSDK
    }
}
