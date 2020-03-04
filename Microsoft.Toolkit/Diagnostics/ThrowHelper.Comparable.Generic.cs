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
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1618", Justification = "Internal helper methods")]
    internal static partial class ThrowHelper
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEqualTo<T>(T value, T target, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be == {target.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotEqualTo<T>(T value, T target, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be != {target.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsLessThan{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsLessThan<T>(T value, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be < {maximum.ToAssertString()}, was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsLessThanOrEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsLessThanOrEqualTo<T>(T value, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be <= \"{maximum}\", was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsGreaterThan{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsGreaterThan<T>(T value, T minimum, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be > \"{minimum}\", was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsGreaterThanOrEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsGreaterThanOrEqualTo<T>(T value, T minimum, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be >= \"{minimum}\", was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRange{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsInRange<T>(T value, T minimum, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be >= \"{minimum}\" and < \"{maximum}\", was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRange{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsNotInRange<T>(T value, T minimum, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be < \"{minimum}\" or >= \"{maximum}\", was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsBetween{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsBetween<T>(T value, T minimum, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be > \"{minimum}\" and < \"{maximum}\", was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsNotBetween{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsNotBetween<T>(T value, T minimum, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be <= \"{minimum}\" or >= \"{maximum}\", was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsBetweenOrEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsBetweenOrEqualTo<T>(T value, T minimum, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be >= \"{minimum}\" and <= \"{maximum}\", was {value.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsNotBetweenOrEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsNotBetweenOrEqualTo<T>(T value, T minimum, T maximum, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must be < \"{minimum}\" or > \"{maximum}\", was {value.ToAssertString()}");
        }
    }
}
