using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    internal static class InternalConversionExtensions
    {
        public static Rect ToWpf(this global::Windows.Foundation.Rect uwp)
        {
            return new Rect(uwp.X, uwp.Y, uwp.Width, uwp.Height);
        }

        public static global::Windows.Foundation.Point ToUwp(this Point wpf)
        {
            return new global::Windows.Foundation.Point(wpf.X, wpf.Y);
        }
    }
}
