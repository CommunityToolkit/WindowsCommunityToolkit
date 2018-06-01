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
    public class RefreshTests : HostFormWebViewContextSpecification
    {
        private int _navigations;
        protected override void Given()
        {
            base.Given();
            WebView.NavigationCompleted += (o, e) =>
            {
                _navigations++;
                if (_navigations == 2)
                {
                    Form.Close();
                }
                else
                {
                    WebView.Refresh();
                }
            };
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleOrg);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        public void RefreshCountsAsNavigation()
        {
            _navigations.ShouldEqual(2);
        }
    }
}
