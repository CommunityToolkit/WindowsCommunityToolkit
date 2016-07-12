// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Windows.Toolkit.Notifications
{
    /// <summary>
    /// Elements that can be direct children of adaptive content, including (<see cref="AdaptiveText"/>, <see cref="AdaptiveImage"/>, and <see cref="AdaptiveGroup"/>).
    /// </summary>
    public interface IAdaptiveChild
    {
        // Blank interface simply for compile-enforcing the child types in the list.
    }

    /// <summary>
    /// Elements that can be direct children of an <see cref="AdaptiveSubgroup"/>, including  (<see cref="AdaptiveText"/> and <see cref="AdaptiveImage"/>).
    /// </summary>
    public interface IAdaptiveSubgroupChild
    {
        // Blank interface simply for compile-enforcing the child types in the list.
    }
}
