// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.Extensions;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UnitTests
{
    /// <summary>
    /// Base class to be used in API tests which require UI layout or rendering to occur first.
    /// For more E2E scenarios or testing components for user interation, see integration test suite instead.
    /// Use this class when an API needs direct access to test functions of the UI itself in more simplistic scenarios (i.e. visual tree helpers).
    /// </summary>
    public class VisualUITestBase
    {
        /// <summary>
        /// Sets the content of the test app to a simple <see cref="FrameworkElement"/> to load into the visual tree.
        /// Waits for that element to be loaded and rendered before returning.
        /// </summary>
        /// <param name="content">Content to set in test app.</param>
        /// <returns>When UI is loaded.</returns>
        protected Task<bool> SetTestContentAsync(FrameworkElement content, TaskCreationOptions? options = null)
        {
            var taskCompletionSource = options.HasValue ? new TaskCompletionSource<bool>(options.Value)
                : new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            App.DispatcherQueue.EnqueueAsync(() =>
            {
                async void Callback(object sender, RoutedEventArgs args)
                {
                    content.Loaded -= Callback;

                    // Wait for first Render pass
                    await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { }, TaskCreationOptions.RunContinuationsAsynchronously);

                    taskCompletionSource.SetResult(true);
                }

                // Going to wait for our original content to unload
                content.Loaded += Callback;

                // Trigger that now
                try
                {
                    App.ContentRoot = content;
                }
                catch (Exception e)
                {
                    taskCompletionSource.SetException(e);
                }
            });

            return taskCompletionSource.Task;
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            // Need to await TaskCompletionSource before Task
            // See https://devblogs.microsoft.com/premier-developer/the-danger-of-taskcompletionsourcet-class/
            await taskCompletionSource.Task;

            await App.DispatcherQueue.EnqueueAsync(() =>
            {
                void Callback(object sender, RoutedEventArgs args)
                {
                    App.ContentRoot.Unloaded -= Callback;

                    taskCompletionSource.SetResult(true);
                }

                // Going to wait for our original content to unload
                App.ContentRoot.Unloaded += Callback;

                // Trigger that now
                App.ContentRoot = null;
            });
        }
    }
}
