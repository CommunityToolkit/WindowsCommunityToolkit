// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Threading;

namespace System.Threading.Tasks
{
    /// <summary>
    /// Contains extensions to process a task within a nested Windows Presentation Foundation (WPF) message loop
    /// </summary>
    internal static partial class TaskExtensions
    {
        public static T WaitWithNestedMessageLoop<T>(this Task<T> task, Dispatcher dispatcher)
        {
            // Check if we have a valid dispatcher
            if (dispatcher != null
                && !dispatcher.HasShutdownStarted
                && !dispatcher.HasShutdownFinished)
            {
                // Set the priority to ContextIdle to force the queue to flush higher priority events
                var frame = new DispatcherFrame();
                task.ContinueWith(_ => frame.Continue = false, TaskScheduler.Default);
                Dispatcher.PushFrame(frame);
            }

            return task.Result;
        }
    }
}
