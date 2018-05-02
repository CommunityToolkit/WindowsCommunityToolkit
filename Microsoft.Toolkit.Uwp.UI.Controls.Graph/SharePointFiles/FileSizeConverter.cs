using System;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    internal class FileSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            long size = (long)value;
            if (size > 1024 * 1024)
                return Math.Round(size / 1024.0 / 1024, 1) + "MB";
            else if (size > 1024 * 2)
                return Math.Round(size / 1024.0, 1) + "KB";
            else if (size == 1)
                return size + " byte";
            else
                return size + " bytes";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
