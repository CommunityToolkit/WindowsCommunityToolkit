// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.UserAgent
{
    [TestClass]
    public class Given_a_WebView : HostFormWebViewContextSpecification
    {
        private string _userAgent;

        protected override void Given()
        {
            base.Given();

            WebView.NavigationCompleted += (o, e) =>
            {
                var wv = o as Controls.WinForms.WebView;
                _userAgent = wv.Process.UserAgent;

                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleCom);
        }


        [TestMethod]
        public void UserAgent_accessed_without_exception()
        {
            _userAgent.ShouldEqual(string.Empty);
        }
    }
}
