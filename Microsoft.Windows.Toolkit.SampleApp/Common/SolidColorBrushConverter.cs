using System;

namespace Microsoft.Windows.Toolkit.SampleApp.Common
{
    using global::Windows.UI;
    using global::Windows.UI.Xaml.Data;
    using global::Windows.UI.Xaml.Media;

    public class SolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {
                return value;
            }
            var brush = (SolidColorBrush)value;

            return brush.Color.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return new SolidColorBrush(value.ToString().ToColor());
        }
    }
}
