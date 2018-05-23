// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer
{
    /// <summary>
    /// The struct provides guard related info.
    /// </summary>
    public struct HowToGuard
    {
        /// <summary>
        /// Type being checked
        /// </summary>
        public string TypeToCheck;

        /// <summary>
        /// Member being checked
        /// </summary>
        public string MemberToCheck;

        /// <summary>
        /// Whether parameter count will be used for the check
        /// </summary>
        public int? ParameterCountToCheck;

        /// <summary>
        /// Type of check
        /// </summary>
        public string KindOfCheck;
    }
}
