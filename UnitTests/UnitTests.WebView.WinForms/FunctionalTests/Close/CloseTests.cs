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

using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Close
{
    [TestClass]
    public class CallCloseOnceAfterInit : BlockTestStartEndContextSpecification
    {
        protected override void CreateWebView()
        {
            WebView = new UI.Controls.WinForms.WebView();
            ((ISupportInitialize) WebView).BeginInit();
            ((ISupportInitialize) WebView).EndInit();

            WebView.ShouldNotBeNull();
            WebView.Process.ShouldNotBeNull();
        }

        protected override void When()
        {
            WebView.Close();
        }

        [TestMethod]
        public void Close()
        {
            WebView.Process.ShouldBeNull();
        }
    }

    [TestClass]
    public class CallCloseTwiceAfterInit : BlockTestStartEndContextSpecification
    {
        protected override void CreateWebView()
        {
            WebView = new UI.Controls.WinForms.WebView();
            ((ISupportInitialize)WebView).BeginInit();
            ((ISupportInitialize)WebView).EndInit();

            WebView.ShouldNotBeNull();
            WebView.Process.ShouldNotBeNull();
        }

        protected override void When()
        {
            WebView.Close();
            WebView.Close();
        }

        [TestMethod]
        public void Close()
        {
            WebView.Process.ShouldBeNull();
        }
    }
}
