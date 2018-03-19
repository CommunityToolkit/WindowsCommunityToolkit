﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
    /// <summary>
    /// This class converts an object value into a an object (if the value is null returns the false value).
    /// Can be used to bind a visibility, a color or an image to the value of an object.
    /// </summary>
    public class EmptyObjectToObjectConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// Identifies the <see cref="NotEmptyValue"/> property.
        /// </summary>
        public static readonly DependencyProperty NotEmptyValueProperty =
            DependencyProperty.Register(nameof(NotEmptyValue), typeof(object), typeof(EmptyObjectToObjectConverter), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EmptyValue"/> property.
        /// </summary>
        public static readonly DependencyProperty EmptyValueProperty =
            DependencyProperty.Register(nameof(EmptyValue), typeof(object), typeof(EmptyObjectToObjectConverter), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the value to be returned when the object is neither null nor empty
        /// </summary>
        public object NotEmptyValue
        {
            get { return GetValue(NotEmptyValueProperty); }
            set { SetValue(NotEmptyValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to be returned when the object is either null or empty
        /// </summary>
        public object EmptyValue
        {
            get { return GetValue(EmptyValueProperty); }
            set { SetValue(EmptyValueProperty, value); }
        }

        /// <summary>
        /// Convert a boolean value to an other object.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isEmpty = CheckValueIsEmpty(value);

            // Negate if needed
            if (ConverterTools.TryParseBool(parameter))
            {
                isEmpty = !isEmpty;
            }

            return ConverterTools.Convert(isEmpty ? EmptyValue : NotEmptyValue, targetType);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The type of the target property, as a type reference (System.Type for Microsoft .NET, a TypeName helper struct for Visual C++ component extensions (C++/CX)).</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks value for emptiness.
        /// </summary>
        /// <param name="value">Value to be checked.</param>
        /// <returns>True if value is null, false otherwise.</returns>
        protected virtual bool CheckValueIsEmpty(object value)
        {
            return value == null;
        }
    }
}