// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    internal static class InfoBarExtensions
    {
        public static void SetNotification(this InfoBar infoBar, Notification notification)
        {
            infoBar.Title = notification.Title;
            infoBar.Message = notification.Message;
        }
    }
}
