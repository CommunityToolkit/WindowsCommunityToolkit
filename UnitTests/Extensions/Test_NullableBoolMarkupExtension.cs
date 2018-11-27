// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using UnitTests.Extensions.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

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

        [TestCategory("NullableBoolMarkupExtension")]
        [UITestMethod]
        public void Test_NullableBool_Test_TestObject()
        {
            // Just test that we can properly parse our object as-is into our test harness.
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:ex=""using:Microsoft.Toolkit.Uwp.UI.Extensions""
    xmlns:helpers=""using:UnitTests.Extensions.Helpers"">
    <Page.Resources>
        <helpers:ObjectWithNullableBoolProperty x:Key=""OurObject""/>
    </Page.Resources>
</Page>") as FrameworkElement;

            var obj = treeroot.Resources["OurObject"] as ObjectWithNullableBoolProperty;

            Assert.IsNotNull(obj, "Could not find object in resources.");

            Assert.AreEqual(null, obj.NullableBool, "Expected obj value to be null.");
        }

        [TestCategory("NullableBoolMarkupExtension")]
        [UITestMethod]
        public void Test_NullableBool_DependencyProperty_SystemTrueValueFails()
        {
            // This is the failure case in the OS currently which causes us to need
            // this markup extension.
            var exception = Assert.ThrowsException<XamlParseException>(
                () =>
            {
                var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:ex=""using:Microsoft.Toolkit.Uwp.UI.Extensions""
    xmlns:helpers=""using:UnitTests.Extensions.Helpers"">
    <Page.Resources>
        <helpers:ObjectWithNullableBoolProperty x:Key=""OurObject"" NullableBool=""True""/>
    </Page.Resources>
</Page>") as FrameworkElement;
            }, "Expected assignment failure during parsing, OS now supports, update documentation.");

            Assert.IsNotNull(exception);

            Assert.IsTrue(exception.Message.Contains("Failed to create a 'Windows.Foundation.IReference`1<Boolean>' from the text 'True'."));
        }

        [TestCategory("NullableBoolMarkupExtension")]
        [UITestMethod]
        public void Test_NullableBool_DependencyProperty_TrueValue()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:ex=""using:Microsoft.Toolkit.Uwp.UI.Extensions""
    xmlns:helpers=""using:UnitTests.Extensions.Helpers"">
    <Page.Resources>
        <helpers:ObjectWithNullableBoolProperty x:Key=""OurObject"" NullableBool=""{ex:NullableBool Value=True}""/>
    </Page.Resources>
</Page>") as FrameworkElement;
                
            var obj = treeroot.Resources["OurObject"] as ObjectWithNullableBoolProperty;

            Assert.IsNotNull(obj, "Could not find object in resources.");

            Assert.AreEqual(true, obj.NullableBool, "Expected obj value to be true.");
        }
    }
}
