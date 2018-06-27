using System;
using System.Windows;
using System.Windows.Input;
using Windows.Media.Core;
using Windows.UI.Xaml.Controls;

namespace TestSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

        }

        private void inkCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.InputDeviceTypes =
                Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                Windows.UI.Core.CoreInputDeviceTypes.Pen | Windows.UI.Core.CoreInputDeviceTypes.Touch;
        }




        private void inkToolbar_Initialized(object sender, EventArgs e)
        {
            toolButtonLasso.Content = new SymbolIcon(Symbol.SelectAll);
            ToolTipService.SetToolTip(toolButtonLasso.XamlRoot, "Select All");
        }

        private void WebView1_ContainsFullScreenElementChanged(object sender, Microsoft.Windows.Interop.DynamicForwardedEventArgs e)
        {
            //WebView1.NavigationCompleted += WebView1_NavigationCompleted1;
        }

        private void WebView1_NavigationCompleted1(object sender, Microsoft.Windows.Interop.DynamicForwardedEventArgs e)
        {
            
        }

        private void WebView1_NavigationCompleted(object sender, Microsoft.Windows.Interop.DynamicForwardedEventArgs e)
        {

        }

        private void WebView1_NavigationStarting(object sender, Microsoft.Windows.Interop.DynamicForwardedEventArgs e)
        {

        }

        private void WebView1_PermissionRequested(object sender, Microsoft.Windows.Interop.DynamicForwardedEventArgs e)
        {

        }

        private void WebView1_ScriptNotify(object sender, Microsoft.Windows.Interop.DynamicForwardedEventArgs e)
        {

        }
    }

}
