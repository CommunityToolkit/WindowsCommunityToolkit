using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BackgroundTaskHelperPage : Page
    {
        public BackgroundTaskHelperPage()
        {
            this.InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (BackgroundTaskHelper.IsBackgroundTaskRegistered("TestBackgroundTaskName"))
            {
                // Background task already registered.
                StatusMessage.Text = "Background Task already registered";
                return;
            }

            // Check for background access.
            await BackgroundExecutionManager.RequestAccessAsync();

            BackgroundTaskHelper.Register("TestBackgroundTaskName", new TimeTrigger(15, false));

            // If registering Multi-Process Background task
            // BackgroundTaskHelper.Register("TestName", "TestEntryPoint", new TimeTrigger(15, false), false, true, new SystemCondition(SystemConditionType.InternetAvailable));
            StatusMessage.Text = "Background Task registered";
        }

        private void UnregisterButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundTaskHelper.Unregister("TestBackgroundTaskName");

            StatusMessage.Text = "Background Task Unregistered";
        }
    }
}
