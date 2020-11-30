// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Microsoft.Toolkit.Extensions;

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to efficiently throw exceptions.
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
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNull{T}(T,string)"/> (where <typeparamref name="T"/> is <see langword="class"/>) fails.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsNull<T>(T value, string name)
            where T : class
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be null, was {value.ToAssertString()} ({value.GetType().ToTypeString()})", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNull{T}(T,string)"/> (where <typeparamref name="T"/> is <see langword="struct"/>) fails.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsNull<T>(T? value, string name)
            where T : struct
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({typeof(T?).ToTypeString()}) must be null, was {value.ToAssertString()} ({typeof(T).ToTypeString()})", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> when <see cref="Guard.IsNotNull{T}(T,string)"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        [DoesNotReturn]
        internal static void ThrowArgumentNullExceptionForIsNotNull<T>(string name)
        {
            throw new ArgumentNullException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be not null)");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsOfType{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsOfType<T>(object value, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} must be of type {typeof(T).ToTypeString()}, was {value.GetType().ToTypeString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotOfType{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsNotOfType<T>(object value, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} must not be of type {typeof(T).ToTypeString()}, was {value.GetType().ToTypeString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsOfType"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsOfType(object value, Type type, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} must be of type {type.ToTypeString()}, was {value.GetType().ToTypeString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotOfType"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsNotOfType(object value, Type type, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} must not be of type {type.ToTypeString()}, was {value.GetType().ToTypeString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsAssignableToType{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type being checked against.</typeparam>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsAssignableToType<T>(object value, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} must be assignable to type {typeof(T).ToTypeString()}, was {value.GetType().ToTypeString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotAssignableToType{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type being checked against.</typeparam>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsNotAssignableToType<T>(object value, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} must not be assignable to type {typeof(T).ToTypeString()}, was {value.GetType().ToTypeString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsAssignableToType"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsAssignableToType(object value, Type type, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} must be assignable to type {type.ToTypeString()}, was {value.GetType().ToTypeString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsAssignableToType"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsNotAssignableToType(object value, Type type, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} must not be assignable to type {type.ToTypeString()}, was {value.GetType().ToTypeString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsReferenceEqualTo{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of input value being compared.</typeparam>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsReferenceEqualTo<T>(string name)
            where T : class
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be the same instance as the target object", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsReferenceNotEqualTo{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of input value being compared.</typeparam>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsReferenceNotEqualTo<T>(string name)
            where T : class
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must not be the same instance as the target object", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsTrue(bool,string)"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsTrue(string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} must be true, was false", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsTrue(bool,string,string)"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsTrue(string name, string message)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} must be true, was false: {message.ToAssertString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsFalse(bool,string)"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsFalse(string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} must be false, was true", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsFalse(bool,string,string)"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsFalse(string name, string message)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} must be false, was true: {message.ToAssertString()}", name);
        }
    }
}
