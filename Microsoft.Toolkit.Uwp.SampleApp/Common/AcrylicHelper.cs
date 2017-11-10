using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public static class AcrylicHelper
    {
        public static Color GetColor(FrameworkElement obj)
        {
            return (Color)obj.GetValue(ColorProperty);
        }

        public static void SetColor(FrameworkElement obj, Color value)
        {
            obj.SetValue(ColorProperty, value);
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.RegisterAttached("Color", typeof(Color), typeof(AcrylicHelper), new PropertyMetadata(Colors.White, OnColorChanged));

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Color color)
            {
                var type = d.GetType();
                var backgroundProperty = type.GetProperty("Background");
                if (backgroundProperty == null)
                {
                    return;
                }

                var fullColor = Color.FromArgb(0xff, color.R, color.G, color.B);

                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.AcrylicBrush"))
                {
                    AcrylicBrush myBrush = new AcrylicBrush
                    {
                        BackgroundSource = AcrylicBackgroundSource.Backdrop,
                        TintColor = fullColor,
                        FallbackColor = color,
                        TintOpacity = (double)color.A / 255d
                    };

                    backgroundProperty.SetValue(d, myBrush);
                }
                else
                {
                    backgroundProperty.SetValue(d, new SolidColorBrush(color));
                    ((FrameworkElement)d).Blur(3, duration: 0).Start();
                }
            }
        }
    }
}
