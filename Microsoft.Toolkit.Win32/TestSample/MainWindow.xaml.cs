using Microsoft.Windows.Interop;
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
            //inkToolbar.NativeActiveToolChanged += InkToolbar_NativeActiveToolChanged;
        }

        private void inkToolbar_ActiveToolChanged(object sender, object e)
        {

        }

        private void inkToolbar_InkDrawingAttributesChanged(object sender, object e)
        {

        }

        private void inkToolbar_IsStencilButtonCheckedChanged(object sender, Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarIsStencilButtonCheckedChangedEventArgs e)
        {

        }
    }

}
