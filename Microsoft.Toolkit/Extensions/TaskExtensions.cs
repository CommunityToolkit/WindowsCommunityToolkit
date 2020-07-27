// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable

namespace Microsoft.Toolkit.Extensions
{
    /// <summary>
    /// Helpers for working with tasks.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Gets the result of a <see cref="Task"/> if available, or <see langword="null"/> otherwise.
        /// </summary>
        /// <param name="task">The input <see cref="Task"/> instance to get the result for.</param>
        /// <returns>The result of <paramref name="task"/> if completed successfully, or <see langword="default"/> otherwise.</returns>
        /// <remarks>
        /// This method does not block if <paramref name="task"/> has not completed yet. Furthermore, it is not generic
        /// and uses reflection to access the <see cref="Task{TResult}.Result"/> property and boxes the result if it's
        /// a value type, which adds overhead. It should only be used when using generics is not possible.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object? ResultOrDefault(this Task task)
        {
            // Check if the instance is a completed Task
            if (
#if NETSTANDARD2_1
                task.IsCompletedSuccessfully
#else
                task.Status == TaskStatus.RanToCompletion
#endif
            )
            {
                Type taskType = task.GetType();

                // Check if the task is actually some Task<T>
                if (
#if NETSTANDARD1_4
                    taskType.GetTypeInfo().IsGenericType &&
#else
                    taskType.IsGenericType &&
#endif
                    taskType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    // Get the Task<T>.Result property
                    PropertyInfo propertyInfo =
#if NETSTANDARD1_4
                        taskType.GetRuntimeProperty(nameof(Task<object>.Result));
#else
                        taskType.GetProperty(nameof(Task<object>.Result));
#endif

                    // Finally retrieve the result
                    return propertyInfo!.GetValue(task);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the result of a <see cref="Task{TResult}"/> if available, or <see langword="default"/> otherwise.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Task{TResult}"/> to get the result for.</typeparam>
        /// <param name="task">The input <see cref="Task{TResult}"/> instance to get the result for.</param>
        /// <returns>The result of <paramref name="task"/> if completed successfully, or <see langword="default"/> otherwise.</returns>
        /// <remarks>This method does not block if <paramref name="task"/> has not completed yet.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: MaybeNull]
        public static T ResultOrDefault<T>(this Task<T> task)
        {
#if NETSTANDARD2_1
            return task.IsCompletedSuccessfully ? task.Result : default;
#else
            return task.Status == TaskStatus.RanToCompletion ? task.Result : default;
#endif
        }
    }
}
