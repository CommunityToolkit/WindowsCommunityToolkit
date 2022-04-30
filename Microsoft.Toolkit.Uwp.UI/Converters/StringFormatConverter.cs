// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
    /// <summary>
    /// This class provides a binding converter to display formatted strings
    /// </summary>
    public class StringFormatConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the CultureInfo to make converter culture sensitive. The default value is <see cref="CultureInfo.CurrentCulture"/>
        /// </summary>
        public CultureInfo CultureInfo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringFormatConverter"/> class.
        /// </summary>
        public StringFormatConverter()
        {
            CultureInfo = CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Return the formatted string version of the source object.
        /// </summary>
        /// <param name="value">Object to transform to string.</param>
        /// <param name="targetType">The type of the target property, as a type reference</param>
        /// <param name="parameter">An optional parameter to be used in the string.Format method.</param>
        /// <param name="language">The language of the conversion. If language is null or empty then <see cref="CultureInfo"/> will be used.</param>
        /// <returns>Formatted string.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            string formatParameter = parameter as string;
            if (formatParameter == null)
            {
                return value;
            }

            return FormatToString(value, formatParameter, GetCultureInfoOrDefault(language, () => GetDefaultCultureInfo()));
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The type of the target property, as a type reference (System.Type for Microsoft .NET, a TypeName helper struct for Visual C++ component extensions (C++/CX)).</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private object FormatToString(object value, string parameter, CultureInfo cultureInfo)
        {
            try
            {
                return string.Format(cultureInfo, parameter, value);
            }
            catch
            {
                return value;
            }
        }

        private CultureInfo GetCultureInfoOrDefault(string language, Func<CultureInfo> getDefaultCultureInfo)
        {
            CultureInfo cultureInfo;
            if (!TryGetCultureInfo(language, out cultureInfo))
            {
                cultureInfo = getDefaultCultureInfo();
            }

            return cultureInfo;
        }

        private bool TryGetCultureInfo(string language, out CultureInfo cultureInfo)
        {
            cultureInfo = null;
            if (string.IsNullOrEmpty(language))
            {
                return false;
            }

            try
            {
                cultureInfo = CultureInfo.GetCultureInfo(language);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private CultureInfo GetDefaultCultureInfo()
        {
            return CultureInfo ?? CultureInfo.InvariantCulture;
        }
    }
}