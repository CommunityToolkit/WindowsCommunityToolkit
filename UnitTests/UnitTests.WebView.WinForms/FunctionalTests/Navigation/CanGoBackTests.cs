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
