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

using System.Windows.Threading;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides additional functionality to the <see cref="Dispatcher"/> object.
    /// </summary>
    internal static class DispatcherExtensions
    {
        /// <summary>
        /// Pushes an empty <see cref="DispatcherFrame"/> at <see cref="DispatcherPriority.ContextIdle"/>.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/> instance.</param>
        /// <remarks>
        /// No frame is pushed if the <paramref name="dispatcher"/> is null, <see cref="Dispatcher.HasShutdownStarted"/> is true, or <see cref="Dispatcher.HasShutdownFinished"/> is true.
        /// </remarks>
        internal static void DoEvents(this Dispatcher dispatcher)
        {
            object ExitFrame(object arg)
            {
                ((DispatcherFrame)arg).Continue = false;
                return null;
            }

            // Check if we have a valid dispatcher
            if (dispatcher != null
                && !dispatcher.HasShutdownStarted
                && !dispatcher.HasShutdownFinished)
            {
                // Set the priority to ContextIdle to force the queue to flush higher priority events
                var frame = new DispatcherFrame();
                dispatcher.BeginInvoke(
                    DispatcherPriority.ContextIdle,
                    new DispatcherOperationCallback(ExitFrame),
                    frame);
                Dispatcher.PushFrame(frame);
            }
        }
    }
}
