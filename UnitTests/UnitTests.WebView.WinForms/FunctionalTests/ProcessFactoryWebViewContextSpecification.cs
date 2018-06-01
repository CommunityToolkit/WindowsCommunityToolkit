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