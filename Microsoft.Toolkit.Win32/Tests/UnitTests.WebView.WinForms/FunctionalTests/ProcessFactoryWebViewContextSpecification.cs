// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebViewControlProcess = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlProcess;
using WebViewControlProcessOptions = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlProcessOptions;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests
{
    [TestCategory(TestConstants.Categories.Proc)]
    public abstract class ProcessFactoryWebViewContextSpecification : WebViewFormContextSpecification
    {
        protected WebViewControlProcess WebViewControlProcess { get; private set; }

        protected WebViewControlProcessOptions WebViewControlProcessOptions { get; set; }

        protected override void CreateWebView()
        {
            CreateWebView(new WebViewControlProcessOptions());
        }

        protected virtual void CreateWebView(WebViewControlProcessOptions webViewControlProcessOptions)
        {
            Form.SuspendLayout();
            WebViewControlProcessOptions = webViewControlProcessOptions;
            WebViewControlProcess = new WebViewControlProcess(WebViewControlProcessOptions);
            WebView = (Controls.WinForms.WebView)WebViewControlProcess.CreateWebView(Form.Handle, Form.ClientRectangle);
            Form.Controls.Add(WebView);
            Form.ResumeLayout(false);
            Form.PerformLayout();
        }
    }
}