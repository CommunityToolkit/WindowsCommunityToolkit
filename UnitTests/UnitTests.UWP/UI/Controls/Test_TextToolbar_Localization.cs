// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons.Common;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats.RichText;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System.Linq;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace UnitTests.UI.Controls
{
    [TestClass]
    public class Test_TextToolbar_Localization
    {
        /// <summary>
        /// Tests the general ability to look-up a resource from the UI control as a base-case.
        /// </summary>
        [TestCategory("Test_TextToolbar_Localization")]
        [UITestMethod]
        public void Test_Retrieve()
        {
            var treeRoot = XamlReader.Load(
@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">

    <controls:TextToolbar x:Name=""TextToolbarControl"">
    </controls:TextToolbar>

</Page>") as FrameworkElement;

            Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

            var toolbar = treeRoot.FindChildByName("TextToolbarControl") as TextToolbar;

            Assert.IsNotNull(toolbar, "Could not find TextToolbar in tree.");

            var commonButtons = new CommonButtons(toolbar);
            var boldButton = commonButtons.Bold;

            Assert.IsNotNull(boldButton, "Bold Button not found.");

            Assert.AreEqual("Bold", boldButton.ToolTip, "Label doesn't match expected default value.");
        }

        /// <summary>
        /// Tests the ability to override the resource lookup for a toolkit component in the app resource dictionary.
        /// See Link:Strings/en-us/Resources.resw
        /// </summary>
        [TestCategory("Test_TextToolbar_Localization")]
        [UITestMethod]
        public void Test_Override()
        {
            var commonButtons = new CommonButtons(new TextToolbar());
            var italicsButton = commonButtons.Italics;

            Assert.IsNotNull(italicsButton, "Italics Button not found.");

            Assert.AreEqual("ItalicsOverride", italicsButton.ToolTip, "Label doesn't match expected default value.");
        }

        /// <summary>
        /// Tests the ability to have different overrides in different languages.
        /// </summary>
        [TestCategory("Test_TextToolbar_Localization")]
        [UITestMethod]
        public void Test_Override_Fr()
        {
            // Just double-check we've got the right environment setup in our tests.
            CollectionAssert.AreEquivalent(new string[] { "en-US", "fr" }, ApplicationLanguages.ManifestLanguages.ToArray(), "Missing locales for test");

            // Override the default language for this test only (we'll set it back after).
            var defaultLanguage = ApplicationLanguages.PrimaryLanguageOverride;
            ApplicationLanguages.PrimaryLanguageOverride = "fr";

            var commonButtons = new CommonButtons(new TextToolbar());
            var italicsButton = commonButtons.Italics;

            ApplicationLanguages.PrimaryLanguageOverride = defaultLanguage;

            // Check for expected values.
            Assert.IsNotNull(italicsButton, "Italics Button not found.");

            Assert.AreEqual("ItalicsFr", italicsButton.ToolTip, "Label doesn't match expected default value.");
        }
    }
}
