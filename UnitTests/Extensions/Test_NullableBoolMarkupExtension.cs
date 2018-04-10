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

using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Assert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;

namespace UnitTests.Extensions
{
    [TestClass]
    public class Test_NullableBoolMarkupExtension
    {
        [TestCategory("NullableBoolMarkupExtension")]
        [UITestMethod]
        public void Test_NullableBool_MarkupExtension_ProvidesTrue()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:ex=""using:Microsoft.Toolkit.Uwp.UI.Extensions"">
        <CheckBox x:Name=""Check"" IsChecked=""{ex:NullableBool Value=True}""/>
</Page>") as FrameworkElement;

            var toggle = treeroot.FindChildByName("Check") as CheckBox;

            Assert.IsNotNull(toggle, "Could not find checkbox control in tree.");

            Assert.AreEqual(true, toggle.IsChecked, "Expected checkbox value to be true.");
        }

        [TestCategory("NullableBoolMarkupExtension")]
        [UITestMethod]
        public void Test_NullableBool_MarkupExtension_ProvidesFalse()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:ex=""using:Microsoft.Toolkit.Uwp.UI.Extensions"">
        <CheckBox x:Name=""Check"" IsChecked=""{ex:NullableBool Value=False}""/>
</Page>") as FrameworkElement;

            var toggle = treeroot.FindChildByName("Check") as CheckBox;

            Assert.IsNotNull(toggle, "Could not find checkbox control in tree.");

            Assert.AreEqual(false, toggle.IsChecked, "Expected checkbox value to be false.");
        }

        [TestCategory("NullableBoolMarkupExtension")]
        [UITestMethod]
        public void Test_NullableBool_MarkupExtension_ProvidesNull()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:ex=""using:Microsoft.Toolkit.Uwp.UI.Extensions"">
        <CheckBox x:Name=""Check"" IsChecked=""{ex:NullableBool IsNull=True}""/>
</Page>") as FrameworkElement;

            var toggle = treeroot.FindChildByName("Check") as CheckBox;

            Assert.IsNotNull(toggle, "Could not find checkbox control in tree.");

            Assert.AreEqual(null, toggle.IsChecked, "Expected checkbox value to be null.");
        }

        [TestCategory("NullableBoolMarkupExtension")]
        [UITestMethod]
        public void Test_NullableBool_MarkupExtension_ProvidesNullStillWithTrueValue()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:ex=""using:Microsoft.Toolkit.Uwp.UI.Extensions"">
        <CheckBox x:Name=""Check"" IsChecked=""{ex:NullableBool IsNull=True, Value=True}""/>
</Page>") as FrameworkElement;

            var toggle = treeroot.FindChildByName("Check") as CheckBox;

            Assert.IsNotNull(toggle, "Could not find checkbox control in tree.");

            Assert.AreEqual(null, toggle.IsChecked, "Expected checkbox value to be null.");
        }

        [TestCategory("NullableBoolMarkupExtension")]
        [UITestMethod]
        public void Test_NullableBool_MarkupExtension_ProvidesNullStillWithFalseValue()
        {
            // Should be no-op as Value is false by default.
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:ex=""using:Microsoft.Toolkit.Uwp.UI.Extensions"">
        <CheckBox x:Name=""Check"" IsChecked=""{ex:NullableBool IsNull=True, Value=False}""/>
</Page>") as FrameworkElement;

            var toggle = treeroot.FindChildByName("Check") as CheckBox;

            Assert.IsNotNull(toggle, "Could not find checkbox control in tree.");

            Assert.AreEqual(null, toggle.IsChecked, "Expected checkbox value to be null.");
        }
    }
}
