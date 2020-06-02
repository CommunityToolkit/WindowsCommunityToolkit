// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETSTANDARD2_1

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Applied to a method that will never return under any circumstance.
    /// </summary>
    /// <remarks>Internal copy of the .NET Standard 2.1 attribute.</remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class DoesNotReturnAttribute : Attribute
    {
    }
}

#endif