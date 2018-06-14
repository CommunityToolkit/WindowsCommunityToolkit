// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
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
            WebView.IsIndexedDBEnabled.ShouldEqual(WebViewDefaults.IsIndexedDBEnabled);
        }

        [TestMethod]
        public void JavaScriptEnabled()
        {
            WebView.IsJavaScriptEnabled.ShouldEqual(WebViewDefaults.IsJavaScriptEnabled);
        }

        [TestMethod]
        public void ScriptNotifyDisabled()
        {
            WebView.IsScriptNotifyAllowed.ShouldEqual(WebViewDefaults.IsScriptNotifyEnabled);
        }

        [TestMethod]
        public void PrivateNetworkDisabled()
        {
            WebView.IsPrivateNetworkClientServerCapabilityEnabled.ShouldEqual(WebViewDefaults.IsPrivateNetworkEnabled);
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