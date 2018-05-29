// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.ScriptNotify
{
    [TestClass]
    public class ScriptNotifyOnNavigate : HostFormWebViewContextSpecification
    {
        private string _parameter;
        private string _content;
        private bool _scriptNotifyCalled;

        protected override void Given()
        {
            _parameter = "2";
            _content = $"(()=> {{ window.external.notify('{_parameter}');}})()";

            base.Given();
            WebView.IsScriptNotifyAllowed = true;
            WebView.IsJavaScriptEnabled = true;

            // This runs after Navigate
            WebView.NavigationCompleted += (o, e) =>
            {
                // Set up the event handler
                WebView.ScriptNotify += (c, a) =>
                {
                    _scriptNotifyCalled = true;
                    a.Value.ShouldEqual(_parameter, "ScriptNotify received, but with different argument.");
                    Form.Close();
                };

                // Inject the script
                WebView.InvokeScriptAsync("eval", _content);
            };
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleCom);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        public void ScriptNotifyRaised()
        {
            _scriptNotifyCalled.ShouldBeTrue();
        }
    }
}
