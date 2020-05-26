// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Extensions;

#nullable enable

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to throw exceptions
    /// </summary>
    internal static partial class ThrowHelper
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.CanRead"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForCanRead(Stream stream, string name)
        {
            ThrowArgumentException(name, $"Stream {name.ToAssertString()} ({stream.GetType().ToTypeString()}) doesn't support reading");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.CanWrite"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForCanWrite(Stream stream, string name)
        {
            ThrowArgumentException(name, $"Stream {name.ToAssertString()} ({stream.GetType().ToTypeString()}) doesn't support writing");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.CanSeek"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForCanSeek(Stream stream, string name)
        {
            ThrowArgumentException(name, $"Stream {name.ToAssertString()} ({stream.GetType().ToTypeString()}) doesn't support seeking");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsAtStartPosition"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsAtStartPosition(Stream stream, string name)
        {
            ThrowArgumentException(name, $"Stream {name.ToAssertString()} ({stream.GetType().ToTypeString()}) must be at position {0.ToAssertString()}, was at {stream.Position.ToAssertString()}");
        }
    }
}
