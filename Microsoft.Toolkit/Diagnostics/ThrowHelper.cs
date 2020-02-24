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
#pragma warning disable CS0419
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNull{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNull<T>(T value, string name)
            where T : class
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be null, was \"{value}\" ({value.GetType().ToTypeString()})");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNull{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNull<T>(T? value, string name)
            where T : struct
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T?).ToTypeString()}) must be null, was \"{value}\" ({typeof(T).ToTypeString()})");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> when <see cref="Guard.IsNotNull{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentNullExceptionForIsNotNull<T>(string name)
        {
            ThrowArgumentNullException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be not null)");
        }
#pragma warning restore CS0419

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsOfType{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsOfType<T>(object value, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" must be of type {typeof(T).ToTypeString()}, was {value.GetType().ToTypeString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsOfType"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsOfType(object value, Type type, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" must be of type {type.ToTypeString()}, was {value.GetType().ToTypeString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsAssignableToType{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsAssignableToType<T>(object value, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" must be assignable to type {typeof(T).ToTypeString()}, was {value.GetType().ToTypeString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsAssignableToType"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsAssignableToType(object value, Type type, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" must be assignable to type {type.ToTypeString()}, was {value.GetType().ToTypeString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEqualTo<T>(T value, T target, string name)
            where T : notnull, IEquatable<T>
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be == \"{target}\", was \"{value}\"");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotEqualTo<T>(T value, T target, string name)
            where T : notnull, IEquatable<T>
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be != \"{target}\", was \"{value}\"");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsBitwiseEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForsBitwiseEqualTo<T>(T value, T target, string name)
            where T : unmanaged
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) is not a bitwise match, was \"{value.ToHexString()}\" instead of \"{target.ToHexString()}\"");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsReferenceEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsReferenceEqualTo<T>(string name)
            where T : class
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be the same instance as the target object");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsReferenceNotEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsReferenceNotEqualTo<T>(string name)
            where T : class
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must not be the same instance as the target object");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsTrue"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsTrue(string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" must be true, was false");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsFalse"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsFalse(string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" must be false, was true");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsLessThan{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsLessThan<T>(T value, T max, string name)
            where T : notnull, IComparable<T>
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be < \"{max}\", was \"{value}\"");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsLessThanOrEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsLessThanOrEqualTo<T>(T value, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be <= \"{maximum}\", was \"{value}\"");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsGreaterThan{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsGreaterThan<T>(T value, T minimum, string name)
            where T : notnull, IComparable<T>
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be > \"{minimum}\", was \"{value}\"");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsGreaterThanOrEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsGreaterThanOrEqualTo<T>(T value, T minimum, string name)
            where T : notnull, IComparable<T>
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be >= \"{minimum}\", was \"{value}\"");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRange{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsInRange<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be >= \"{minimum}\" and < \"{maximum}\", was \"{value}\"");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRange{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsNotInRange<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be < \"{minimum}\" or >= \"{maximum}\", was \"{value}\"");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsBetween{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsBetween<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be > \"{minimum}\" and < \"{maximum}\", was \"{value}\"");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsNotBetween{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsNotBetween<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be <= \"{minimum}\" or >= \"{maximum}\", was \"{value}\"");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsBetweenOrEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsBetweenOrEqualTo<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be >= \"{minimum}\" and <= \"{maximum}\", was \"{value}\"");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsNotBetweenOrEqualTo{T}"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsNotBetweenOrEqualTo<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" ({typeof(T).ToTypeString()}) must be < \"{minimum}\" or > \"{maximum}\", was \"{value}\"");
        }
    }
}
