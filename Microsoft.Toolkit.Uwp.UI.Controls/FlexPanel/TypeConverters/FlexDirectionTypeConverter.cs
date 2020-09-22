// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See the LICENSE file in the project root
// for the license information.
//
// Author(s):
//  - Laurent Sansonetti (native Xamarin flex https://github.com/xamarin/flex)
//  - Stephane Delcroix (.NET port)
//  - Ben Askren (UWP/Uno port)
//
using Microsoft.Toolkit.Diagnostics;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// String to FlexDirection type converter
    /// </summary>
    internal class FlexDirectionTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                if (Enum.TryParse(stringValue, true, out FlexDirection aligncontent))
                {
                    return aligncontent;
                }

                if (stringValue.Equals("row-reverse", StringComparison.OrdinalIgnoreCase))
                {
                    return FlexDirection.RowReverse;
                }

                if (stringValue.Equals("column-reverse", StringComparison.OrdinalIgnoreCase))
                {
                    return FlexDirection.ColumnReverse;
                }
            }

            ThrowHelper.ThrowInvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", value, typeof(FlexDirection)));
            return null;
        }
    }
}