// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using Microsoft.Windows.Toolkit.Notifications.Adaptive.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Windows.Toolkit.Notifications
{
    /// <summary>
    /// Defines an attribution text element to be displayed on the Toast notification.
    /// </summary>
    public sealed class ToastGenericAttributionText : IBaseText
    {
        /// <summary>
        /// Constructs a new attribution text element that can be displayed on a Toast notification.
        /// </summary>
        public ToastGenericAttributionText() { }

        /// <summary>
        /// The text to display.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The target locale of the XML payload, specified as a BCP-47 language tags such as "en-US" or "fr-FR". The locale specified here overrides any other specified locale, such as that in binding or visual. If this value is a literal string, this attribute defaults to the user's UI language. If this value is a string reference, this attribute defaults to the locale chosen by Windows Runtime in resolving the string.
        /// </summary>
        public string Language { get; set; }

        internal Element_AdaptiveText ConvertToElement()
        {
            var el = BaseTextHelper.CreateBaseElement(this);

            el.Placement = AdaptiveTextPlacement.Attribution;

            return el;
        }
    }
}
