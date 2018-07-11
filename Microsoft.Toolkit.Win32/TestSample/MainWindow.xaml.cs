using Microsoft.Windows.Interop;
using System;
using System.Windows;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

namespace TestSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly Geopoint SeattleGeopoint = new Geopoint(new BasicGeoposition() { Latitude = 47.604, Longitude = -122.329 });

        public MainWindow()
        {
            InitializeComponent();
        }

        private void inkCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen | Windows.UI.Core.CoreInputDeviceTypes.Touch;
        }

        private void inkToolbar_Initialized(object sender, EventArgs e)
        {
            
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

        private void WebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void myMap_Loaded(object sender, RoutedEventArgs e)
        {
            myMap.Center = SeattleGeopoint;
            myMap.ZoomLevel = 12;
            myMap.Style = MapStyle.Road;
            myMap.MapProjection = MapProjection.Globe;
            
        }
    }

}
