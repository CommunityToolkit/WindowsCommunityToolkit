// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Microsoft.Windows.Toolkit.Notifications.Adaptive
{
    internal static class AdaptiveHelper
    {
        internal static object ConvertToElement(object obj)
        {
            if (obj is AdaptiveText)
                return (obj as AdaptiveText).ConvertToElement();

            else if (obj is AdaptiveImage)
                return (obj as AdaptiveImage).ConvertToElement();

            else if (obj is AdaptiveGroup)
                return (obj as AdaptiveGroup).ConvertToElement();

            else if (obj is AdaptiveSubgroup)
                return (obj as AdaptiveSubgroup).ConvertToElement();

            else
                throw new NotImplementedException("Unknown object: " + obj.GetType());
        }
    }
}
