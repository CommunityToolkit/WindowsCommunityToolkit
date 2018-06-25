// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WPF.WebView.FunctionalTests
{
    [TestClass]
    public class Class1 : ContextSpecification
    {
        private Controls.WPF.WebView _webView;
        private Window _window;

        protected override void Given()
        {
            _window = new Window
            {
                Title = TestContext.TestName,
                Width = 1000,
                Height = 800
            };

            _window.Loaded += (o, e) => { WriteLine("Window.Loaded"); };
            _window.MouseEnter += (o, e) => { WriteLine("Window.MouseEnter"); };
            _window.GotFocus += (o, e) => { WriteLine("Window.GotFocus"); };
            _window.LostFocus += (o, e) => { WriteLine("Window.LostFocus"); };
            _window.Closing += (o, e) => { WriteLine("Window.Closing"); };
            _window.Closed += (o, e) => { WriteLine("Window.Closed"); };


            _webView = new Controls.WPF.WebView
            {
                Name = "WebView1",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = _window.Height,
                Width = _window.Width,
                MinHeight = 200,
                MinWidth = 200
            };
            var dp = new DockPanel();
            dp.Children.Add(_webView);
            _window.Content = dp;

            _webView.NavigationCompleted += (o, e) =>
            {
                _window.Close();
            };
        }

        protected override void When()
        {
            _window.Loaded += (o, e) =>
            {
                _webView.Navigate(TestConstants.Uris.ExampleCom);
            };



            ShowModalWindow(_window);
            //_window.Show();
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Short)]
        [Ignore]
        public void Foo()
        {
            _webView.Process.ShouldNotBeNull();
        }

        protected override void Cleanup()
        {
            Dispatcher.CurrentDispatcher.InvokeShutdown();

            _window.BringIntoView();
            _window.Close();

            _webView.Dispose();
            _webView = null;
        }

        private bool? ShowModalWindow(Window window)
        {
            // check if we are in the right thread
            if (!window.Dispatcher.CheckAccess())
            {
                // if not, initialize the delegate and invoke the right thread
                ShowModalWindowCallback callback = ShowModalWindow;
                return (bool?)window.Dispatcher.Invoke(callback, DispatcherPriority.Normal, window);
            }

            // now we are in the right thread, show modal window
            return window.ShowDialog();
        }

        private delegate bool? ShowModalWindowCallback(Window window);


    }
}
