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
using Microsoft.Toolkit.Uwp.Helpers;
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

        private bool IsBackgroundTaskRegistered(string taskName)
        {
            if (BackgroundTaskHelper.IsBackgroundTaskRegistered(taskName))
            {
                // Background task already registered.
                StatusMessage.Text = "Background Task already registered";
                return true;
            }

            return false;
        }

        private async void RegisterMpmButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsBackgroundTaskRegistered(nameof(TestBackgroundTask)))
            {
                return;
            }

            // Check for background access.
            await BackgroundExecutionManager.RequestAccessAsync();

            // Registering Multi-Process Background task
            BackgroundTaskHelper.Register(nameof(TestBackgroundTask), "Microsoft.Toolkit.Uwp.Samples.BackgroundTasks.TestBackgroundTask", new TimeTrigger(15, false), false, true, new SystemCondition(SystemConditionType.InternetAvailable));

            StatusMessage.Text = "Background Task registered (MPM)";
        }

        private void UnregisterMpmButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundTaskHelper.Unregister(nameof(TestBackgroundTask));
            StatusMessage.Text = "Background Task Unregistered";
        }

        private async void RegisterSpmButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsBackgroundTaskRegistered(Constants.TestBackgroundTaskName))
            {
                return;
            }

            // Check for background access.
            await BackgroundExecutionManager.RequestAccessAsync();

            // Registering Single-Process Background task
            BackgroundTaskHelper.Register(Constants.TestBackgroundTaskName, new TimeTrigger(15, false));

            StatusMessage.Text = "Background Task registered (SPM)";
        }

        private void UnregisterSpmButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundTaskHelper.Unregister(Constants.TestBackgroundTaskName);
            StatusMessage.Text = "Background Task Unregistered";
        }
    }
}
