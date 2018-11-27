// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.ScriptNotify
{
    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class ScriptNotifyOnNavigateToString : HostFormWebViewContextSpecification
    {
        private string _content;
        private bool _scriptNotifyCalled;

        protected override void Given()
        {
            _content = @"
<html>
  <body OnLoad=""window.external.notify('Hello World!')"">
  </body>
</html>
";

            base.Given();
            WebView.IsScriptNotifyAllowed = true;
            WebView.IsJavaScriptEnabled = true;

            WebView.ScriptNotify += (c, a) =>
            {
                _scriptNotifyCalled = true;
                a.Value.ShouldEqual("Hello World!", "ScriptNotify received, but with different argument.");
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateToStringAndWaitForFormClose(_content);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]

        public void ScriptNotifyRaised()
        {
            _scriptNotifyCalled.ShouldBeTrue();
        }
    }
}