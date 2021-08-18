// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;

namespace UnitTests.UWP.UI.Controls
{
    [TestClass]
    public class Test_TokenizingTextBox_General
    {
        [TestCategory("Test_TokenizingTextBox_General")]
        [UITestMethod]
        public void Test_Clear()
        {
            var treeRoot = XamlReader.Load(
@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:CommunityToolkit.WinUI.UI.Controls"">

    <controls:TokenizingTextBox x:Name=""tokenboxname"">
    </controls:TokenizingTextBox>

</Page>") as FrameworkElement;

            Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

            var tokenBox = treeRoot.FindChild("tokenboxname") as TokenizingTextBox;

            Assert.IsNotNull(tokenBox, "Could not find TokenizingTextBox in tree.");

            Assert.AreEqual(tokenBox.Items.Count, 1, "Token default items failed");

            tokenBox.AddTokenItem("TokenItem1");
            tokenBox.AddTokenItem("TokenItem2");
            tokenBox.AddTokenItem("TokenItem3");
            tokenBox.AddTokenItem("TokenItem4");

            Assert.AreEqual(tokenBox.Items.Count, 5, "Token Add count failed");

            // now test clear
            Assert.IsTrue(tokenBox.ClearAsync().Wait(200), "Failed to wait for Clear() to finish");

            Assert.AreEqual(tokenBox.Items.Count, 1, "Clear Failed to clear");

            // test cancelled clear
            tokenBox.AddTokenItem("TokenItem1");
            tokenBox.AddTokenItem("TokenItem2");
            tokenBox.AddTokenItem("TokenItem3");
            tokenBox.AddTokenItem("TokenItem4");

            Assert.AreEqual(tokenBox.Items.Count, 5, "Token Add count failed");

            tokenBox.TokenItemRemoving += (sender, args) => { args.Cancel = true; };

            Assert.IsTrue(tokenBox.ClearAsync().Wait(200), "Failed to wait for Clear() to finish");

            Assert.AreEqual(tokenBox.Items.Count, 5, "Cancelled Clear Failed ");
        }

        [TestCategory("Test_TokenizingTextBox_General")]
        [UITestMethod]
        public void Test_MaximumTokens()
        {
            var maxTokens = 2;

            var treeRoot = XamlReader.Load(
$@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">

    <controls:TokenizingTextBox x:Name=""tokenboxname"" MaximumTokens=""{maxTokens}"">
    </controls:TokenizingTextBox>

</Page>") as FrameworkElement;

            Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

            var tokenBox = treeRoot.FindChild("tokenboxname") as TokenizingTextBox;

            Assert.IsNotNull(tokenBox, "Could not find TokenizingTextBox in tree.");

            // Items includes the text fields as well, so we can expect at least one item to exist initially, the input box.
            // Use the starting count as an offset.
            var startingItemsCount = tokenBox.Items.Count;

            // Add two items.
            tokenBox.AddTokenItem("TokenItem1");
            tokenBox.AddTokenItem("TokenItem2");

            // Make sure we have the appropriate amount of items and that they are in the appropriate order.
            Assert.AreEqual(startingItemsCount + maxTokens, tokenBox.Items.Count, "Token Add failed");
            Assert.AreEqual("TokenItem1", tokenBox.Items[0]);
            Assert.AreEqual("TokenItem2", tokenBox.Items[1]);

            // Attempt to add an additional item, beyond the maximum.
            tokenBox.AddTokenItem("TokenItem3");

            // Check that the number of items did not change, because the maximum number of items are already present.
            Assert.AreEqual(startingItemsCount + maxTokens, tokenBox.Items.Count, "Token Add succeeded, where it should have failed.");
            Assert.AreEqual("TokenItem1", tokenBox.Items[0]);
            Assert.AreEqual("TokenItem2", tokenBox.Items[1]);

            // Reduce the maximum number of tokens.
            tokenBox.MaximumTokens = 1;

            // The last token should be removed to account for the reduced maximum.
            Assert.AreEqual(startingItemsCount + 1, tokenBox.Items.Count);
            Assert.AreEqual("TokenItem1", tokenBox.Items[0]);
        }
    }
}