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

using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests
{
    [TestCategory(TestConstants.Categories.Des)]
    public abstract class HostFormWebViewContextSpecification : WebViewFormContextSpecification
    {
        protected override void CreateWebView()
        {
            // This is what Windows Forms designer emits
            WebView = new UI.Controls.WinForms.WebView();
            ((ISupportInitialize)WebView).BeginInit();
            Form.SuspendLayout();
            WebView.Dock = DockStyle.Fill;
            WebView.Size = Form.ClientSize;
            WebView.IsScriptNotifyAllowed = true;
            WebView.IsIndexedDBEnabled = true;
            WebView.IsJavaScriptEnabled = true;
            WebView.Visible = true;
            Form.Controls.Add(WebView);
            ((ISupportInitialize)WebView).EndInit();
            Form.ResumeLayout(false);
            Form.PerformLayout();

            WebView.IsScriptNotifyAllowed.ShouldBeTrue();
            WebView.IsIndexedDBEnabled.ShouldBeTrue();
            WebView.IsJavaScriptEnabled.ShouldBeTrue();
        }
    }
}