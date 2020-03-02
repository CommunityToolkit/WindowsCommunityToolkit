// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
    /// <summary>
    /// A one-way convert to transform an enumeration value to <see cref="TrueValue"/> if it matches the converter parameter
    /// or <see cref="FalseValue"/> otherwise.
    /// </summary>
    /// <example>
    /// In the code behind:
    ///     - enum MyEnum { Value1, Value2, }
    /// In the resources:
    ///     - &lt;namespace:MyEnum x:Key="MyEnumValue1"&gt;Value1&lt;/namespace:MyEnum&gt;.
    /// In a control:
    ///     - IsEnabled="{x:Bind ViewModel.MyEnum, Converter={StaticResource EnumerationValueConverter}, ConverterParameter={StaticResource MyEnumValue1}}".
    /// </example>
    public sealed class EnumerationValueConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the value to return if the value to convert matches the converter parameter.
        /// </summary>
        public object TrueValue
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the value to return if the value to convert does not match the converter parameter.
        /// </summary>
        public object FalseValue
        {
            get; set;
        }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null)
            {
                throw new ArgumentNullException();
            }

            if (!(value.GetType().IsEnum || value.GetType() == typeof(int)) ||
                !(parameter.GetType().Equals(value.GetType()) || parameter.GetType().Equals(typeof(int))))
            {
                throw new ArgumentException("value is not an enum type or parameter does not match value type nor is convertible to value type.");
            }

            // we convert the enum values to 'int' because the XAML runtime is converting the enum parameters to 'int'.
            return (int)value == (int)parameter ? TrueValue : FalseValue;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
