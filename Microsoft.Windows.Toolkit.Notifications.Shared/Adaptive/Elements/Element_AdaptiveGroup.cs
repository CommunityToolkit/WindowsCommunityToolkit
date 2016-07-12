// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved



using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Windows.Toolkit.Notifications.Adaptive.Elements
{

    [NotificationXmlElement("group")]
    internal sealed class Element_AdaptiveGroup : IElement_TileBindingChild, IElement_ToastBindingChild, IElementWithDescendants
    {
        public IList<Element_AdaptiveSubgroup> Children { get; private set; } = new List<Element_AdaptiveSubgroup>();

        public IEnumerable<object> Descendants()
        {
            foreach (Element_AdaptiveSubgroup subgroup in Children)
            {
                // Return the subgroup
                yield return subgroup;

                // And also return its descendants
                foreach (object descendant in subgroup.Descendants())
                    yield return descendant;
            }
        }
    }
}
