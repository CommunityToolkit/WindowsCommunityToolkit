// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Notifications.Adaptive.Elements
{
    internal sealed class Element_AdaptiveProgressBar : IElement_ToastBindingChild, INotificationXmlElement
    {
        [NotificationXmlAttribute("value")]
        public string Value { get; set; }

        [NotificationXmlAttribute("title")]
        public string Title { get; set; }

        [NotificationXmlAttribute("valueStringOverride")]
        public string ValueStringOverride { get; set; }

        [NotificationXmlAttribute("status")]
        public string Status { get; set; }

        /// <inheritdoc/>
        string INotificationXmlElement.Name => "progress";
    }
}