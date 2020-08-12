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
    /// Helper methods to efficiently throw exceptions.
    /// </summary>
    public static partial class ThrowHelper
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsDefault{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of <see langword="struct"/> value type being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsDefault<T>(T value, string name)
            where T : struct
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be the default value {default(T).ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotDefault{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of <see langword="struct"/> value type being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsNotDefault<T>(string name)
            where T : struct
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must not be the default value {default(T).ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEqualTo{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of values being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsEqualTo<T>(T value, T target, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be equal to {target.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEqualTo{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of values being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsNotEqualTo<T>(T value, T target, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must not be equal to {target.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsBitwiseEqualTo{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of input values being compared.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForBitwiseEqualTo<T>(T value, T target, string name)
            where T : unmanaged
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) is not a bitwise match, was <{value.ToHexString()}> instead of <{target.ToHexString()}>");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsLessThan{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of values being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeExceptionForIsLessThan<T>(T value, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, value!, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be less than {maximum.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsLessThanOrEqualTo{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of values being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeExceptionForIsLessThanOrEqualTo<T>(T value, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, value!, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be less than or equal to {maximum.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsGreaterThan{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of values being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeExceptionForIsGreaterThan<T>(T value, T minimum, string name)
        {
            ThrowArgumentOutOfRangeException(name, value!, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be greater than {minimum.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsGreaterThanOrEqualTo{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of values being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeExceptionForIsGreaterThanOrEqualTo<T>(T value, T minimum, string name)
        {
            ThrowArgumentOutOfRangeException(name, value!, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be greater than or equal to {minimum.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRange{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of values being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeExceptionForIsInRange<T>(T value, T minimum, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, value!, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be in the range given by {minimum.ToAssertString()} and {maximum.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRange{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of values being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeExceptionForIsNotInRange<T>(T value, T minimum, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, value!, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must not be in the range given by {minimum.ToAssertString()} and {maximum.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsBetween{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of values being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeExceptionForIsBetween<T>(T value, T minimum, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, value!, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be between {minimum.ToAssertString()} and {maximum.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsNotBetween{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of values being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeExceptionForIsNotBetween<T>(T value, T minimum, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, value!, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must not be between {minimum.ToAssertString()} and {maximum.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsBetweenOrEqualTo{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of values being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeExceptionForIsBetweenOrEqualTo<T>(T value, T minimum, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, value!, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be between or equal to {minimum.ToAssertString()} and {maximum.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsNotBetweenOrEqualTo{T}"/> fails.
        /// </summary>
        /// <typeparam name="T">The type of values being tested.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeExceptionForIsNotBetweenOrEqualTo<T>(T value, T minimum, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, value!, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must not be between or equal to {minimum.ToAssertString()} and {maximum.ToAssertString()}, was {value.ToAssertString()}");
        }
    }
}
