// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop
{
    /// <summary>
    /// Identifies Windows 10 release IDs
    /// </summary>
    internal enum Windows10Release
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 10.0.10240.0
        /// </summary>
        Threshold1 = 1507,

        /// <summary>
        /// 10.0.10586
        /// </summary>
        Threshold2 = 1511,

        /// <summary>
        /// 10.0.14393.0 (Redstone 1)
        /// </summary>
        Anniversary = 1607,

        /// <summary>
        /// 10.0.15063.0 (Redstone 2)
        /// </summary>
        Creators = 1703,

        /// <summary>
        /// 10.0.16299.0 (Redstone 3)
        /// </summary>
        FallCreators = 1709,

        /// <summary>
        /// 10.0.17134.0 (Redstone 4)
        /// </summary>
        April2018 = 1803,
    }
}