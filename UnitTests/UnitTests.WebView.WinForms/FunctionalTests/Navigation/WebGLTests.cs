// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Navigation
{
    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class CanCreateWebGL : HostFormWebViewContextSpecification
    {
        // Execute some script that creates a WebGLRenderingContext. There is code that
        // will prevent the underlying backend (edgeangle.dll) from loading in processes
        // that are not part of a package. The process that hosts edgehtml.dll (and thus
        // edgeangle.dll) in Win32WebView scenarios should be packaged so the below should
        // succeed and not crash.
        private string _content = @"
<h1>TEST</h1>
<script>
  document.createElement('canvas').getContext('webgl');
</script>
";

        protected override void Given()
        {
            base.Given();

            WebView.NavigationCompleted += (o, e) =>
            {
                Form.Close();
            };
        }

        protected override void When()
        {
            WebView.NavigateToString(_content);
        }

        [TestMethod]
        public void WebGLContextCreated()
        {
        }
    }
}
