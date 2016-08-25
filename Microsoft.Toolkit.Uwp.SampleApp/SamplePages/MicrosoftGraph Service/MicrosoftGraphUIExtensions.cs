using System;
using System.Collections.Generic;
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
    }
}
