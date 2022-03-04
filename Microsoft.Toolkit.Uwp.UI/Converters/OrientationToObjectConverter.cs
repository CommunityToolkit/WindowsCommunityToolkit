// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
    /// <summary>
    /// This class returns a value depending on the <see cref="Orientation"/> of the value provided to the converter. In case of default will return the <see cref="VerticalValue"/>.
    /// </summary>
    public partial class OrientationToObjectConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// Identifies the <see cref="HorizontalValue"/> property.
        /// </summary>
        public static readonly DependencyProperty HorizontalValueProperty =
            DependencyProperty.Register(nameof(HorizontalValue), typeof(object), typeof(TypeToObjectConverter), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="VerticalValue"/> property.
        /// </summary>
        public static readonly DependencyProperty VerticalValueProperty =
            DependencyProperty.Register(nameof(VerticalValue), typeof(object), typeof(TypeToObjectConverter), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the value to be returned when the <see cref="Orientation"/> of the provided value is <see cref="Orientation.Horizontal"/>.
        /// </summary>
        public object HorizontalValue
        {
            get { return GetValue(HorizontalValueProperty); }
            set { SetValue(HorizontalValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to be returned when the <see cref="Orientation"/> of the provided value is <see cref="Orientation.Vertical"/>.
        /// </summary>
        public object VerticalValue
        {
            get { return GetValue(VerticalValueProperty); }
            set { SetValue(VerticalValueProperty, value); }
        }

        /// <summary>
        /// Convert the <paramref name="value"/>'s Orientation to an other object.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isHorizontal = value != null && value is Orientation orientation && orientation == Orientation.Horizontal;

            // Negate if needed
            if (ConverterTools.TryParseBool(parameter))
            {
                isHorizontal = !isHorizontal;
            }

            return ConverterTools.Convert(isHorizontal ? HorizontalValue : VerticalValue, targetType);
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">Optional parameter. Not used.</param>
        /// <param name="language">The language of the conversion. Not used.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}