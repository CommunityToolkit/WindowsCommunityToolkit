// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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