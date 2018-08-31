// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Contains dispatcher code
    /// </summary>
    public sealed partial class WebView
    {
        private const int InitializationBlockingTime = 200;

        /// <summary>
        /// Dispatches a blank frame to allow dispatcher queue to flush. If WebViewControl is not initialized, waits for control
        /// to be loaded by subscribing to Loaded event. Once loaded finishes blocking until control is initialized, then
        /// dispatches a message to perform the <paramref name="callback"/>
        /// </summary>
        /// <param name="callback">The callback to perform after loaded</param>
        /// <remarks>
        /// This exists to
        /// </remarks>
        private void InvokeAfterInitializing(Action callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            // Fast path: if the control is already initialized call the callback directly
            if (WebViewControlInitialized)
            {
                callback();
            }

            // Slower path: control is already loaded, but web view is not initialized
            else if (IsLoaded)
            {
                // Block until the web view initialization is completed on the dispatcher
                InvokeAfterInitializing();
                callback();
            }

            // Slow path: wait for Loaded event and re-enter
            else
            {
                void OnLoaded(object o, RoutedEventArgs e)
                {
                    // This fires early in the cycle.
                    // Unwire the event
                    Loaded -= OnLoaded;

                    // And re-enter the method. IsLoaded will be true
                    InvokeAfterInitializing(callback);
                }

                // Wait for Loaded event to fire, meaning WPF has created most of what is needed for HwndHost to proceed
                Loaded += OnLoaded;
            }
        }

        /// <summary>
        /// Dispatches a blank frame to allow dispatcher queue to flush. If WebViewControl is not initialized, waits for control
        /// to be loaded by subscribing to Loaded event. Once loaded finishes blocking until control is initialized, then
        /// dispatches a message to perform the <paramref name="callback"/>
        /// </summary>
        /// <param name="callback">The callback to perform after loaded</param>
        /// <remarks>
        /// This exists to
        /// </remarks>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <returns>The result of <paramref name="callback"/></returns>
        /// <exception cref="InvalidOperationException">Occurs when the callback cannot be completed because the control is not yet loaded.</exception>
        private T InvokeAfterInitializing<T>(Func<T> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            // Fast path: if the control is already initialized call the callback directly
            if (WebViewControlInitialized)
            {
                return callback();
            }

            // Slightly slower path: control is already loaded, but web view not initialized
            if (IsLoaded)
            {
                // Block until the web view initialization is completed on the dispatcher
                InvokeAfterInitializing();
                return callback();
            }

            // Not possible: we would need to wait for loaded event
            // but may be called early enough in the lifetime where the dispatcher thread is the UI thread
            // such as after InitializeComponents in MainWindow
            throw new InvalidOperationException(DesignerUI.E_WEBVIEW_CANNOT_INVOKE_BEFORE_INIT);
        }

        /// <summary>
        /// Dispatches empty frames to Dispatcher queue while web view is initializing to keep UI responsive
        /// </summary>
        /// <seealso cref="DispatcherExtensions.DoEvents"/>
        [DebuggerStepThrough]
        private void InvokeAfterInitializing(DispatcherPriority priority = DispatcherPriority.ContextIdle)
        {
            do
            {
                Dispatcher.CurrentDispatcher.DoEvents(priority);
            }
            while (!_initializationComplete.WaitOne(InitializationBlockingTime));
        }
    }
}
