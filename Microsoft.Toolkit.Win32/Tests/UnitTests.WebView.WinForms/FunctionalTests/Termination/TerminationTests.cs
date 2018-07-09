// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Should;

using System.ComponentModel;
using System.Threading;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Termination
{
    [TestClass]
    public class TerminationTests : BlockTestStartEndContextSpecification
    {
        private bool _processExitedEventFired;
        private ManualResetEvent _mre;

        protected override void Given()
        {
            // Perform check to see if we can run before we get too far
            Assert.That.OSBuildShouldBeAtLeast(TestConstants.Windows10Builds.InsiderFast17650);

            base.Given();

            _mre = new ManualResetEvent(false);
        }

        protected override void CreateWebView()
        {
            WebView = new UI.Controls.WinForms.WebView();
            ((ISupportInitialize)WebView).BeginInit();
            ((ISupportInitialize)WebView).EndInit();

            WebView.ShouldNotBeNull();
            WebView.Process.ShouldNotBeNull();
            WebView.Process.ProcessId.ShouldNotEqual(0U);

            WebView.Process.ProcessExited += (o, e) =>
            {
                _processExitedEventFired = true;
                _mre.Set();
            };
        }

        protected override void When()
        {
            WebView.Process.Terminate();
        }

        [TestMethod]
        public void TheTerminatedProcessShouldNoLongerHaveAProcessId()
        {
            WebView.Process.ProcessId.ShouldEqual(0U);
        }

        [TestMethod]
        public void TheProcessExitedEventWasRaised()
        {
            _mre.WaitOne(TestConstants.Timeouts.Short);
            _processExitedEventFired.ShouldBeTrue();
        }
    }
}