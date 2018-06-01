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