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
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Windows.Toolkit.UI.Converters
{
    /// <summary>
    /// This class converts a collection size to visibility.
    /// </summary>
    public class CollectionVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// This class return Visibility.Visible if the given collection is not empty or null.
        /// </summary>
        /// <param name="value">Collection to convert to Visibility.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>Visibility.Visible if the collection is not null and not empty</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility result = Visibility.Collapsed;
            IEnumerable<object> collection = value as IEnumerable<object>;

            if (collection != null && collection.Any())
            {
                result = Visibility.Visible;
            }

            if (ConverterTools.SafeParseBool(parameter))
            {
                return ConverterTools.Opposite(result);
            }

            return result;
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
