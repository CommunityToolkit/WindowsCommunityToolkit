// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Designer
{
    //TODO: Designer mode
    [TestClass]
    [TestCategory(TestConstants.Categories.Des)]
    public class ScriptNotifyPropertySet : BlockTestStartEndContextSpecification
    {
        protected override void CreateWebView()
        {
            WebView = new UI.Controls.WinForms.WebView()
            {
                IsScriptNotifyAllowed = true
            };
        }

        protected override void When()
        {
            WebView.IsScriptNotifyAllowed = false;
        }

        [TestMethod]
        //[Timeout(TestConstants.Timeouts.Short)]
        public void ScriptNotifyIsDisabled()
        {
            WebView.IsScriptNotifyAllowed.ShouldBeFalse();
        }

        protected override void Cleanup()
        {
            try
            {
                WebView.Close();
            }
            finally
            {
                base.Cleanup();
            }
        }
    }

    [TestClass]
    [TestCategory(TestConstants.Categories.Des)]
    public class BeginInitAfterScriptNotifyPropertySet : BlockTestStartEndContextSpecification
    {
        protected override void CreateWebView()
        {
            WebView = new UI.Controls.WinForms.WebView();
        }

        protected override void When()
        {
            WebView.IsScriptNotifyAllowed = false;
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Short)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ControlIsAlreadyInitialized()
        {
            ((ISupportInitialize)WebView).BeginInit();
        }

        protected override void Cleanup()
        {
            try
            {
                WebView.Close();
            }
            finally
            {
                base.Cleanup();
            }
        }
    }
}