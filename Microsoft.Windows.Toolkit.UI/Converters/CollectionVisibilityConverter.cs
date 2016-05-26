using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Windows.Toolkit.UI.Converters
{
    /// <summary>
    /// 
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
        /// <returns></returns>
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
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
