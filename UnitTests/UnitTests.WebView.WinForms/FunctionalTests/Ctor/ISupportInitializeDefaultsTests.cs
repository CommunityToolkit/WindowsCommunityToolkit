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
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Ctor
{
    [TestClass]
    [TestCategory(TestConstants.Categories.Init)]
    public class WebViewBeginEndInitDefaults : BlockTestStartEndContextSpecification
    {
        protected override void CreateWebView()
        {
            WebView = new UI.Controls.WinForms.WebView();
        }

        protected override void When()
        {
            ((ISupportInitialize)WebView).BeginInit();
            ((ISupportInitialize)WebView).EndInit();
        }

        [TestMethod]
        public void IndexDBEnabled()
        {
            WebView.IsIndexDBEnabled.ShouldBeTrue();
        }

        [TestMethod]
        public void JavaScriptEnabled()
        {
            WebView.IsJavaScriptEnabled.ShouldBeTrue();
        }

        [TestMethod]
        public void ScriptNotifyEnabled()
        {
            WebView.IsScriptNotifyAllowed.ShouldBeTrue();
        }

        protected override void Cleanup()
        {
            WebView.Dispose();

            base.Cleanup();
        }
    }

    [TestClass]
    [TestCategory(TestConstants.Categories.Init)]
    public class WebViewControlProcessUnavailableBeforeEndInit : BlockTestStartEndContextSpecification
    {
        protected override void CreateWebView()
        {
            WebView = new UI.Controls.WinForms.WebView();
        }

        [TestMethod]
        public void ProcessIsUnavailable()
        {
            WebView.Process.ShouldBeNull();
        }

        protected override void Cleanup()
        {
            WebView.Close();
            base.Cleanup();
        }
    }

    [TestClass]
    [TestCategory(TestConstants.Categories.Init)]
    public class WebViewControlProcessAvailableAfterEndInit : BlockTestStartEndContextSpecification
    {
        protected override void CreateWebView()
        {
            WebView = new UI.Controls.WinForms.WebView();
            ((ISupportInitialize)WebView).BeginInit();
            ((ISupportInitialize)WebView).EndInit();
        }

        [TestMethod]
        public void ProcessIsAvailable()
        {
            WebView.Process.ShouldNotBeNull();
        }

        [TestMethod]
        public void SettingsIsAvailable()
        {
            WebView.Settings.ShouldNotBeNull();
        }

        protected override void Cleanup()
        {
            WebView.Close();
            base.Cleanup();
        }
    }
}