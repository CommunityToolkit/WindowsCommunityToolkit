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
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// This class provides static methods helper for executing code in UI thread of the main window.
    /// </summary>
    public static class DispatcherHelper
    {
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
        /// <param name="function">Synchronous function to be executed on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task </returns>
        public static Task<T> ExecuteOnUIThreadAsync<T>(Func<T> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return ExecuteOnUIThreadAsync(CoreApplication.MainView, function, priority);
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
        /// <param name="viewToExecuteOn">View for the <paramref name="function"/>  to be executed on </param>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task/></returns>
        public static Task ExecuteOnUIThreadAsync(CoreApplicationView viewToExecuteOn, Action function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
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
        /// <typeparam name="T">returned data type of the function</typeparam>
        /// <param name="viewToExecuteOn">View for the <paramref name="function"/>  to be executed on </param>
        /// <param name="function">Synchronous function with return type <typeparamref name="T"/> to be executed on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task with type <typeparamref name="T"/></returns>
        public static Task<T> ExecuteOnUIThreadAsync<T>(CoreApplicationView viewToExecuteOn, Func<T> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
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
        public static Task ExecuteOnUIThreadAsync(CoreApplicationView viewToExecuteOn, Func<Task> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
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
        /// <typeparam name="T">returned data type of the function</typeparam>
        /// <param name="viewToExecuteOn">View for the <paramref name="function"/>  to be executed on </param>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task with type <typeparamref name="T"/></returns>
        public static Task<T> ExecuteOnUIThreadAsync<T>(CoreApplicationView viewToExecuteOn, Func<Task<T>> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (viewToExecuteOn == null)
            {
                throw new ArgumentNullException("viewToExecuteOn can't be null!");
            }

            return viewToExecuteOn.Dispatcher.AwaitableRunAsync<T>(function, priority);
        }

        /// <summary>
        /// Extension method for CoreApplicationView. Execute given function asynchronously on the main dispatcher of the current view.
        /// </summary>
        /// <param name="currentView">View that will have its UI thread execute the specified function</param>
        /// <param name="function">Function to execute asynchronously on UI thread of the current view.</param>
        /// <param name="priority">Execute priority within the UI Thread. Default to normal </param>
        /// <returns>Awaitable Task</returns>
        public static Task ExecuteAsync(this CoreApplicationView currentView, Action function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return currentView.Dispatcher.AwaitableRunAsync(function, priority);
        }

        /// <summary>
        /// Extension method for CoreApplicationView. Execute given function asynchronously on the main dispatcher of the current view with a return value.
        /// </summary>
        /// <typeparam name="T">Type of return value</typeparam>
        /// <param name="currentView">View that will have its UI thread execute the specified function</param>
        /// <param name="function">Function to execute asynchronously on UI thread of the current view with a return type <typeparamref name="T"/>.</param>
        /// <param name="priority">Execute priority within the UI Thread. Default to normal </param>
        /// <returns>Awaitable Task with return type <typeparamref name="T"/></returns>
        public static Task<T> ExecuteAsync<T>(this CoreApplicationView currentView, Func<T> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return currentView.Dispatcher.AwaitableRunAsync<T>(function, priority);
        }

        /// <summary>
        /// Extension method for CoreApplicationView. Execute given function asynchronously on the main dispatcher of the current view.
        /// </summary>
        /// <param name="currentView">View that will have its UI thread execute the specified function</param>
        /// <param name="function">Asynchronous Function to execute asynchronously on UI thread of the current view.</param>
        /// <param name="priority">Execute priority within the UI Thread. Default to normal </param>
        /// <returns>Awaitable Task</returns>
        public static Task ExecuteAsync(this CoreApplicationView currentView, Func<Task> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return currentView.Dispatcher.AwaitableRunAsync(function, priority);
        }

        /// <summary>
        /// Extension method for CoreApplicationView. Execute given function asynchronously on the main dispatcher of the current view with a return value.
        /// </summary>
        /// <typeparam name="T">Type of return value</typeparam>
        /// <param name="currentView">View that will have its UI thread execute the specified function</param>
        /// <param name="function">Asynchronous function to execute asynchronously on UI thread of the current view with a return type <typeparamref name="T"/>.</param>
        /// <param name="priority">Execute priority within the UI Thread. Default to normal </param>
        /// <returns>Awaitable Task with return type <typeparamref name="T"/></returns>
        public static Task<T> ExecuteAsync<T>(this CoreApplicationView currentView, Func<Task<T>> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return currentView.Dispatcher.AwaitableRunAsync<T>(function, priority);
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
            return dispatcher.AwaitableRunAsync<object>(
                () =>
                {
                    function();
                    return new object();
                }, priority);
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

            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();

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
        /// <param name="function">Asynchrounous function to be executed asynchrounously on the given dispatcher</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task</returns>
        public static Task AwaitableRunAsync(this CoreDispatcher dispatcher, Func<Task> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return dispatcher.AwaitableRunAsync<object>(
                async () =>
                {
                    var task = function();

                    if (task == null)
                    {
                        throw new InvalidOperationException("Task returned from async function parameter cannot be null!");
                    }

                    await task.ConfigureAwait(false);
                    return new object();
                }, priority);
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

            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();

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
                         taskCompletionSource.SetException(new InvalidOperationException("Task returned from async function parameter cannot be null!"));
                     }
                 }
                 catch (Exception e)
                 {
                     taskCompletionSource.SetException(e);
                 }
             });

            return taskCompletionSource.Task;
        }
    }
}
