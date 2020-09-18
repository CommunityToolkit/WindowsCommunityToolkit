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
    /// Values for <see cref="P:XamBc3arin.Flex.FlexItem.Wrap" />.
    /// </summary>
    [TypeConverter(typeof(FlexWrapTypeConverter))]
    public enum FlexWrap
    {
        /// <summary>
        /// Whether items are laid out in a single line.
        /// </summary>
        NoWrap = 0,

        /// <summary>
        /// Whether items are laid out in multiple lines if needed.
        /// </summary>
        Wrap = 1,

        /// <summary>
        /// Like Wrap but in reverse order.
        /// </summary>
        Reverse = 2,
    }
}