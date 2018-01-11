using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class AdaptiveHeightValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                var gridView = parameter as AdaptiveGridView;
                if (gridView == null)
                {
                    return value;
                }

                double.TryParse(value.ToString(), out double height);

                var setter = gridView.ItemContainerStyle?.Setters.OfType<Setter>().FirstOrDefault(s => s.Property == FrameworkElement.MarginProperty);
                if (setter != null)
                {
                    var margin = (Thickness)setter.Value;
                    height = height + margin.Top + margin.Bottom;
                }

                return height;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
