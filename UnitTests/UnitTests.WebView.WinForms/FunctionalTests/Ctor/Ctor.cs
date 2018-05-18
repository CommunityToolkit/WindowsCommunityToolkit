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
