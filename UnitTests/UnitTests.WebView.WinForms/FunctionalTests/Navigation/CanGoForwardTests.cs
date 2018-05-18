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

using System;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Navigation
{
    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class AfterNavigatingMoreThanOnceThenGoingBack : HostFormWebViewContextSpecification
    {
        private int _navigationCount;

        protected override void Given()
        {
            base.Given();

            WebView.NavigationCompleted += (o, e) =>
            {
                _navigationCount++;
                WriteLine($"NavCompleted: {e.Uri}; NavCount: {_navigationCount}");
                e.IsSuccess.ShouldBeTrue($"Navigation failure: {e.WebErrorStatus}");

                if (WebView.CanGoBack)
                {
                    WebView.GoBack();
                } else if (!WebView.CanGoBack && WebView.CanGoForward)
                {
                    Form.Close();
                }
                else
                {
                    WebView.Navigate(new Uri(TestConstants.Uris.ExampleCom, "?" + _navigationCount));
                }

                
            };

        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleCom);
        }

        [TestMethod]
        //[Timeout(TestConstants.Timeouts.Long)]
        public void CanGoForwardIsTrue()
        {
            // NOTE: Due to asynchronous nature the actual assert is in Given()
        }
    }
}
