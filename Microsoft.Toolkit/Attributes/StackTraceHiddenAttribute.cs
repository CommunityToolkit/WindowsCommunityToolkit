// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Diagnostics
{
    /// <summary>
    /// Attribute to add to non-returning throw only methods, 
    /// to restore the stack trace back to what it would be if the throw was in-place
    /// </summary>
    /// <remarks>Internal copy of the CoreCLR attribute.</remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Struct, Inherited = false)]
    internal sealed class StackTraceHiddenAttribute : Attribute
    {
    }
}