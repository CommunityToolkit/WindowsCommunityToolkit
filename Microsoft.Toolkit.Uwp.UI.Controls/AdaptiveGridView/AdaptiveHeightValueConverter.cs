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

                var margin = GetItemMargin(gridView);
                height = height + margin.Top + margin.Bottom;

                return height;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private static Thickness GetItemMargin(GridView view)
        {
            var setter = view.ItemContainerStyle?.Setters.OfType<Setter>().FirstOrDefault(s => s.Property == FrameworkElement.MarginProperty);
            if (setter != null)
            {
                return (Thickness)setter.Value;
            }
            else
            {
                if (view.Items.Count > 0)
                {
                    var container = (GridViewItem)view.ContainerFromIndex(0);
                    if (container != null)
                    {
                        return container.Margin;
                    }
                }

                // Use the default thickness for a GridViewItem
                return new Thickness(0, 0, 4, 4);
            }
        }
    }
}
