// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
    /// <summary>
    /// A converter that can be used to safely retrieve results from <see cref="Task{T}"/> instances.
    /// This is needed because accessing <see cref="Task{TResult}.Result"/> when the task has not
    /// completed yet will block the current thread and might cause a deadlock (eg. if the task was
    /// scheduled on the same synchronization context where the result is being retrieved from).
    /// The methods in this converter will safely return <see langword="default"/> if the input
    /// task is not set yet, still running, has faulted, or has been canceled.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/default-values">Default values of C# types</seealso>
    public sealed class TaskResultConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //// Check if we need to return a specific type, which only matters for value types where the default won't be null.
            var hasValueTypeTarget = targetType is not null && targetType.IsValueType;
            //// If we have a value type, then calculate it's default value to return, as we probably need it unless the task is completed.
            var defaultValue = hasValueTypeTarget ? Activator.CreateInstance(targetType) : null;

            if (value is Task task)
            {
                // If we have a task, check if we have a result now, otherwise the non-generic version of this
                // function always returns null, so we want to use whatever we actually want the default value to be
                // for our target type (in case it's a value type).
                return task.GetResultOrDefault() ?? defaultValue;
            }
            else if (value is null)
            {
                // If we have a value type, return that value, otherwise this will be null.
                return defaultValue;
            }

            // Otherwise, we'll just pass through whatever value/result was given to us.
            return value;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}