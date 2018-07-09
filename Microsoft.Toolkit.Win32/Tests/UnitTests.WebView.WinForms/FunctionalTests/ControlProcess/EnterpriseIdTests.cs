// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Interop;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.ControlProcess
{
    [TestClass]
    public class EnterpriseIdTests : ProcessFactoryWebViewContextSpecification
    {
        protected override void Given()
        {
            // TODO: Does Windows Information Protection work for WinForms?
            OSVersionHelper.UseWindowsInformationProtectionApi.ShouldBeTrue("The operating system does not support Windows Information Protection (WIP) or WIP is not enabled on this device.");

            var o = new WebViewControlProcessOptions() {EnterpriseId = nameof(EnterpriseIdTests)};
            CreateWebView(o);
        }

        [TestMethod]
        public void Unknown()
        {
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Inconclusive("No implemented behavior for enlightened LOB using WebView.");
        }
    }
}
