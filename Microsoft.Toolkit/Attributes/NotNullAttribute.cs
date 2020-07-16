// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETSTANDARD2_1

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Specifies that an output will not be <see langword="null"/> even if the corresponding type allows it.
    /// Specifies that an input argument was not <see langword="null"/> when the call returns.
    /// </summary>
    /// <remarks>Internal copy of the .NET Standard 2.1 attribute.</remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue)]
    internal sealed class NotNullAttribute : Attribute
    {
    }
}

#endif