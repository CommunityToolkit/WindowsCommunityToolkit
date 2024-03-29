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
    /// The methods in this converter will safely return <see langword="null"/> if the input
    /// task is not set yet, still running, has faulted, or has been canceled.
    /// </summary>
    public sealed class TaskResultConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Task task)
            {
                return task.GetResultOrDefault();
            }
            else if (value is null)
            {
                return null;
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