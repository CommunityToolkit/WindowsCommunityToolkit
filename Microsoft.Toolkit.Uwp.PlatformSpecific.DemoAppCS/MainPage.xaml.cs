using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Services.Maps;
using Windows.UI.Xaml.Documents;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Microsoft.Toolkit.Uwp.PlatformSpecific.DemoAppCS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void LoadCoreInput()
        {
            PlaceInfo placeInfo1 = PlaceInfo.CreateFromAddress("11 Glover Close, Cheshunt");
            PlaceInfo placeInfo2 = PlaceInfo.CreateFromAddress("11 Glover Close, Cheshunt", "Home");
        }

        private void LoadTextTypes()
        {
            ContentLink contentLink = new ContentLink();
            contentLink.IsTextScaleFactorEnabled = true;
        }
    }
}
