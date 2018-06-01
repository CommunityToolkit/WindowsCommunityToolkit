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