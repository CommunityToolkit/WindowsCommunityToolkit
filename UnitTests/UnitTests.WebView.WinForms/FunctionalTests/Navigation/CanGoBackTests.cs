// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Navigation
{
    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class AfterNavigatingMoreThanOnce : HostFormWebViewContextSpecification
    {
        private int _navigationCount;
        private bool _canGoBack;

        protected override void Given()
        {
            base.Given();

            WebView.NavigationCompleted += (o, e) =>
            {
                if (e.Uri == null) return;

                _navigationCount++;

                WriteLine($"NavCompleted: {e.Uri}");
                e.IsSuccess.ShouldBeTrue($"Navigation failure: {e.WebErrorStatus}");
                if (e.Uri == TestConstants.Uris.ExampleCom)
                {
                    WebView.Navigate(TestConstants.Uris.ExampleNet);
                }
                else
                {
                    _canGoBack = WebView.CanGoBack;
                    Form.Close();
                }
            };
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleCom);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Medium)]
        public void CanGoBackIsTrue()
        {
            _navigationCount.ShouldEqual(2);
            _canGoBack.ShouldBeTrue();
        }
    }
}
