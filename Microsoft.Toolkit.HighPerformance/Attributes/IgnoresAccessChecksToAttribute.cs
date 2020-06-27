// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETCORE_RUNTIME

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// A special attribute recognized by CoreCLR that allows to ignore visibility checks for internal APIs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    internal sealed class IgnoresAccessChecksToAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoresAccessChecksToAttribute"/> class.
        /// </summary>
        /// <param name="assemblyName">The assembly name to use.</param>
        public IgnoresAccessChecksToAttribute(string assemblyName)
        {
        }
    }
}

#endif
