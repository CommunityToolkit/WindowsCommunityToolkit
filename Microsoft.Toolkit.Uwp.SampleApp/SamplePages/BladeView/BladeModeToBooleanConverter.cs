using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public class BladeModeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var bladeMode = (BladeMode)value;
            return (bladeMode == BladeMode.Fullscreen) ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool bValue = (bool)value;
            return bValue ? BladeMode.Fullscreen : BladeMode.Normal;
        }
    }
}
