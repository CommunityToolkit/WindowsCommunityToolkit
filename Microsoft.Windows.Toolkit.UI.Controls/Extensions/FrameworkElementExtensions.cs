using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace Microsoft.Windows.Toolkit.UI.Controls.Extensions
{
    public static class FrameworkElementExtensions
    {
        public static double GetTranslateX(this FrameworkElement elem)
        {
            return elem.GetCompositeTransform().TranslateX;
        }

        public static double GetTranslateY(this FrameworkElement elem)
        {
            return elem.GetCompositeTransform().TranslateY;
        }

        public static void TranslateX(this FrameworkElement elem, double x)
        {
            elem.GetCompositeTransform().TranslateX = x;
        }

        public static void TranslateY(this FrameworkElement elem, double y)
        {
            elem.GetCompositeTransform().TranslateY = y;
        }

        public static void TranslateDeltaX(this FrameworkElement elem, double x)
        {
            elem.GetCompositeTransform().TranslateX += x;
        }

        public static void TranslateDeltaY(this FrameworkElement elem, double y)
        {
            elem.GetCompositeTransform().TranslateY += y;
        }

        public static CompositeTransform GetCompositeTransform(this FrameworkElement elem)
        {
            var trans = elem.RenderTransform as CompositeTransform;
            if (trans == null)
            {
                trans = new CompositeTransform();
                elem.RenderTransform = trans;
            }
            return trans;
        }
    }
}