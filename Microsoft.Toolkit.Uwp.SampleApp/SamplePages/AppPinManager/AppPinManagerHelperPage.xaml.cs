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
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.ApplicationModel;
using Windows.System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppPinManagerHelperPage : Page
    {
        public AppPinManagerHelperPage()
        {
            this.InitializeComponent();
        }

        private async void SpecificAppSMenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            var appList = (await Package.Current.GetAppListEntriesAsync())[0];
            var pinResult = await AppPinManager.PinSpecificAppToStartMenuAsync(appList);
            StatusMessage.Text = "SpecificApp in StartMenu : " + pinResult.ToString();
        }

        private async void UserSpecificAppSMenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            var userInfo = await User.FindAllAsync();
            if (userInfo.Count > 0)
            {
                var appList = (await Package.Current.GetAppListEntriesAsync())[0];
                var pinResult = await AppPinManager.PinUserSpecificAppToStartMenuAsync(userInfo[0], appList);
                StatusMessage.Text = "User SpecificApp in StartMenu : " + pinResult.ToString();
            }
        }

        private async void CurrentAppTBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            var pinResult = await AppPinManager.PinCurrentAppToTaskBarAsync();
            StatusMessage.Text = "Current App in TaskBar : " + pinResult.ToString();
        }

        private async void SpecificAppTBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            var appList = (await Package.Current.GetAppListEntriesAsync())[0];
            var pinResult = await AppPinManager.PinSpecificAppToTaskBarAsync(appList);
            StatusMessage.Text = "Specific App in TaskBar : " + pinResult.ToString();
        }
    }
}
