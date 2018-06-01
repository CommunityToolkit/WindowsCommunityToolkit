// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Reflection;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public static class XAMLHelper
    {
        public static Color GetBackground(FrameworkElement obj)
        {
            return (Color)obj.GetValue(BackgroundProperty);
        }

        public static void SetBackground(FrameworkElement obj, Color value)
        {
            obj.SetValue(BackgroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(Color), typeof(XAMLHelper), new PropertyMetadata(Colors.White, OnBackgroundChanged));

        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
