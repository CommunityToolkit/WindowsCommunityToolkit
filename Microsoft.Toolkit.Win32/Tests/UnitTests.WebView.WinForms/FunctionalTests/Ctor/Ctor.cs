// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Ctor
{
    [TestClass]
    public class CreationTests
    {
        [TestMethod]
        [TestCategory(TestConstants.Categories.Init)]
        public void CanInitializeCtorBeginEndInit()
        {
            var wv = new UI.Controls.WinForms.WebView();
            ((ISupportInitialize)wv).BeginInit();
            ((ISupportInitialize)wv).EndInit();
        }

        [TestMethod]
        [TestCategory(TestConstants.Categories.Init)]
        public void CanInitializeCtorOnly()
        {
            var wv = new UI.Controls.WinForms.WebView();
            wv.Process.ShouldBeNull();
        }

        [TestMethod]
        [TestCategory(TestConstants.Categories.Init)]
        public void DesignerPropertyEqualsSettingsProperty()
        {
            var wv = new UI.Controls.WinForms.WebView();
            ((ISupportInitialize)wv).BeginInit();
            wv.IsScriptNotifyAllowed = !wv.IsScriptNotifyAllowed;
            ((ISupportInitialize)wv).EndInit();

            wv.IsScriptNotifyAllowed.ShouldEqual(wv.Settings.IsScriptNotifyAllowed);
        }


    }
}
