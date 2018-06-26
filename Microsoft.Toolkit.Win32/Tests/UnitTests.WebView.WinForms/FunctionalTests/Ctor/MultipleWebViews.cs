// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Ctor
{
    [TestClass]
    public class MultipleWebViewsTests : HostFormWebViewContextSpecification
    {
        private Controls.WinForms.WebView WebView2 { get; set; }

        protected override void Given()
        {
            // Perform check to see if we can run before we get too far
            Assert.That.OSBuildShouldBeAtLeast(TestConstants.Windows10Builds.InsiderFast17650);

            base.Given();
        }

        protected override void When()
        {
            WebView2 = (Controls.WinForms.WebView)WebView.Process.CreateWebView(Form.Handle, Rectangle.Empty);
        }

        [TestMethod]
        public void SecondWebViewIsCreated()
        {
            WebView2.ShouldNotBeNull();
        }

        [TestMethod]
        public void SecondWebViewInSameProcess()
        {
            WebView2.Process.ProcessId.ShouldEqual(WebView.Process.ProcessId);
        }

        protected override void Cleanup()
        {
            WebView2?.Dispose();
            WebView2 = null;

            base.Cleanup();
        }
    }
}
