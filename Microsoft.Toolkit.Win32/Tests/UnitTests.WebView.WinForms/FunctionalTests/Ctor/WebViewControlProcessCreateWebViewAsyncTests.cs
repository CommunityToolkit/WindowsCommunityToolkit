// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Ctor
{
    [TestClass]
    [TestCategory(TestConstants.Categories.Init)]
    public class WebViewControlProcessCreateWebViewAsync : ProcessFactoryWebViewContextSpecification
    {
        [TestMethod]
        public void ProcessIsNotNull()
        {
            WebViewControlProcess.ShouldNotBeNull();
        }

        [TestMethod]
        public void WebViewIsNotNull()
        {
            WebView.ShouldNotBeNull();
        }

        [TestMethod]
        public void WebViewProcessIsNotNull()
        {
            WebView.Process.ShouldNotBeNull();
        }
    }

    [TestClass]
    [TestCategory(TestConstants.Categories.Init)]
    [Ignore("Unstable. The remote procedure call failed. (Exception from HRESULT: 0x800706BE).")]
    public class MultipleWebViewsFromOneProcess : ProcessFactoryWebViewContextSpecification
    {
        private UI.Controls.WinForms.WebView _webView2;

        protected override void When()
        {
            _webView2 = new UI.Controls.WinForms.WebView(WebViewControlProcess.CreateWebViewControlHost(Form.Handle, Form.ClientRectangle));
        }

        [TestMethod]
        public void CanCreateSecondWebView()
        {
            _webView2.ShouldNotBeNull();
        }

        // TODO: Assert they have the same PID
        [TestMethod]
        public void BothWebViewHaveSameProcessId()
        {
            _webView2.Process.ProcessId.ShouldEqual(WebView.Process.ProcessId, "Expected same PID");
        }
    }
}
