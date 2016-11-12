// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Helpers
{
    /// <summary>
    /// Task with implemented INotifyPropertyChanged
    /// </summary>
    /// <typeparam name="TResult">type of returned Result</typeparam>
    public class TaskWithNotification<TResult> : INotifyPropertyChanged, ITaskWithNotification
    {
        /// <summary>
        /// Gets the task that is being run
        /// </summary>
        public Task<TResult> InnerTask { get; }

        private bool isStarted;

        /// <summary>
        /// Gets a value indicating whether the task has been started
        /// </summary>
        public bool IsStarted
        {
            get
            {
                return InnerTask != null && isStarted;
            }

            private set
            {
                isStarted = value;
                RaiseProperties(nameof(IsStarted), nameof(IsRunning));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the has been cancelled
        /// </summary>
        public bool IsCanceled => InnerTask.IsCanceled;

        /// <summary>
        /// Gets a value indicating whether the task is completed (cancelled/faulted/successfull)
        /// </summary>
        public bool IsCompleted => InnerTask.IsCompleted;

        /// <summary>
        /// Gets a value indicating whether the task faulted
        /// </summary>
        public bool IsFaulted => InnerTask.IsFaulted;

        /// <summary>
        /// Gets a value indicating whether the task is running
        /// </summary>
        public bool IsRunning => IsStarted && !IsCompleted;

        /// <summary>
        /// Gets a value indicating whether the task has run to completion
        /// </summary>
        public bool IsSuccessfull => InnerTask.Status == TaskStatus.RanToCompletion;

        /// <summary>
        /// Gets Result of the task
        /// </summary>
        public TResult Result => IsSuccessfull ? InnerTask.Result : default(TResult);

        /// <summary>
        /// Gets inner task's result
        /// </summary>
        object ITaskWithNotification.Result => Result;

        /// <summary>
        /// Event fired when properties change
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event fired when the task's state changes
        /// </summary>
        public event EventHandler<TaskStatus> OnTaskStateChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskWithNotification{TResult}"/> class.
        /// </summary>
        /// <param name="task">task to run</param>
        public TaskWithNotification(Task<TResult> task)
        {
            InnerTask = task;
            if (InnerTask.IsCompleted)
            {
                RaiseProperties(nameof(IsRunning), nameof(IsCompleted), nameof(IsSuccessfull), nameof(Result), nameof(InnerTask));
            }
            else
            {
                Task ignore = FireTask(InnerTask);
                IsStarted = true;
            }
        }

        /// <summary>
        /// Method raising properties
        /// </summary>
        /// <param name="properties">names of properties to be raised</param>
        public void RaiseProperties(params string[] properties)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                foreach (var item in properties)
                {
                    handler.Invoke(this, new PropertyChangedEventArgs(item));
                }
            }
        }

        private async Task FireTask(Task<TResult> task)
        {
            try
            {
                await task;
            }
            catch
            {
            }
            finally
            {
                RaiseProperties(nameof(IsRunning), nameof(IsCompleted), task.IsCanceled ? nameof(IsCanceled) : task.IsFaulted ? nameof(IsFaulted) : nameof(IsSuccessfull), nameof(Result));
                OnTaskStateChanged?.Invoke(this, task.Status);
            }
        }
    }
}
