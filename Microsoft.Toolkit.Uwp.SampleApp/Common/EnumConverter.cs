using System;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.SampleApp.Common
{
    public class EnumConverter : IValueConverter
    {
        private readonly Type _enumType;

        public EnumConverter(Type enumType)
        {
            _enumType = enumType;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Enum.Parse(_enumType, value.ToString());
        }
    }
}
