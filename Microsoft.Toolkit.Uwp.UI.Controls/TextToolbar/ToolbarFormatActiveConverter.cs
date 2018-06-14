// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
    /// <summary>
    /// Compares if Formats are equal and returns bool
    /// </summary>
    public class ToolbarFormatActiveConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Format)
            {
                CurrentFormat = (Format)value;
                return CurrentFormat == Format;
            }
            else
            {
                return value;
            }
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (CurrentFormat != null)
            {
                return CurrentFormat;
            }

            return value;
        }

        /// <summary>
        /// Gets or sets the <see cref="Format"/> to compare
        /// </summary>
        public Format Format { get; set; }

        private Format? CurrentFormat { get; set; }
    }
}