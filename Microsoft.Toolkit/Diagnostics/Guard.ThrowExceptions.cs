// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;

#nullable enable

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to verify conditions when running code.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Throws a new <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentException">Thrown with <paramref name="message"/> and <paramref name="name"/>.</exception>
        /// <remarks>This method is marked as <see cref="MethodImplOptions.NoInlining"/> to reduce the binary size of the caller.</remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
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
        /// <remarks>This method is marked as <see cref="MethodImplOptions.NoInlining"/> to reduce the binary size of the caller.</remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
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
        /// <remarks>This method is marked as <see cref="MethodImplOptions.NoInlining"/> to reduce the binary size of the caller.</remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentOutOfRangeException(string name, string message)
        {
            throw new ArgumentOutOfRangeException(name, message);
        }
    }
}
