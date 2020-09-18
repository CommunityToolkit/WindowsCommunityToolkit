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
    /// Values for <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.FlexItem.Direction" />.
    /// </summary>
    [TypeConverter(typeof(FlexDirectionTypeConverter))]
    public enum FlexDirection
    {
        /// <summary>
        /// Whether items should be stacked horizontally.
        /// </summary>
        Row = 0,

        /// <summary>
        /// Like Row but in reverse order.
        /// </summary>
        RowReverse = 1,

        /// <summary>
        /// Whether items should be stacked vertically.
        /// </summary>
        Column = 2,

        /// <summary>
        /// Like Column but in reverse order.
        /// </summary>
        ColumnReverse = 3,
    }
}