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
#if ANNIVERSARY_UPDATE
    /// <summary>
    /// Groups semantically identify that the content in the group must either be displayed as a whole, or not displayed if it cannot fit. Groups also allow creating multiple columns. Supported on Tiles since RTM. Supported on Toasts since Anniversary Update.
    /// </summary>
#else
    /// <summary>
    /// Groups semantically identify that the content in the group must either be displayed as a whole, or not displayed if it cannot fit. Groups also allow creating multiple columns.
    /// </summary>
#endif
    public sealed class AdaptiveGroup
        : ITileBindingContentAdaptiveChild
        , IAdaptiveChild
#if ANNIVERSARY_UPDATE
        , IToastBindingGenericChild
#endif
    {
        /// <summary>
        /// Initializes a new group. Groups semantically identify that the content in the group must either be displayed as a whole, or not displayed if it cannot fit. Groups also allow creating multiple columns.
        /// </summary>
        public AdaptiveGroup() { }

        /// <summary>
        /// The only valid children of groups are <see cref="AdaptiveSubgroup"/>. Each subgroup is displayed as a separate vertical column. Note that you must include at least one subgroup in your group, otherwise an <see cref="InvalidOperationException"/> will be thrown when you try to retrieve the XML for the notification.
        /// </summary>
        public IList<AdaptiveSubgroup> Children { get; private set; } = new List<AdaptiveSubgroup>();

        internal Element_AdaptiveGroup ConvertToElement()
        {
            if (Children.Count == 0)
                throw new InvalidOperationException("Groups must have at least one child subgroup. The Children property had zero items in it.");

            Element_AdaptiveGroup group = new Element_AdaptiveGroup();

            foreach (var subgroup in Children)
                group.Children.Add(subgroup.ConvertToElement());

            return group;
        }
    }
}
