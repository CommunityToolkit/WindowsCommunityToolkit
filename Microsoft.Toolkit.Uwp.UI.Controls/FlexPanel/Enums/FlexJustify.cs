// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See the LICENSE file in the project root
// for the license information.
//
// Author(s):
//  - Laurent Sansonetti (native Xamarin flex https://github.com/xamarin/flex)
//  - Stephane Delcroix (.NET port)
//  - Ben Askren (UWP/Uno port)
//
using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Values for <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.FlexItem.Justify" />.
    /// </summary>
    [TypeConverter(typeof(FlexJustifyTypeConverter))]
    public enum FlexJustify
    {
        /// <summary>
        /// Whether an item should be packed around the center.
        /// </summary>
        Center = 2,

        /// <summary>
        /// Whether an item should be packed at the start.
        /// </summary>
        Start = 3,

        /// <summary>
        /// Whether an item should be packed at the end.
        /// </summary>
        End = 4,

        /// <summary>
        /// Whether items should be distributed evenly, the first item being at the start and the last item being at the end.
        /// </summary>
        SpaceBetween = 5,

        /// <summary>
        /// Whether items should be distributed evenly, the first and last items having a half-size space.
        /// </summary>
        SpaceAround = 6,

        /// <summary>
        /// Whether items should be distributed evenly, all items having equal space around them.
        /// </summary>
        SpaceEvenly = 7,
    }
}