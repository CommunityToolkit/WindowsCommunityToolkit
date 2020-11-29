// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Toolkit.Extensions;

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to efficiently throw exceptions.
    /// </summary>
    public static partial class ThrowHelper
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.CanRead"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForCanRead(Stream stream, string name)
        {
            throw new ArgumentException($"Stream {name.ToAssertString()} ({stream.GetType().ToTypeString()}) doesn't support reading", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.CanWrite"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForCanWrite(Stream stream, string name)
        {
            throw new ArgumentException($"Stream {name.ToAssertString()} ({stream.GetType().ToTypeString()}) doesn't support writing", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.CanSeek"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForCanSeek(Stream stream, string name)
        {
            throw new ArgumentException($"Stream {name.ToAssertString()} ({stream.GetType().ToTypeString()}) doesn't support seeking", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsAtStartPosition"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsAtStartPosition(Stream stream, string name)
        {
            throw new ArgumentException($"Stream {name.ToAssertString()} ({stream.GetType().ToTypeString()}) must be at position {0.ToAssertString()}, was at {stream.Position.ToAssertString()}", name);
        }
    }
}
