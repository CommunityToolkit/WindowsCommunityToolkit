using System;
using System.Runtime.CompilerServices;

#nullable enable

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to throw exceptions
    /// </summary>
    internal static partial class ThrowHelper
    {
        /// <summary>
        /// Throws a new <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentException">Thrown with <paramref name="message"/> and <paramref name="name"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowArgumentException(string name, string message)
        {
            throw new ArgumentException(message, name);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentNullException">Thrown with <paramref name="name"/> and <paramref name="message"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowArgumentNullException(string name, string message)
        {
            throw new ArgumentNullException(name, message);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown with <paramref name="name"/> and <paramref name="message"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowArgumentOutOfRangeException(string name, string message)
        {
            throw new ArgumentOutOfRangeException(name, message);
        }
    }
}
