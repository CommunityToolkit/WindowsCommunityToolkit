// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class BackgroundTaskHelperPage : Page
    {
        public BackgroundTaskHelperPage()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (BackgroundTaskHelper.IsBackgroundTaskRegistered(Constants.TestBackgroundTaskName))
            {
                // Background task already registered.
                StatusMessage.Text = "Background Task already registered";
                return;
            }

            // Check for background access.
            await BackgroundExecutionManager.RequestAccessAsync();

            BackgroundTaskHelper.Register(Constants.TestBackgroundTaskName, new TimeTrigger(15, false));

            // If registering Multi-Process Background task
            // BackgroundTaskHelper.Register(testBackgroundTaskName, "Microsoft.Toolkit.Uwp.SampleApp.SamplePages", new TimeTrigger(15, false), false, true, new SystemCondition(SystemConditionType.InternetAvailable));
            StatusMessage.Text = "Background Task registered";
        }

        private void UnregisterButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundTaskHelper.Unregister(Constants.TestBackgroundTaskName);

            StatusMessage.Text = "Background Task Unregistered";
        }
    }
}
