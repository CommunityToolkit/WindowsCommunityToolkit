// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Navigation
{
    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class StopAfterNavigate : HostFormWebViewContextSpecification
    {
        private System.Windows.Forms.Timer _timer;
        private bool _navCompleted;
        protected override void Given()
        {
            _timer = new System.Windows.Forms.Timer
            {
                Interval = TestConstants.Timeouts.Shorter
            };
            _timer.Tick += (o, e) => { Form.Close(); };

            base.Given();
            WebView.NavigationCompleted += (o, e) => { _navCompleted = true; };
        }

        protected override void When()
        {
            PerformActionAndWaitForFormClose(() =>
            {
                WebView.Navigate(TestConstants.Uris.ExampleOrg);
                _timer.Start();
                WebView.Stop();
            });
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        [Ignore("Causing test run to abort")]
        public void StopAfterNavigateDoesNotCompleteNavigation()
        {
            _navCompleted.ShouldBeFalse();
        }
    }

    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class NavigationCancelOnNavigationStarting : HostFormWebViewContextSpecification
    {
        private System.Windows.Forms.Timer _timer;
        private bool _navCompleted;

        protected override void Given()
        {
            _timer = new System.Windows.Forms.Timer
            {
                Interval = TestConstants.Timeouts.Shorter
            };
            _timer.Tick += (o, e) => { Form.Close(); };

            base.Given();
            WebView.NavigationStarting += (o, e) => { e.Cancel = true; };
            WebView.NavigationCompleted += (o, e) => { _navCompleted = true; };
        }

        protected override void When()
        {
            PerformActionAndWaitForFormClose(()=>
            {
                WebView.Navigate(TestConstants.Uris.ExampleOrg);
                _timer.Start();
            });
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        public void NavigationCompletedEventNeverFired()
        {
            _navCompleted.ShouldBeFalse();
        }
    }
}
