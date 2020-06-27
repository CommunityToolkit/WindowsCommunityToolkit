// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System
{
    /// <summary>
    /// This type is meant to be used to represent "ref T" fields. It is working around lack of first class
    /// support for byref fields in C# and IL. The JIT and type loader has special handling for it that turns
    /// it into a thin wrapper around ref T. This type is ported from CoreCLR, see original source
    /// <see href="https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/ByReference.cs">here</see>.
    /// </summary>
    /// <typeparam name="T">The type of reference being stored.</typeparam>
    public readonly ref struct ByReference<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ByReference{T}"/> struct.
        /// </summary>
        /// <param name="value">The reference to the target <typeparamref name="T"/> value.</param>
        public ByReference(ref T value)
        {
            // Implemented as a JIT intrinsic - This default implementation is for
            // completeness and to provide a concrete error if called via reflection
            // or if intrinsic is missed.
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Gets the <typeparamref name="T"/> reference represented by the current <see cref="ByReference{T}"/> instance.
        /// </summary>
        public ref T Value
        {
            // Implemented as a JIT intrinsic - This default implementation is for
            // completeness and to provide a concrete error if called via reflection
            // or if the intrinsic is missed.
            get => throw new PlatformNotSupportedException();
        }
    }
}
