// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Windows.Media;
using Microsoft.Toolkit.Win32.UI.XamlHost;
using Microsoft.Toolkit.Wpf.UI.XamlHost;

namespace Microsoft.Toolkit.Sample.Wpf.XamlHost
{
    public class WrappedButton : WindowsXamlHostBase
    {

        public WrappedButton() : base()
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.ChildInternal = UWPTypeFactory.CreateXamlContentByType("Windows.UI.Xaml.Controls.Button");

            // Make button expand to the size of its host control
            global::Windows.UI.Xaml.FrameworkElement frameworkElement = this.ChildInternal as global::Windows.UI.Xaml.FrameworkElement;
            frameworkElement.SizeChanged += FrameworkElement_SizeChanged;
            frameworkElement.HorizontalAlignment = global::Windows.UI.Xaml.HorizontalAlignment.Stretch;
            frameworkElement.VerticalAlignment = global::Windows.UI.Xaml.VerticalAlignment.Stretch;

            // Set DesktopWindowXamlSource
            SetContent();

            // Properties set in markup need to be re-applied in OnInitialized.
            Background = background;
            Content = content;
        }

        private void FrameworkElement_SizeChanged(object sender, global::Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

        private string background;

        [Category("UWP XAML Button")]
        public string Background
        {
            set
            {
                background = value;

                // UWP XAML content is not created until base.OnInitialized
                if (value != null && this.ChildInternal != null)
                {
                    global::Windows.UI.Xaml.Controls.Button button = this.ChildInternal as global::Windows.UI.Xaml.Controls.Button;

                    Color wpfColor = (Color)ColorConverter.ConvertFromString(value);

                    button.Background = new global::Windows.UI.Xaml.Media.SolidColorBrush(ConvertWPFColorToUWPColor(wpfColor));
                }

            }
        }

        [Category("UWP XAML Button")]
        string content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;

                // UWP XAML content is not created until base.OnInitialized
                if (this.ChildInternal != null)
                {
                    global::Windows.UI.Xaml.Controls.Button button = this.ChildInternal as global::Windows.UI.Xaml.Controls.Button;

                    button.Content = value;
                }
            }
        }

        private global::Windows.UI.Color ConvertWPFColorToUWPColor(Color wpfColor)
        {
            return global::Windows.UI.Color.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
        }

    }
}
