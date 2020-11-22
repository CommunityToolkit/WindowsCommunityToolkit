// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETSTANDARD2_1_OR_GREATER

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Specifies that when a method returns <see cref="ReturnValue"/>, the parameter may be null even if the corresponding type disallows it.
    /// </summary>
    /// <remarks>Internal copy from the BCL attribute.</remarks>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class MaybeNullWhenAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaybeNullWhenAttribute"/> class.
        /// </summary>
        /// <param name="returnValue">The return value condition. If the method returns this value, the associated parameter may be null.</param>
        public MaybeNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;

        /// <summary>
        /// Gets a value indicating whether the value may be <see langword="null"/> depending on the return value.
        /// </summary>
        public bool ReturnValue { get; }
    }
}

#endif
