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
    public class ToolbarFormatterActiveConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Formatter formatter)
            {
                CurrentFormatter = formatter.GetType();
                return CurrentFormatter.ToString() == FormatterType;
            }
            else
            {
                return value;
            }
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (CurrentFormatter != null)
            {
                return CurrentFormatter;
            }

            return value;
        }

        /// <summary>
        /// Gets or sets the <see cref="Formatter"/>'s <see cref="Type"/> to compare
        /// </summary>
        public string FormatterType { get; set; }

        private Type CurrentFormatter { get; set; }
    }
}