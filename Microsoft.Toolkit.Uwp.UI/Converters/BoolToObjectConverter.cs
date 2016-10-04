// ******************************************************************
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
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
    /// <summary>
    /// This class converts a boolean value into an other object.
    /// Can be used to convert true/false to visibility, a couple of colors, couple of images, etc.
    /// </summary>
    public class BoolToObjectConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the value to be returned when the boolean is true
        /// </summary>
        public object TrueValue { get; set; }

        /// <summary>
        /// Gets or sets the value to be returned when the boolean is false
        /// </summary>
        public object FalseValue { get; set; }

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
            bool boolValue = value is bool && (bool)value;

            // Negate if needed
            if (ConverterTools.TryParseBool(parameter))
            {
                boolValue = !boolValue;
            }

            return ConverterTools.Convert(boolValue ? TrueValue : FalseValue, targetType);
        }

        /// <summary>
        /// Convert back the value to a boolean
        /// </summary>
        /// <remarks>If the <paramref name="value"/> parameter is a reference type, <see cref="TrueValue"/> must match its reference to return true.</remarks>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The type of the target property, as a type reference (System.Type for Microsoft .NET, a TypeName helper struct for Visual C++ component extensions (C++/CX)).</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool result = Equals(value, ConverterTools.Convert(TrueValue, targetType));

            if (ConverterTools.TryParseBool(parameter))
            {
                result = !result;
            }

            return result;
        }
    }
}
