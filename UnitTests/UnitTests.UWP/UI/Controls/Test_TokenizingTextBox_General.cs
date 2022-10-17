// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace UnitTests.UWP.UI.Controls
{
    [TestClass]
    public class Test_TokenizingTextBox_General : VisualUITestBase
    {
        [TestCategory("Test_TokenizingTextBox_General")]
        [TestMethod]
        public async Task Test_ClearTokens()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(
@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">

    <controls:TokenizingTextBox x:Name=""tokenboxname"">
    </controls:TokenizingTextBox>

</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                await SetTestContentAsync(treeRoot);

                var tokenBox = treeRoot.FindChild("tokenboxname") as TokenizingTextBox;

                Assert.IsNotNull(tokenBox, "Could not find TokenizingTextBox in tree.");
                Assert.AreEqual(1, tokenBox.Items.Count, "Token default items failed");

                // Add 4 items
                tokenBox.AddTokenItem("TokenItem1");
                tokenBox.AddTokenItem("TokenItem2");
                tokenBox.AddTokenItem("TokenItem3");
                tokenBox.AddTokenItem("TokenItem4");

                Assert.AreEqual(5, tokenBox.Items.Count, "Token Add count failed"); // 5th item is the textbox

                var count = 0;

                tokenBox.TokenItemRemoving += (sender, args) => { count++; };

                // now test clear
                await tokenBox.ClearAsync();

                Assert.AreEqual(1, tokenBox.Items.Count, "Clear Failed to clear"); // Still expect textbox to remain
                Assert.AreEqual(4, count, "Did not receive 4 removal events.");
            });
        }

        [TestCategory("Test_TokenizingTextBox_General")]
        [TestMethod]
        public async Task Test_ClearTokenCancel()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(
@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">

    <controls:TokenizingTextBox x:Name=""tokenboxname"">
    </controls:TokenizingTextBox>

</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                await SetTestContentAsync(treeRoot);

                var tokenBox = treeRoot.FindChild("tokenboxname") as TokenizingTextBox;

                Assert.IsNotNull(tokenBox, "Could not find TokenizingTextBox in tree.");
                Assert.AreEqual(1, tokenBox.Items.Count, "Token default items failed");

                // test cancelled clear
                tokenBox.AddTokenItem("TokenItem1");
                tokenBox.AddTokenItem("TokenItem2");
                tokenBox.AddTokenItem("TokenItem3");
                tokenBox.AddTokenItem("TokenItem4");

                Assert.AreEqual(5, tokenBox.Items.Count, "Token Add count failed");

                tokenBox.TokenItemRemoving += (sender, args) => { args.Cancel = true; };

                await tokenBox.ClearAsync();

                // Should have the same number of items left
                Assert.AreEqual(5, tokenBox.Items.Count, "Cancelled Clear Failed ");

                // TODO: We should have test for individual removal as well.
            });
        }

        [TestCategory("Test_TokenizingTextBox_General")]
        [TestMethod]
        public async Task Test_MaximumTokens()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
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

                await SetTestContentAsync(treeRoot);

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
            });
        }

        [TestCategory("Test_TokenizingTextBox_General")]
        [TestMethod]
        public async Task Test_SetInitialText()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(
@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">

    <controls:TokenizingTextBox x:Name=""tokenboxname"" Text=""Some Text""/>

</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                await SetTestContentAsync(treeRoot);

                var tokenBox = treeRoot.FindChild("tokenboxname") as TokenizingTextBox;

                Assert.IsNotNull(tokenBox, "Could not find TokenizingTextBox in tree.");
                Assert.AreEqual(1, tokenBox.Items.Count, "Token default items failed"); // AutoSuggestBox

                // Test initial value of property
                Assert.AreEqual("Some Text", tokenBox.Text, "Token text not equal to starting value.");

                // Reach into AutoSuggestBox's text to check it was set properly
                var autoSuggestBox = tokenBox.FindDescendant<AutoSuggestBox>();

                Assert.IsNotNull(autoSuggestBox, "Could not find inner autosuggestbox");
                Assert.AreEqual("Some Text", autoSuggestBox.Text, "Inner text not set based on initial value of TokenizingTextBox");
            });
        }

        [TestCategory("Test_TokenizingTextBox_General")]
        [TestMethod]
        public async Task Test_ChangeText()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(
@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">

    <controls:TokenizingTextBox x:Name=""tokenboxname""/>

</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                await SetTestContentAsync(treeRoot);

                var tokenBox = treeRoot.FindChild("tokenboxname") as TokenizingTextBox;

                Assert.IsNotNull(tokenBox, "Could not find TokenizingTextBox in tree.");
                Assert.AreEqual(1, tokenBox.Items.Count, "Token default items failed"); // AutoSuggestBox

                // Test initial value of property
                Assert.AreEqual(string.Empty, tokenBox.Text, "Text should start as empty.");

                // Reach into AutoSuggestBox's text to check it was set properly
                var autoSuggestBox = tokenBox.FindDescendant<AutoSuggestBox>();

                Assert.IsNotNull(autoSuggestBox, "Could not find inner autosuggestbox");
                Assert.AreEqual(string.Empty, autoSuggestBox.Text, "Inner text not set based on initial value of TokenizingTextBox");

                // Change Text
                tokenBox.Text = "New Text";

                // Wait for update
                await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() => { });

                Assert.AreEqual("New Text", tokenBox.Text, "Text should be changed now.");
                Assert.AreEqual("New Text", autoSuggestBox.Text, "Inner text not set based on value of TokenizingTextBox");
            });
        }

        [TestCategory("Test_TokenizingTextBox_General")]
        [TestMethod]
        public async Task Test_ClearText()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(
@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">

    <controls:TokenizingTextBox x:Name=""tokenboxname"" Text=""Some Text""/>

</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                await SetTestContentAsync(treeRoot);

                var tokenBox = treeRoot.FindChild("tokenboxname") as TokenizingTextBox;

                Assert.IsNotNull(tokenBox, "Could not find TokenizingTextBox in tree.");
                Assert.AreEqual(1, tokenBox.Items.Count, "Token default items failed"); // AutoSuggestBox

                // TODO: When in Labs, we should inject text via keyboard here vs. setting an initial value (more independent of SetInitialText test).

                // Test initial value of property
                Assert.AreEqual("Some Text", tokenBox.Text, "Token text not equal to starting value.");

                // Reach into AutoSuggestBox's text to check it was set properly
                var autoSuggestBox = tokenBox.FindDescendant<AutoSuggestBox>();

                Assert.IsNotNull(autoSuggestBox, "Could not find inner autosuggestbox");
                Assert.AreEqual("Some Text", autoSuggestBox.Text, "Inner text not set based on initial value of TokenizingTextBox");

                await tokenBox.ClearAsync();

                Assert.AreEqual(string.Empty, autoSuggestBox.Text, "Inner text was not cleared.");
                Assert.AreEqual(string.Empty, tokenBox.Text, "TokenizingTextBox text was not cleared.");
            });
        }

        [TestCategory("Test_TokenizingTextBox_General")]
        [TestMethod]
        public async Task Test_SetInitialTextWithDelimiter()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(
@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">

    <controls:TokenizingTextBox x:Name=""tokenboxname"" TokenDelimiter="","" Text=""Token 1, Token 2, Token 3""/>

</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                await SetTestContentAsync(treeRoot);

                var tokenBox = treeRoot.FindChild("tokenboxname") as TokenizingTextBox;

                Assert.IsNotNull(tokenBox, "Could not find TokenizingTextBox in tree.");
                Assert.AreEqual(1, tokenBox.Items.Count, "Tokens not created"); // AutoSuggestBox

                Assert.AreEqual("Token 1, Token 2, Token 3", tokenBox.Text, "Token text not equal to starting value.");

                await Task.Delay(500); // TODO: Wait for a loaded event?

                Assert.AreEqual(1 + 2, tokenBox.Items.Count, "Tokens not created");

                // Test initial value of property
                Assert.AreEqual("Token 3", tokenBox.Text, "Token text should be last value now.");

                Assert.AreEqual("Token 1", tokenBox.Items[0]);
                Assert.AreEqual("Token 2", tokenBox.Items[1]);
            });
        }

        [TestCategory("Test_TokenizingTextBox_General")]
        [TestMethod]
        public async Task Test_SetInitialTextWithDelimiterAll()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(
@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">

    <controls:TokenizingTextBox x:Name=""tokenboxname"" TokenDelimiter="","" Text=""Token 1, Token 2, Token 3,   ""/>

</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                await SetTestContentAsync(treeRoot);

                var tokenBox = treeRoot.FindChild("tokenboxname") as TokenizingTextBox;

                Assert.IsNotNull(tokenBox, "Could not find TokenizingTextBox in tree.");
                Assert.AreEqual(1, tokenBox.Items.Count, "Tokens not created"); // AutoSuggestBox

                Assert.AreEqual("Token 1, Token 2, Token 3,   ", tokenBox.Text, "Token text not equal to starting value.");

                await Task.Delay(500); // TODO: Wait for a loaded event?

                Assert.AreEqual(1 + 3, tokenBox.Items.Count, "Tokens not created");

                // Test initial value of property
                Assert.AreEqual(string.Empty, tokenBox.Text, "Token text should be blank now.");

                Assert.AreEqual("Token 1", tokenBox.Items[0]);
                Assert.AreEqual("Token 2", tokenBox.Items[1]);
                Assert.AreEqual("Token 3", tokenBox.Items[2]);
            });
        }
    }
}