using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Toolkit.Uwp.Notifications.Adaptive.Elements
{
    [NotificationXmlElement("progressBar")]
    internal sealed class Element_AdaptiveProgressBar : IElement_ToastBindingChild
    {
        [NotificationXmlAttribute("value")]
        public string Value { get; set; }

        [NotificationXmlAttribute("title")]
        public string Title { get; set; }

        [NotificationXmlAttribute("valueStringOverride")]
        public string ValueStringOverride { get; set; }

        [NotificationXmlAttribute("status")]
        public string Status { get; set; }
    }
}
