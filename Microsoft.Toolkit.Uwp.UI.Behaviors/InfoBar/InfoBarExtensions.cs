// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    internal static class InfoBarExtensions
    {
        /// <summary>
        /// Sets the <paramref name="notification"/> content on the <see cref="InfoBar"/>.
        /// </summary>
        /// <param name="infoBar">The <see cref="InfoBar"/> to update.</param>
        /// <param name="notification">The notification to set on the <see cref="InfoBar"/>.</param>
        public static void SetNotification(this InfoBar infoBar, Notification notification)
        {
            infoBar.Title = notification.Title;
            infoBar.Message = notification.Message;
            infoBar.Severity = notification.Severity;
            infoBar.IsIconVisible = notification.IsIconVisible;
            infoBar.Content = notification.Content;
            infoBar.ContentTemplate = notification.ContentTemplate;
            infoBar.ActionButton = notification.ActionButton;
        }
    }
}
