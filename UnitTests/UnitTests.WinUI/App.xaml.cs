// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using UnitTests.WinUI;

namespace UnitTests
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private static MainWindow _window;

        public static FrameworkElement ContentRoot
        {
            get
            {
                var rootFrame = _window.Content as Frame;
                return rootFrame.Content as FrameworkElement;
            }

            set
            {
                var rootFrame = _window.Content as Frame;
                rootFrame.Content = value;
            }
        }

        // Abstract CoreApplication.MainView.DispatcherQueue
        public static DispatcherQueue DispatcherQueue
        {
            get
            {
                return _window.DispatcherQueue;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Microsoft.VisualStudio.TestPlatform.TestExecutor.UnitTestClient.CreateDefaultUI();

            _window = new MainWindow();

            // Ensure the current window is active
            _window.Activate();

            Logger.LogMessage("Looking for DefaultRichEditBoxStyle...");
            if (!Resources.TryGetValue("DefaultRichEditBoxStyle", out var value))
            {
                Logger.LogMessage("ERROR: Couldn't find DefaultRichEditBoxStyle in WinUI!");
                throw new ApplicationException("Couldn't find DefaultRichEditBoxStyle resource.");
            }
            else
            {
                Logger.LogMessage("FOUND!");
            }

            UITestMethodAttribute.DispatcherQueue = _window.DispatcherQueue;

            // replace back with e.Arguments when https://github.com/microsoft/microsoft-ui-xaml/issues/3368 is fixed
            Microsoft.VisualStudio.TestPlatform.TestExecutor.UnitTestClient.Run(Environment.CommandLine);
        }
    }
}