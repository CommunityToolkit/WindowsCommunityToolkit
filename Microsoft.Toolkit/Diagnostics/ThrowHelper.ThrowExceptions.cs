// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

#nullable enable

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to throw exceptions
    /// </summary>
    public static partial class ThrowHelper
    {
        /// <summary>
        /// Returns a formatted representation of the input value.
        /// </summary>
        /// <param name="obj">The input <see cref="object"/> to format.</param>
        /// <returns>A formatted representation of <paramref name="obj"/> to display in error messages.</returns>
        [Pure]
        private static string ToAssertString(this object? obj)
        {
            return obj switch
            {
                string _ => $"\"{obj}\"",
                null => "null",
                _ => $"<{obj}>"
            };
        }

        /// <summary>
        /// Throws a new <see cref="AccessViolationException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="AccessViolationException">Thrown with <paramref name="message"/>.</exception>
        [DoesNotReturn]
        public static void ThrowAccessViolationException(string message)
        {
            throw new AccessViolationException(message);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentException">Thrown with <paramref name="message"/>.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentException(string message)
        {
            throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentException">Thrown with <paramref name="message"/> and <paramref name="name"/>.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentException(string name, string message)
        {
            throw new ArgumentException(message, name);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <exception cref="ArgumentNullException">Thrown with <paramref name="name"/>.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string name)
        {
            throw new ArgumentNullException(name);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentNullException">Thrown with <paramref name="name"/> and <paramref name="message"/>.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string name, string message)
        {
            throw new ArgumentNullException(name, message);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown with <paramref name="name"/>.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string name)
        {
            throw new ArgumentOutOfRangeException(name);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown with <paramref name="name"/> and <paramref name="message"/>.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string name, string message)
        {
            throw new ArgumentOutOfRangeException(name, message);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="value">The current argument value.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown with <paramref name="name"/>, <paramref name="value"/> and <paramref name="message"/>.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string name, object value, string message)
        {
            throw new ArgumentOutOfRangeException(name, value, message);
        }
    }
}
