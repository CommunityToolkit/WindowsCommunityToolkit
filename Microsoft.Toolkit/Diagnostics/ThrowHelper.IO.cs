using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

#nullable enable

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to throw exceptions
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1618", Justification = "Internal helper methods")]
    internal static partial class ThrowHelper
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.CanRead"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForCanRead(string name)
        {
            ThrowArgumentException(name, $"Stream {name} doesn't support reading");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.CanWrite"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForCanWrite(string name)
        {
            ThrowArgumentException(name, $"Stream {name} doesn't support writing");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.CanSeek"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForCanSeek(string name)
        {
            ThrowArgumentException(name, $"Stream {name} doesn't support seeking");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsAtStartPosition"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsAtStartPosition(Stream stream, string name)
        {
            ThrowArgumentException(name, $"Stream {name} must be at start position, was at {stream.Position}");
        }
    }
}
