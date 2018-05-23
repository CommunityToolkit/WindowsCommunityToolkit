// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// This class provides static methods helper for executing code in UI thread of the main window.
    /// </summary>
    public static class DispatcherHelper
    {
        /// <summary>
        /// Execute the given function asynchronously on UI thread of the main view
        /// </summary>
        /// <typeparam name="T">returned data type of the function</typeparam>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task with type <typeparamref name="T"/></returns>
        public static Task<T> ExecuteOnUIThreadAsync<T>(Func<Task<T>> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return ExecuteOnUIThreadAsync<T>(CoreApplication.MainView, function, priority);
        }

        /// <summary>
        /// Execute the given function asynchronously on given view's UI thread. Default view is the main view.
        /// </summary>
        /// <typeparam name="T">returned data type of the function</typeparam>
        /// <param name="viewToExecuteOn">View for the <paramref name="function"/>  to be executed on </param>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task with type <typeparamref name="T"/></returns>
        public static Task<T> ExecuteOnUIThreadAsync<T>(this CoreApplicationView viewToExecuteOn, Func<Task<T>> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (viewToExecuteOn == null)
            {
                throw new ArgumentNullException("viewToExecuteOn can't be null!");
            }

            return viewToExecuteOn.Dispatcher.AwaitableRunAsync<T>(function, priority);
        }

        /// <summary>
        /// Execute the given function asynchronously on given view's UI thread. Default view is the main view.
        /// </summary>
        /// <param name="viewToExecuteOn">View for the <paramref name="function"/>  to be executed on </param>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task</returns>
        public static Task ExecuteOnUIThreadAsync(this CoreApplicationView viewToExecuteOn, Func<Task> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (viewToExecuteOn == null)
            {
                throw new ArgumentNullException("viewToExecuteOn can't be null!");
            }

            return viewToExecuteOn.Dispatcher.AwaitableRunAsync(function, priority);
        }

        /// <summary>
        /// Execute the given function asynchronously on UI thread of the main view
        /// </summary>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task</returns>
        public static Task ExecuteOnUIThreadAsync(Func<Task> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return ExecuteOnUIThreadAsync(CoreApplication.MainView, function, priority);
        }

        /// <summary>
        /// Execute the given function asynchronously on given view's UI thread. Default view is the main view.
        /// </summary>
        /// <param name="viewToExecuteOn">View for the <paramref name="function"/>  to be executed on </param>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task/></returns>
        public static Task ExecuteOnUIThreadAsync(this CoreApplicationView viewToExecuteOn, Action function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (viewToExecuteOn == null)
            {
                throw new ArgumentNullException("viewToExecuteOn can't be null!");
            }

            return viewToExecuteOn.Dispatcher.AwaitableRunAsync(function, priority);
        }

        /// <summary>
        /// Execute the given function asynchronously on given view's UI thread. Default view is the main view.
        /// </summary>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task/></returns>
        public static Task ExecuteOnUIThreadAsync(Action function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return ExecuteOnUIThreadAsync(CoreApplication.MainView, function, priority);
        }

        /// <summary>
        /// Execute the given function asynchronously on given view's UI thread. Default view is the main view.
        /// </summary>
        /// <typeparam name="T">returned data type of the function</typeparam>
        /// <param name="viewToExecuteOn">View for the <paramref name="function"/>  to be executed on </param>
        /// <param name="function">Synchronous function with return type <typeparamref name="T"/> to be executed on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task with type <typeparamref name="T"/></returns>
        public static Task<T> ExecuteOnUIThreadAsync<T>(this CoreApplicationView viewToExecuteOn, Func<T> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (viewToExecuteOn == null)
            {
                throw new ArgumentNullException("viewToExecuteOn can't be null!");
            }

            return viewToExecuteOn.Dispatcher.AwaitableRunAsync<T>(function, priority);
        }

        /// <summary>
        /// Execute the given function asynchronously on given view's UI thread. Default view is the main view.
        /// </summary>
        /// <typeparam name="T">returned data type of the function</typeparam>
        /// <param name="function">Synchronous function to be executed on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task </returns>
        public static Task<T> ExecuteOnUIThreadAsync<T>(Func<T> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return ExecuteOnUIThreadAsync(CoreApplication.MainView, function, priority);
        }

        /// <summary>
        /// Extension method for CoreDispatcher. Offering an actual awaitable Task with optional result that will be executed on the given dispatcher
        /// </summary>
        /// <typeparam name="T">returned data type of the function</typeparam>
        /// <param name="dispatcher">Dispatcher of a thread to run <paramref name="function"/></param>
        /// <param name="function">Asynchrounous function to be executed asynchrounously on the given dispatcher</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task with type <typeparamref name="T"/></returns>
        public static Task<T> AwaitableRunAsync<T>(this CoreDispatcher dispatcher, Func<Task<T>> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (function == null)
            {
                throw new ArgumentNullException("function can't be null!");
            }

            var taskCompletionSource = new TaskCompletionSource<T>();

            var ignored = dispatcher.RunAsync(priority, async () =>
            {
                try
                {
                    var awaitableResult = function();
                    if (awaitableResult != null)
                    {
                        var result = await awaitableResult.ConfigureAwait(false);
                        taskCompletionSource.SetResult(result);
                    }
                    else
                    {
                        taskCompletionSource.SetException(new InvalidOperationException("The Task returned by function cannot be null."));
                    }
                }
                catch (Exception e)
                {
                    taskCompletionSource.SetException(e);
                }
            });

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Extension method for CoreDispatcher. Offering an actual awaitable Task with optional result that will be executed on the given dispatcher
        /// </summary>
        /// <param name="dispatcher">Dispatcher of a thread to run <paramref name="function"/></param>
        /// <param name="function">Asynchrounous function to be executed asynchrounously on the given dispatcher</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task</returns>
        public static Task AwaitableRunAsync(this CoreDispatcher dispatcher, Func<Task> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (function == null)
            {
                throw new ArgumentNullException("function can't be null!");
            }

            var taskCompletionSource = new TaskCompletionSource<object>();

            var ignored = dispatcher.RunAsync(priority, async () =>
            {
                try
                {
                    var awaitableResult = function();
                    if (awaitableResult != null)
                    {
                        await awaitableResult.ConfigureAwait(false);
                        taskCompletionSource.SetResult(null);
                    }
                    else
                    {
                        taskCompletionSource.SetException(new InvalidOperationException("The Task returned by function cannot be null."));
                    }
                }
                catch (Exception e)
                {
                    taskCompletionSource.SetException(e);
                }
            });

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Extension method for CoreDispatcher. Offering an actual awaitable Task with optional result that will be executed on the given dispatcher
        /// </summary>
        /// <typeparam name="T">returned data type of the function</typeparam>
        /// <param name="dispatcher">Dispatcher of a thread to run <paramref name="function"/></param>
        /// <param name="function"> Function to be executed asynchrounously on the given dispatcher</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task</returns>
        public static Task<T> AwaitableRunAsync<T>(this CoreDispatcher dispatcher, Func<T> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (function == null)
            {
                throw new ArgumentNullException("function can't be null!");
            }

            var taskCompletionSource = new TaskCompletionSource<T>();

            var ignored = dispatcher.RunAsync(priority, () =>
            {
                try
                {
                    taskCompletionSource.SetResult(function());
                }
                catch (Exception e)
                {
                    taskCompletionSource.SetException(e);
                }
            });

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Extension method for CoreDispatcher. Offering an actual awaitable Task with optional result that will be executed on the given dispatcher
        /// </summary>
        /// <param name="dispatcher">Dispatcher of a thread to run <paramref name="function"/></param>
        /// <param name="function"> Function to be executed asynchrounously on the given dispatcher</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task</returns>
        public static Task AwaitableRunAsync(this CoreDispatcher dispatcher, Action function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return dispatcher.AwaitableRunAsync(
                () =>
                {
                    function();
                    return (object)null;
                }, priority);
        }
    }
}
