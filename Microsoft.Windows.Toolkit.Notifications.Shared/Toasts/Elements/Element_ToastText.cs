// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved



namespace Microsoft.Windows.Toolkit.Notifications
{
    [NotificationXmlElement("text")]
    internal sealed class Element_ToastText : IElement_ToastBindingChild
    {
        internal const ToastTextPlacement DEFAULT_PLACEMENT = ToastTextPlacement.Inline;

        [NotificationXmlContent]
        public string Text { get; set; }
        
        [NotificationXmlAttribute("lang")]
        public string Lang { get; set; }

        [NotificationXmlAttribute("placement", DEFAULT_PLACEMENT)]
        public ToastTextPlacement Placement { get; set; } = DEFAULT_PLACEMENT;
    }

    internal enum ToastTextPlacement
    {
        Inline,

        [EnumString("attribution")]
        Attribution
    }
}