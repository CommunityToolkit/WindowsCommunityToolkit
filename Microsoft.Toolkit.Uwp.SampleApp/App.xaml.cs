// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Microsoft.Toolkit.Uwp.SampleApp.SamplePages;
using Microsoft.Toolkit.Uwp.SampleApp.Styles;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.System.Profile;
using Windows.UI.ViewManagement;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public sealed partial class App : Application
    {
        private MainWindow _window;

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await RunAppInitialization(null);

            if (args.Kind == ActivationKind.Protocol)
            {
                try
                {
                    // Launching via protocol link
                    var parser = DeepLinkParser.Create(args);
                    var targetSample = await Sample.FindAsync(parser.Root, parser["sample"]);
                    if (targetSample != null)
                    {
                        Shell.Current?.NavigateToSample(targetSample);
                    }
                }
                catch (Exception ex)
                {
                    global::System.Diagnostics.Debug.WriteLine($"Error processing protocol launch: {ex.ToString()}");
                }
            }
        }

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);

            IActivatedEventArgs activatedArgs = AppInstance.GetActivatedEventArgs();

            if (activatedArgs.PreviousExecutionState != ApplicationExecutionState.Running
                && activatedArgs.PreviousExecutionState != ApplicationExecutionState.Suspended)
            {
                await RunAppInitialization(e?.Arguments);
            }

            SystemInformation.Instance.TrackAppUse(activatedArgs);
        }

        /// <summary>
        /// Event fired when a Background Task is activated (in Single Process Model)
        /// </summary>
        /// <param name="args">Arguments that describe the BackgroundTask activated</param>
        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);

            var deferral = args.TaskInstance.GetDeferral();

            switch (args.TaskInstance.Task.Name)
            {
                case Constants.TestBackgroundTaskName:
                    new TestBackgroundTask().Run(args.TaskInstance);
                    break;
            }

            deferral.Complete();
        }

        private async global::System.Threading.Tasks.Task RunAppInitialization(string launchParameters)
        {
            ThemeInjector.InjectThemeResources(Application.Current.Resources);

            // Go fullscreen on Xbox
            if (AnalyticsInfo.VersionInfo.GetDeviceFormFactor() == DeviceFormFactor.Xbox)
            {
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            }

            // Initialize the constant for the app display name, used for tile and toast previews
            if (Constants.ApplicationDisplayName == null)
            {
                // Constants.ApplicationDisplayName = (await Package.Current.GetAppListEntriesAsync())[0].DisplayInfo.DisplayName;
            }

            // Check if the Cache is Latest, wipe if not.
            Sample.EnsureCacheLatest();

            _window = new MainWindow(launchParameters);

            _window.Activate();
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
        }
    }
}