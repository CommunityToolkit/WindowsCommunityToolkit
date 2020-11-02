// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Windows.Apps.Test.Foundation.Controls;
using Windows.UI.Xaml.Tests.MUXControls.InteractionTests.Common;

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
    public class Tests : UITest
    {
        [TestMethod]
        [TestPage("Simple")]
        public void SimpleTest()
        {
            var button = new Button(FindElement.ByName("Click Me"));
            var textBlock = new TextBlock(FindElement.ById("textBlock"));

            Verify.IsNotNull(button);

            Verify.AreEqual(string.Empty, textBlock.GetText());

            button.Click();

            Wait.ForIdle();

            Verify.AreEqual("Clicked", textBlock.GetText());
        }

        [TestMethod]
        [TestPage("TextBox Mask")]
        public void TestTextBoxMaskBinding_Property()
        {
            var initialValue = FindElement.ById<TextBlock>("InitialValueTextBlock").GetText();
            var textBox = FindElement.ById<Edit>("TextBox");

            Verify.AreEqual(initialValue, textBox.GetText());

            var changeButton = FindElement.ById<Button>("ChangeButton");

            changeButton.Click();
            Wait.ForIdle();

            var newValue = FindElement.ById<TextBlock>("NewValueTextBlock").GetText();

            Verify.AreEqual(newValue, textBox.GetText());
        }
    }
}