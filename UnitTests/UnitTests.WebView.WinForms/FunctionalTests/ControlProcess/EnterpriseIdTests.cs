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

using Microsoft.Toolkit.Win32.UI.Controls.Interop;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.ControlProcess
{
    [TestClass]
    public class EnterpriseIdTests : ProcessFactoryWebViewContextSpecification
    {
        protected override void Given()
        {
            // TODO: Does Windows Information Protection work for WinForms?
            OSVersionHelper.UseWindowsInformationProtectionApi.ShouldBeTrue("The operating system does not support Windows Information Protection (WIP) or WIP is not enabled on this device.");

            var o = new WebViewControlProcessOptions() {EnterpriseId = nameof(EnterpriseIdTests)};
            CreateWebView(o);
        }

        [TestMethod]
        public void Unknown()
        {
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Inconclusive("No implemented behavior for enlightened LOB using WebView.");
        }
    }
}
