using System.Drawing;
using Windows.Foundation;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    public static class WebViewControlHostExtensions
    {
        internal static void UpdateBounds(this WebViewControlHost host, Rectangle bounds)
        {
            Rect CreateBounds()
            {
                return new Rect(
                    new Windows.Foundation.Point(bounds.X, bounds.Y),
                    new Windows.Foundation.Size(bounds.Width, bounds.Height));
            }

            host.UpdateBounds(CreateBounds());
        }
    }
}