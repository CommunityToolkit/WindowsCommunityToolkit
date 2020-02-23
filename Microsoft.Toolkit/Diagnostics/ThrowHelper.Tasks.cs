using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsCompleted"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsCompleted(Task task, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be completed, had status {task.Status}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotCompleted"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotCompleted(Task task, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must not be completed, had status {task.Status}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsCompletedSuccessfully"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsCompletedSuccessfully(Task task, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be completed successfully, had status {task.Status}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotCompletedSuccessfully"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotCompletedSuccessfully(Task task, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must not be completed successfully, had status {task.Status}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsFaulted"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsFaulted(Task task, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be faulted, had status {task.Status}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotFaulted"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotFaulted(Task task, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must not be faulted, had status {task.Status}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsCanceled"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsCanceled(Task task, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be canceled, had status {task.Status}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotCanceled"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotCanceled(Task task, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must not be canceled, had status {task.Status}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasStatusEqualTo"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasStatusEqualTo(Task task, TaskStatus status, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must have status {status}, had status {task.Status}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasStatusNotEqualTo"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasStatusNotEqualTo(TaskStatus status, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must not have status {status}");
        }
    }
}
