// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Windows.Interop.Sample
{
    using System;
    using System.ComponentModel;
    using System.Windows.Media;
    using Microsoft.Toolkit.Win32.UI.Interop;
    using Microsoft.Toolkit.Win32.UI.Interop.WPF;
    
    public class MyClass
    {
        public MyClass() { }
    }

    public class WrappedRectangle : WindowsXamlHostBase
    {
        public WrappedRectangle() : base()
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Rectangle's HorizontalAlignment and VerticalAlignment properties default to Stretch.
            // The control will fill all space available in the DesktopWindowXamlSource window.
            this.XamlRootInternal = UWPTypeFactory.CreateXamlContentByType("Windows.UI.Xaml.Shapes.Rectangle");

            // Set DesktopWindowXamlSource
            this.desktopWindowXamlSource.Content = this.XamlRootInternal;

            global::Windows.UI.Xaml.Shapes.Rectangle rectangle = this.xamlRoot as global::Windows.UI.Xaml.Shapes.Rectangle;

            // Properties set in markup need to be re-applied in OnInitialized
            Fill = fill;
        }


        private string fill;

        [Category("UWP XAML Rectangle")]
        public string Fill
        {
            set
            {
                fill = value;

                // UWP XAML content is not created in base.OnInitialized
                if (this.xamlRoot != null)
                {
                    global::Windows.UI.Xaml.Shapes.Rectangle rectangle = this.xamlRoot as global::Windows.UI.Xaml.Shapes.Rectangle;

                    Color wpfColor = (Color)ColorConverter.ConvertFromString(value);

                    rectangle.Fill = new global::Windows.UI.Xaml.Media.SolidColorBrush(ConvertWPFColorToUWPColor(wpfColor));
                }

            }
        }

        private global::Windows.UI.Color ConvertWPFColorToUWPColor(Color wpfColor)
        {
            return global::Windows.UI.Color.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
        }
    }
}
