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

        public static IReadOnlyList<InkRecognitionResult> ToWpf(this IReadOnlyList<global::Windows.UI.Input.Inking.InkRecognitionResult> uwp)
        {
            return uwp.Cast<InkRecognitionResult>().ToList();
        }

        public static IReadOnlyList<global::Windows.UI.Input.Inking.InkRecognitionResult> ToUwp(this IReadOnlyList<InkRecognitionResult> wpf)
        {
            return wpf.Cast<global::Windows.UI.Input.Inking.InkRecognitionResult>().ToList();
        }
    }
}
