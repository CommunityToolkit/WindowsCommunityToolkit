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

using System.Drawing;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Ctor
{
    [TestClass]
    public class MultipleWebViewsTests : HostFormWebViewContextSpecification
    {
        private Controls.WinForms.WebView WebView2 { get; set; }

        protected override void Given()
        {
            // Perform check to see if we can run before we get too far
            Assert.That.OSBuildShouldBeAtLeast(TestConstants.Windows10Builds.InsiderFast17650);

            base.Given();
        }

        protected override void When()
        {
            WebView2 = (Controls.WinForms.WebView)WebView.Process.CreateWebView(Form.Handle, Rectangle.Empty);
        }

        [TestMethod]
        public void SecondWebViewIsCreated()
        {
            WebView2.ShouldNotBeNull();
        }

        [TestMethod]
        public void SecondWebViewInSameProcess()
        {
            WebView2.Process.ProcessId.ShouldEqual(WebView.Process.ProcessId);
        }

        protected override void Cleanup()
        {
            WebView2?.Dispose();
            WebView2 = null;

            base.Cleanup();
        }
    }
}
