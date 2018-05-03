using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    internal class DisplayModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is ViewType && parameter != null)
            {
                if (value.ToString() == parameter.ToString())
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
