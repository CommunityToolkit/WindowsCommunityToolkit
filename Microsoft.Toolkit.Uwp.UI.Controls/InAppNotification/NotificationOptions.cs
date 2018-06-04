// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Base class that contains options of notification
    /// </summary>
    internal class NotificationOptions
    {
        /// <summary>
        /// Gets or sets duration of the stacked notification
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets Content of the notification
        /// Could be either a <see cref="string"/> or a <see cref="UIElement"/> or a <see cref="DataTemplate"/>
        /// </summary>
        public object Content { get; set; }
    }
}
