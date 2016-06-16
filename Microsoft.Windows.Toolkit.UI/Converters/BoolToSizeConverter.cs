// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************
using System;
using Windows.UI.Xaml.Data;


namespace Microsoft.Windows.Toolkit.UI.Converters
{
    /// <summary>
    /// This class converts a string value containing a boolean into a Size for XAML property (Width, MaxWidth, Heihgt, MaxHeight).
    /// True becomes NaN (Auto) and false becomes 0.
    /// </summary>
    public class BoolToSizeConverter : IValueConverter
    {
        /// <summary>
        /// Convert a string value containing a boolean into a Size (Width, MaxWidth, Heihgt, MaxHeight).
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">If parameter is defined the operation is inverted (False becomes Nan and true becomes 0).</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>Return NaN (for Auto) or 0.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return 0;
            }

            bool boolInput;
            if (!bool.TryParse(value.ToString(), out boolInput))
            {
                return 0;
            }

            if (ConverterTools.SafeParseBool(parameter))
            {
                return boolInput ? double.NaN : 0;
            }

            return boolInput ? 0 : double.NaN;
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
    }
}
