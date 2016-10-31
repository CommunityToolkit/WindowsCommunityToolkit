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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

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
        public static async Task<T> ExecuteOnUIThreadAsync<T>(Func<Task<T>> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return await ExecuteOnUIThreadAsync<T>(CoreApplication.MainView, function, priority);
        }

        /// <summary>
        /// Execute the given function asynchronously on given view's UI thread. Default view is the main view.
        /// </summary>
        /// <typeparam name="T">returned data type of the function</typeparam>
        /// <param name="viewToExecuteOn">View for the <paramref name="function"/>  to be executed on </param>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task with type <typeparamref name="T"/></returns>
        public static async Task<T> ExecuteOnUIThreadAsync<T>(CoreApplicationView viewToExecuteOn, Func<Task<T>> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return await viewToExecuteOn?.Dispatcher?.AwaitableRunAsync<T>(function, priority);
        }

        /// <summary>
        /// Execute the given function asynchronously on given view's UI thread. Default view is the main view.
        /// </summary>
        /// <param name="viewToExecuteOn">View for the <paramref name="function"/>  to be executed on </param>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task</returns>
        public static async Task ExecuteOnUIThreadAsync(CoreApplicationView viewToExecuteOn, Func<Task> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            await ExecuteOnUIThreadAsync<bool>(
                viewToExecuteOn,
                async () =>
            {
                await function();
                return true;
            }, priority);
        }

        /// <summary>
        /// Execute the given function asynchronously on UI thread of the main view
        /// </summary>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task</returns>
        public static async Task ExecuteOnUIThreadAsync(Func<Task> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            await ExecuteOnUIThreadAsync<bool>(
                async () =>
           {
               await function();
               return true;
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
        public static async Task<T> AwaitableRunAsync<T>(this CoreDispatcher dispatcher, Func<Task<T>> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();

            await dispatcher?.RunAsync(priority, async () =>
            {
                taskCompletionSource.SetResult(await function());
            });

            return await taskCompletionSource.Task;
        }

        /// <summary>
        /// Extension method for CoreDispatcher. Offering an actual awaitable Task with optional result that will be executed on the given dispatcher
        /// </summary>
        /// <param name="dispatcher">Dispatcher of a thread to run <paramref name="function"/></param>
        /// <param name="function">Asynchrounous function to be executed asynchrounously on the given dispatcher</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task</returns>
        public static async Task AwaitableRunAsync(this CoreDispatcher dispatcher, Func<Task> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            await dispatcher.AwaitableRunAsync<bool>(
                async () =>
            {
                await function();
                return true;
            }, priority);
        }
    }
}
