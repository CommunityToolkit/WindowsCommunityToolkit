using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Microsoft.Windows.Toolkit.UI.Converters
{
    /// <summary>
    /// This class provides a binding converter to display formatted strings
    /// </summary>
    public class StringFormatConverter : IValueConverter
    {
        /// <summary>
        /// Return the formatted string version of the source object.
        /// </summary>
        /// <param name="value">Object to transform to string.</param>
        /// <param name="targetType">The type of the target property, as a type reference</param>
        /// <param name="parameter">An optional parameter to be used in the string.Format method.</param>
        /// <param name="language">The language of the conversion (not used).</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            if (parameter == null)
                return value;

            return string.Format((string)parameter, value);
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter,
            string language)
        {
            throw new NotImplementedException();
        }
    }
}
