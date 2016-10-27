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
    public class DispatcherHelper
    {
        /// <summary>
        /// Execute the given function asynchronously on UI thread of the current view
        /// </summary>
        /// <typeparam name="T">returned data type of the function</typeparam>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task with type <typeparamref name="T"/></returns>
        public async Task<T> ExecuteOnUIThreadAsync<T>(Func<Task<T>> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();

            await CoreApplication.GetCurrentView()?.CoreWindow?.Dispatcher?.RunAsync(priority, async () =>
            {
                taskCompletionSource.TrySetResult(await function());
            });

            return await taskCompletionSource.Task;
        }

        /// <summary>
        /// Execute the given function asynchronously on UI thread of the current view
        /// </summary>
        /// <param name="function">Asynchronous function to be executed asynchronously on UI thread</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task</returns>
        public async Task ExecutionOnUIThreadAsync(Func<Task> function, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            await ExecuteOnUIThreadAsync<bool>(
                async () =>
           {
               await function();
               return true;
           }, CoreDispatcherPriority.Normal);
        }
    }
}
