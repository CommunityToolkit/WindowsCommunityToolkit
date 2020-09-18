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
    /// String to FlexAlignItems type converter
    /// </summary>
    internal class FlexAlignItemsTypeConverter : TypeConverter
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
                if (Enum.TryParse(stringValue, true, out FlexAlignItems alignitems))
                {
                    return alignitems;
                }

                if (stringValue.Equals("flex-start", StringComparison.OrdinalIgnoreCase))
                {
                    return FlexAlignItems.Start;
                }

                if (stringValue.Equals("flex-end", StringComparison.OrdinalIgnoreCase))
                {
                    return FlexAlignItems.End;
                }
            }

            throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", value, typeof(FlexAlignItems)));
        }
    }
}