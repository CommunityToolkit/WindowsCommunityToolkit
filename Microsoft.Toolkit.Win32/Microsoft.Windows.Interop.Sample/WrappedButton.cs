namespace Microsoft.Windows.Interop.Sample
{
    using System;
    using System.ComponentModel;
    using System.Windows.Media;
    using Microsoft.Toolkit.Win32.UI.Interop.WPF;


    public class WrappedButton : WindowsXamlHostBase
    {

        #region Constructors and Initialization
        public WrappedButton() : base()
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.XamlRootInternal = UWPTypeFactory.CreateXamlContentByType("Windows.UI.Xaml.Controls.Button");

            // Make button expand to the size of its host control
            global::Windows.UI.Xaml.FrameworkElement frameworkElement = this.xamlRoot as global::Windows.UI.Xaml.FrameworkElement;
            frameworkElement.SizeChanged += FrameworkElement_SizeChanged;
            frameworkElement.HorizontalAlignment = global::Windows.UI.Xaml.HorizontalAlignment.Stretch;
            frameworkElement.VerticalAlignment = global::Windows.UI.Xaml.VerticalAlignment.Stretch;

            // Set DesktopWindowXamlSource
            this.desktopWindowXamlSource.Content = this.XamlRootInternal;


            // Properties set in markup need to be re-applied in OnInitialized.  
            Background = background;
            Content = content;
        }

        private void FrameworkElement_SizeChanged(object sender, global::Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

        #endregion

        #region Properties 

        private string background;

        [Category("UWP XAML Button")]
        public string Background
        {
            set
            {
                background = value;

                // UWP XAML content is not created until base.OnInitialized
                if (value != null && this.xamlRoot != null)
                {
                    global::Windows.UI.Xaml.Controls.Button button = this.xamlRoot as global::Windows.UI.Xaml.Controls.Button;

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
                if (this.xamlRoot != null)
                {
                    global::Windows.UI.Xaml.Controls.Button button = this.xamlRoot as global::Windows.UI.Xaml.Controls.Button;

                    button.Content = value;
                }
            }
        }

        #endregion

        #region Helpers
        private global::Windows.UI.Color ConvertWPFColorToUWPColor(Color wpfColor)
        {
            return global::Windows.UI.Color.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
        }

        #endregion
    }
}
