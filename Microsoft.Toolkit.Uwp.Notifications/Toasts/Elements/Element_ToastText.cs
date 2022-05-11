// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    internal sealed class Element_ToastText : IElement_ToastBindingChild, IHaveXmlName, IHaveXmlNamedProperties, IHaveXmlText
    {
        internal const ToastTextPlacement DEFAULT_PLACEMENT = ToastTextPlacement.Inline;

        [NotificationXmlContent]
        public string Text { get; set; }

        [NotificationXmlAttribute("lang")]
        public string Lang { get; set; }

        [NotificationXmlAttribute("placement", DEFAULT_PLACEMENT)]
        public ToastTextPlacement Placement { get; set; } = DEFAULT_PLACEMENT;

        /// <inheritdoc/>
        string IHaveXmlName.Name => "text";

        /// <inheritdoc/>
        IEnumerable<KeyValuePair<string, object>> IHaveXmlNamedProperties.EnumerateNamedProperties()
        {
            yield return new("lang", Lang);

            if (Placement != DEFAULT_PLACEMENT)
            {
                yield return new("placement", Placement.ToPascalCaseString());
            }
        }
    }

    internal enum ToastTextPlacement
    {
        Inline,

        [EnumString("attribution")]
        Attribution
    }
}