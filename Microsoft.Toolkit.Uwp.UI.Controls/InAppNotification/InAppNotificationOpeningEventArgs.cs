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
    /// A delegate for <see cref="InAppNotification"/> opening.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void InAppNotificationOpeningEventHandler(object sender, InAppNotificationOpeningEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="InAppNotification"/> Dismissing event.
    /// </summary>
    public class InAppNotificationOpeningEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InAppNotificationOpeningEventArgs"/> class.
        /// </summary>
        public InAppNotificationOpeningEventArgs()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the notification should be opened.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
