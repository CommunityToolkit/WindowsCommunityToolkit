// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Toolkit.Extensions;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
    /// <summary>
    /// A converter that can be used to safely retrieve results from <see cref="Task{T}"/> instances.
    /// This is needed because accessing <see cref="Task{TResult}.Result"/> when the task has not
    /// completed yet will block the current thread and might cause a deadlock (eg. if the task was
    /// scheduled on the same synchronization context where the result is being retrieved from).
    /// The methods in this converter will safely return <see langword="default"/> if the input
    /// task is still running, or if it has faulted or has been canceled.
    /// </summary>
    public sealed class TaskResultConverter : IValueConverter
    {
        /// <summary>
        /// Non-generic overload of <see cref="Convert{T}"/>, for compatibility reasons.
        /// </summary>
        /// <param name="task">The input <see cref="Task"/>.</param>
        /// <returns>The result of <paramref name="task"/>, as an <see cref="object"/>.</returns>
        [Obsolete("This method is here for compatibility reasons, use the generic overload")]
        public static object Convert(Task task)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the result of a <see cref="Task{TResult}"/> if available, or <see langword="default"/> otherwise.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Task{TResult}"/> to get the result for.</typeparam>
        /// <param name="task">The input <see cref="Task{TResult}"/> instance to get the result for.</param>
        /// <returns>The result of <paramref name="task"/> if completed successfully, or <see langword="default"/> otherwise.</returns>
        /// <remarks>This method does not block if <paramref name="task"/> has not completed yet.</remarks>
        [Pure]
        public static T Convert<T>(Task<T> task)
        {
            return task.ResultOrDefault();
        }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Check if the instance is a completed Task
            if (!(value is Task task) ||
                !task.IsCompletedSuccessfully)
            {
                return null;
            }

            Type taskType = task.GetType();
            Type[] typeArguments = taskType.GetGenericArguments();

            // Check if the task is actually some Task<T>
            if (typeArguments.Length != 1)
            {
                return null;
            }

            // Get the Task<T>.Result property
            PropertyInfo propertyInfo = taskType.GetProperty(nameof(Task<object>.Result));

            // Finally retrieve the result
            return propertyInfo!.GetValue(task);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
