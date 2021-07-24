// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Windows.Apps.Test.Foundation.Controls;
using Windows.UI.Xaml.Tests.MUXControls.InteractionTests.Common;
using Windows.UI.Xaml.Tests.MUXControls.InteractionTests.Infra;

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
    public class ColorPickerButtonTest : UITestBase
    {
        [ClassInitialize]
        [TestProperty("RunAs", "User")]
        [TestProperty("Classification", "ScenarioTestSuite")]
        [TestProperty("Platform", "Any")]
        public static void ClassInitialize(TestContext testContext)
        {
            TestEnvironment.Initialize(testContext, WinUICSharpUWPSampleApp);
        }

        /// <summary>
        /// This test validates the two way binding of the selected color. It verifies that
        /// when the bound property changes this change is properly forwarded to the internal colorpicker.
        /// See also issue #4367
        /// </summary>
        [TestMethod]
        [TestPage("ColorPickerButtonTestPage")]
        public void TwoWayTestMethod()
        {
            var colorpicker = new Button(FindElement.ById("TheColorPickerButton"));

            var redButton = new Button(FindElement.ById("SetRedButton"));

            Verify.IsNotNull(colorpicker);
            Verify.IsNotNull(redButton);

            colorpicker.Click();

            Wait.ForIdle();
            var colorInput = GetColorPickerInputField();

            Verify.AreEqual("008000", colorInput.GetText());

            // close the picker
            colorpicker.Click();

            Wait.ForIdle();

            redButton.Click();

            Wait.ForIdle();

            colorpicker.Click();

            var colorInput_new = GetColorPickerInputField();
            Verify.AreEqual("FF0000", colorInput_new.GetText());
        }

        private static Edit GetColorPickerInputField()
        {
            var channelButton = new Button(FindElement.ByName("Channels"));
            Verify.IsNotNull(channelButton);

            Wait.ForIdle();

            channelButton.Click();

            Wait.ForIdle();

            var colorInput = new Edit(FindElement.ByName("Hexadecimal Color Input"));
            Verify.IsNotNull(colorInput);
            return colorInput;
        }
    }
}