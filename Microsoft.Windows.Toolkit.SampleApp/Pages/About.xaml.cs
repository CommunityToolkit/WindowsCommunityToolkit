using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Windows.Toolkit.SampleApp.Pages
{
    public sealed partial class About
    {
        public About()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.ShowOnlyHeader("About");

            var packageVersion = Package.Current.Id.Version;
            Version.Text = $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}";
        }
    }
}
