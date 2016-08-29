using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{

    public static class MicrosoftGraphUIExtensions
    {
        public static bool IsVisible(this FrameworkElement element)
        {
            return element.Visibility == Visibility.Visible ? true : false;
        }

        public static void AddTo<T>(this ObservableCollection<T> itemsSource, ObservableCollection<T> itemsDest)
        {
            foreach (var item in itemsSource)
            {
                itemsDest.Add(item);
            }
        }
    }
}
