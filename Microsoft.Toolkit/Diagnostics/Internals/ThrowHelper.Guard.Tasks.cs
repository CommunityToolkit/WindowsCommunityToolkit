// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Toolkit.Extensions;

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to efficiently throw exceptions.
    /// </summary>
    public static partial class ThrowHelper
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsCompleted"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsCompleted(Task task, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({task.GetType().ToTypeString()}) must be completed, had status {task.Status.ToAssertString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotCompleted"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsNotCompleted(Task task, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({task.GetType().ToTypeString()}) must not be completed, had status {task.Status.ToAssertString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsCompletedSuccessfully"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsCompletedSuccessfully(Task task, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({task.GetType().ToTypeString()}) must be completed successfully, had status {task.Status.ToAssertString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotCompletedSuccessfully"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsNotCompletedSuccessfully(Task task, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({task.GetType().ToTypeString()}) must not be completed successfully, had status {task.Status.ToAssertString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsFaulted"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsFaulted(Task task, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({task.GetType().ToTypeString()}) must be faulted, had status {task.Status.ToAssertString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotFaulted"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsNotFaulted(Task task, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({task.GetType().ToTypeString()}) must not be faulted, had status {task.Status.ToAssertString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsCanceled"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsCanceled(Task task, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({task.GetType().ToTypeString()}) must be canceled, had status {task.Status.ToAssertString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotCanceled"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForIsNotCanceled(Task task, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({task.GetType().ToTypeString()}) must not be canceled, had status {task.Status.ToAssertString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasStatusEqualTo"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForHasStatusEqualTo(Task task, TaskStatus status, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({task.GetType().ToTypeString()}) must have status {status}, had status {task.Status.ToAssertString()}", name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasStatusNotEqualTo"/> fails.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowArgumentExceptionForHasStatusNotEqualTo(Task task, TaskStatus status, string name)
        {
            throw new ArgumentException($"Parameter {name.ToAssertString()} ({task.GetType().ToTypeString()}) must not have status {status.ToAssertString()}", name);
        }
    }
}
