// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Windows.Apps.Test.Foundation;
using Microsoft.Windows.Apps.Test.Foundation.Controls;
using Windows.UI.Xaml.Tests.MUXControls.InteractionTests.Common;
using Windows.UI.Xaml.Tests.MUXControls.InteractionTests.Infra;
using Microsoft.Windows.Apps.Test.Automation;

#if USING_TAEF
using WEX.Logging.Interop;
using WEX.TestExecution;
using WEX.TestExecution.Markup;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace UITests.Tests
{
    [TestClass]
    public class RichSuggestBoxTest : UITestBase
    {
        [ClassInitialize]
        [TestProperty("RunAs", "User")]
        [TestProperty("Classification", "ScenarioTestSuite")]
        [TestProperty("Platform", "Any")]
        public static void ClassInitialize(TestContext testContext)
        {
            TestEnvironment.Initialize(testContext, WinUICsUWPSampleApp);
        }

        [TestMethod]
        [TestPage("RichSuggestBoxTestPage")]
        public void RichSuggestBox_DefaultTest()
        {
            var richSuggestBox = FindElement.ByName("richSuggestBox");
            var richEditBox = new TextBlock(FindElement.ByClassName("RichEditBox"));
            var tokenCounter = new TextBlock(FindElement.ById("tokenCounter"));
            var tokenListView = FindElement.ById("tokenListView");

            Verify.AreEqual(string.Empty, richEditBox.GetText());

            richEditBox.SendKeys("Hello@Test1");

            var suggestListView = richSuggestBox.Descendants.Find(UICondition.CreateFromClassName("ListView"));
            Verify.IsNotNull(suggestListView);
            Verify.AreEqual(3, suggestListView.Children.Count);
            InputHelper.LeftClick(suggestListView.Children[0]);

            var tokenInfo1 = tokenListView.Children[0];
            var text = "Hello\u200b@Test1Token1\u200b ";
            var actualText = richEditBox.GetText(false);
            Verify.AreEqual(text, actualText);
            Verify.AreEqual("1", tokenCounter.GetText());
            Verify.AreEqual("Token1", tokenInfo1.Children[0].GetText());
            Verify.AreEqual("5", tokenInfo1.Children[1].GetText());

            richEditBox.SendKeys("@Test2");
            Verify.AreEqual(3, suggestListView.Children.Count);
            InputHelper.LeftClick(suggestListView.Children[1]);

            var tokenInfo2 = tokenListView.Children[1];
            text = "Hello\u200b@Test1Token1\u200b \u200b@Test2Token2\u200b ";
            actualText = richEditBox.GetText(false);
            Verify.AreEqual(text, actualText);
            Verify.AreEqual("2", tokenCounter.GetText());
            Verify.AreEqual("Token2", tokenInfo2.Children[0].GetText());
            Verify.AreEqual("68", tokenInfo2.Children[1].GetText());

            KeyboardHelper.PressKey(Key.Home);
            richEditBox.SendKeys(" ");
            Verify.AreEqual("6", tokenInfo1.Children[1].GetText());
            Verify.AreEqual("69", tokenInfo2.Children[1].GetText());
        }
    }
}