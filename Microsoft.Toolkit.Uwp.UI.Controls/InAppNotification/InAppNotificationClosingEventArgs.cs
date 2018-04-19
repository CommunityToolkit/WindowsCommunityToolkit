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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A delegate for <see cref="InAppNotification"/> dismissing.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void InAppNotificationClosingEventHandler(object sender, InAppNotificationClosingEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="InAppNotification"/> Dismissing event.
    /// </summary>
    public class InAppNotificationClosingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InAppNotificationClosingEventArgs"/> class.
        /// </summary>
        /// <param name="dismissKind">Dismiss kind that triggered the closing event</param>
        public InAppNotificationClosingEventArgs(InAppNotificationDismissKind dismissKind)
        {
            DismissKind = dismissKind;
        }

        /// <summary>
        /// Gets the kind of action for the closing event.
        /// </summary>
        public InAppNotificationDismissKind DismissKind { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the notification should be closed.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
