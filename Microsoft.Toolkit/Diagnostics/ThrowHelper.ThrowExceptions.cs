// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Threading;

#nullable enable

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to throw exceptions
    /// </summary>
    public static partial class ThrowHelper
    {
        /// <summary>
        /// Throws a new <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentException(string message)
        {
            throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="ArgumentException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentException(string message, Exception innerException)
        {
            throw new ArgumentException(message, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentException(string name, string message)
        {
            throw new ArgumentException(message, name);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="ArgumentException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentException(string name, string message, Exception innerException)
        {
            throw new ArgumentException(message, name, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <exception cref="ArgumentNullException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string name)
        {
            throw new ArgumentNullException(name);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="ArgumentNullException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string name, Exception innerException)
        {
            throw new ArgumentNullException(name, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentNullException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string name, string message)
        {
            throw new ArgumentNullException(name, message);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string name)
        {
            throw new ArgumentOutOfRangeException(name);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string name, Exception innerException)
        {
            throw new ArgumentOutOfRangeException(name, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown with the specified parameters.</exception>
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
        /// <exception cref="ArgumentOutOfRangeException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string name, object value, string message)
        {
            throw new ArgumentOutOfRangeException(name, value, message);
        }

        /// <summary>
        /// Throws a new <see cref="FormatException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="FormatException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowFormatException(string message)
        {
            throw new FormatException(message);
        }

        /// <summary>
        /// Throws a new <see cref="FormatException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="FormatException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowFormatException(string message, Exception innerException)
        {
            throw new FormatException(message, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="InsufficientMemoryException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="InsufficientMemoryException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowInsufficientMemoryException(string message)
        {
            throw new InsufficientMemoryException(message);
        }

        /// <summary>
        /// Throws a new <see cref="InsufficientMemoryException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="InsufficientMemoryException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowInsufficientMemoryException(string message, Exception innerException)
        {
            throw new InsufficientMemoryException(message, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="InvalidOperationException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowInvalidOperationException(string message)
        {
            throw new InvalidOperationException(message);
        }

        /// <summary>
        /// Throws a new <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="InvalidOperationException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowInvalidOperationException(string message, Exception innerException)
        {
            throw new InvalidOperationException(message, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="MissingFieldException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="MissingFieldException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowMissingFieldException(string message)
        {
            throw new MissingFieldException(message);
        }

        /// <summary>
        /// Throws a new <see cref="MissingFieldException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="MissingFieldException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowMissingFieldException(string message, Exception innerException)
        {
            throw new MissingFieldException(message, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="MissingFieldException"/>.
        /// </summary>
        /// <param name="className">The target class being inspected.</param>
        /// <param name="fieldName">The target field being retrieved.</param>
        /// <exception cref="MissingFieldException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowMissingFieldException(string className, string fieldName)
        {
            throw new MissingFieldException(className, fieldName);
        }

        /// <summary>
        /// Throws a new <see cref="MissingMemberException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="MissingMemberException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowMissingMemberException(string message)
        {
            throw new MissingMemberException(message);
        }

        /// <summary>
        /// Throws a new <see cref="MissingMemberException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="MissingMemberException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowMissingMemberException(string message, Exception innerException)
        {
            throw new MissingMemberException(message, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="MissingMemberException"/>.
        /// </summary>
        /// <param name="className">The target class being inspected.</param>
        /// <param name="memberName">The target member being retrieved.</param>
        /// <exception cref="MissingMemberException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowMissingMemberException(string className, string memberName)
        {
            throw new MissingMemberException(className, memberName);
        }

        /// <summary>
        /// Throws a new <see cref="MissingMethodException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="MissingMethodException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowMissingMethodException(string message)
        {
            throw new MissingMethodException(message);
        }

        /// <summary>
        /// Throws a new <see cref="MissingMethodException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="MissingMethodException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowMissingMethodException(string message, Exception innerException)
        {
            throw new MissingMethodException(message, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="MissingMethodException"/>.
        /// </summary>
        /// <param name="className">The target class being inspected.</param>
        /// <param name="methodName">The target method being retrieved.</param>
        /// <exception cref="MissingMethodException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowMissingMethodException(string className, string methodName)
        {
            throw new MissingMethodException(className, methodName);
        }

        /// <summary>
        /// Throws a new <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="NotSupportedException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowNotSupportedException(string message)
        {
            throw new NotSupportedException(message);
        }

        /// <summary>
        /// Throws a new <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="NotSupportedException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowNotSupportedException(string message, Exception innerException)
        {
            throw new NotSupportedException(message, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/>.
        /// </summary>
        /// <param name="objectName">The name of the disposed object.</param>
        /// <exception cref="ObjectDisposedException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowObjectDisposedException(string objectName)
        {
            throw new ObjectDisposedException(objectName);
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/>.
        /// </summary>
        /// <param name="objectName">The name of the disposed object.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="ObjectDisposedException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowObjectDisposedException(string objectName, Exception innerException)
        {
            throw new ObjectDisposedException(objectName, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/>.
        /// </summary>
        /// <param name="objectName">The name of the disposed object.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ObjectDisposedException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowObjectDisposedException(string objectName, string message)
        {
            throw new ObjectDisposedException(objectName, message);
        }

        /// <summary>
        /// Throws a new <see cref="OperationCanceledException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="OperationCanceledException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowOperationCanceledException(string message)
        {
            throw new OperationCanceledException(message);
        }

        /// <summary>
        /// Throws a new <see cref="OperationCanceledException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="OperationCanceledException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowOperationCanceledException(string message, Exception innerException)
        {
            throw new OperationCanceledException(message, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="OperationCanceledException"/>.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> in use.</param>
        /// <exception cref="OperationCanceledException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowOperationCanceledException(CancellationToken token)
        {
            throw new OperationCanceledException(token);
        }

        /// <summary>
        /// Throws a new <see cref="OperationCanceledException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="token">The <see cref="CancellationToken"/> in use.</param>
        /// <exception cref="OperationCanceledException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowThrowOperationCanceledException(string message, CancellationToken token)
        {
            throw new OperationCanceledException(message, token);
        }

        /// <summary>
        /// Throws a new <see cref="OperationCanceledException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <param name="token">The <see cref="CancellationToken"/> in use.</param>
        /// <exception cref="OperationCanceledException">Thrown with the specified parameters.</exception>
        [DoesNotReturn]
        public static void ThrowThrowOperationCanceledException(string message, Exception innerException, CancellationToken token)
        {
            throw new OperationCanceledException(message, innerException, token);
        }

        /// <summary>
        /// Throws a new <see cref="PlatformNotSupportedException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="PlatformNotSupportedException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowPlatformNotSupportedException(string message)
        {
            throw new PlatformNotSupportedException(message);
        }

        /// <summary>
        /// Throws a new <see cref="PlatformNotSupportedException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="PlatformNotSupportedException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowPlatformNotSupportedException(string message, Exception innerException)
        {
            throw new PlatformNotSupportedException(message, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="TimeoutException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="TimeoutException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowTimeoutException(string message)
        {
            throw new TimeoutException(message);
        }

        /// <summary>
        /// Throws a new <see cref="TimeoutException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="TimeoutException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowTimeoutException(string message, Exception innerException)
        {
            throw new TimeoutException(message, innerException);
        }

        /// <summary>
        /// Throws a new <see cref="UnauthorizedAccessException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="UnauthorizedAccessException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowUnauthorizedAccessException(string message)
        {
            throw new UnauthorizedAccessException(message);
        }

        /// <summary>
        /// Throws a new <see cref="UnauthorizedAccessException"/>.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner <see cref="Exception"/> to include.</param>
        /// <exception cref="UnauthorizedAccessException">Thrown with the specified parameter.</exception>
        [DoesNotReturn]
        public static void ThrowUnauthorizedAccessException(string message, Exception innerException)
        {
            throw new UnauthorizedAccessException(message, innerException);
        }

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
    }
}
