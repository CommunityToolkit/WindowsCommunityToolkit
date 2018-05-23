// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
