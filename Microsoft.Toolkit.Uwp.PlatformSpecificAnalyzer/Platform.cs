// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer
{
    /// <summary>
    /// Simple struct to hold platform / version / param count info for given symbol
    /// </summary>
    public struct Platform
    {
        /// <summary>
        /// Platform Kind
        /// </summary>
        public PlatformKind Kind;

        /// <summary>
        /// For UWP, this is version 15063 or 16299etc. For User, the fully qualified name of the attribute in use
        /// </summary>
        public string Version;

        /// <summary>
        /// For UWP only
        /// </summary>
        public bool ByParameterCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="Platform"/> struct.
        /// </summary>
        /// <param name="kind"><see cref="PlatformKind"/></param>
        /// <param name="version">version</param>
        /// <param name="byParameterCount">boolean</param>
        public Platform(PlatformKind kind, string version = null, bool byParameterCount = false)
        {
            Kind = kind;
            Version = version;
            ByParameterCount = byParameterCount;

            switch (kind)
            {
                case PlatformKind.Unchecked:
                    if (version != null)
                    {
                        throw new ArgumentException("No version expected");
                    }

                    break;

                case PlatformKind.Uwp:
                    break;

                case PlatformKind.ExtensionSDK:
                    if (version != null)
                    {
                        throw new ArgumentException("Don't specify versions for extension SDKs");
                    }

                    break;
            }

            if (byParameterCount && kind != PlatformKind.Uwp)
            {
                throw new ArgumentException("Only UWP can be distinguished by parameter count");
            }
        }
    }
}
