// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
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
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNull{T}(T,string)"/> (where <typeparamref name="T"/> is <see langword="class"/>) fails.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsNull<T>(T value, string name)
            where T : class
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be null, was {value.ToAssertString()} ({value.GetType().ToTypeString()})");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNull{T}(T,string)"/> (where <typeparamref name="T"/> is <see langword="struct"/>) fails.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsNull<T>(T? value, string name)
            where T : struct
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T?).ToTypeString()}) must be null, was {value.ToAssertString()} ({typeof(T).ToTypeString()})");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> when <see cref="Guard.IsNotNull{T}(T,string)"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentNullExceptionForIsNotNull<T>(string name)
        {
            ThrowArgumentNullException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be not null)");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsOfType{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsOfType<T>(object value, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} must be of type {typeof(T).ToTypeString()}, was {value.GetType().ToTypeString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotOfType{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsNotOfType<T>(object value, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} must not be of type {typeof(T).ToTypeString()}, was {value.GetType().ToTypeString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsOfType"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsOfType(object value, Type type, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} must be of type {type.ToTypeString()}, was {value.GetType().ToTypeString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotOfType"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsNotOfType(object value, Type type, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} must not be of type {type.ToTypeString()}, was {value.GetType().ToTypeString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsAssignableToType{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type being checked against.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsAssignableToType<T>(object value, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} must be assignable to type {typeof(T).ToTypeString()}, was {value.GetType().ToTypeString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotAssignableToType{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type being checked against.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsNotAssignableToType<T>(object value, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} must not be assignable to type {typeof(T).ToTypeString()}, was {value.GetType().ToTypeString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsAssignableToType"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsAssignableToType(object value, Type type, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} must be assignable to type {type.ToTypeString()}, was {value.GetType().ToTypeString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsAssignableToType"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsNotAssignableToType(object value, Type type, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} must not be assignable to type {type.ToTypeString()}, was {value.GetType().ToTypeString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsBitwiseEqualTo{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of input values being compared.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForsBitwiseEqualTo<T>(T value, T target, string name)
            where T : unmanaged
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) is not a bitwise match, was <{value.ToHexString()}> instead of <{target.ToHexString()}>");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsReferenceEqualTo{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of input value being compared.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsReferenceEqualTo<T>(string name)
            where T : class
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be the same instance as the target object");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsReferenceNotEqualTo{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of input value being compared.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsReferenceNotEqualTo<T>(string name)
            where T : class
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must not be the same instance as the target object");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsTrue(bool,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsTrue(string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} must be true, was false");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsTrue(bool,string,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsTrue(string name, string message)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} must be true, was false: {message.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsFalse(bool,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsFalse(string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} must be false, was true");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsFalse(bool,string,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsFalse(string name, string message)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} must be false, was true: {message.ToAssertString()}");
        }
    }
}
