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
    public class RangeSelectorTest : UITestBase
    {
        [ClassInitialize]
        [TestProperty("RunAs", "User")]
        [TestProperty("Classification", "ScenarioTestSuite")]
        [TestProperty("Platform", "Any")]
        public static void ClassInitialize(TestContext testContext)
        {
            TestEnvironment.Initialize(testContext, WinUICsUWPSampleApp);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestEnvironment.AssemblyCleanupWorker(WinUICsUWPSampleApp);
        }

        [TestMethod]
        [TestPage("RangeSelectorTestPage")]
        public void SimpleTestMethod2()
        {
            var inputStepFrequency = new Edit(FindElement.ById("inputStepFrequency"));
            var inputMinimum = new Edit(FindElement.ById("inputMinimum"));
            var inputRangeStart = new Edit(FindElement.ById("inputRangeStart"));
            var inputRangeEnd = new Edit(FindElement.ById("inputRangeEnd"));
            var inputMaximum = new Edit(FindElement.ById("inputMaximum"));

            var submitStepFrequency = new Button(FindElement.ById("submitStepFrequency"));
            var submitMinimum = new Button(FindElement.ById("submitMinimum"));
            var submitRangeStart = new Button(FindElement.ById("submitRangeStart"));
            var submitRangeEnd = new Button(FindElement.ById("submitRangeEnd"));
            var submitMaximum = new Button(FindElement.ById("submitMaximum"));
            var submitAll = new Button(FindElement.ById("submitAll"));

            inputStepFrequency.SendKeys("1");
            Wait.ForIdle();
            inputMinimum.SendKeys("0");
            Wait.ForIdle();
            inputRangeStart.SendKeys("10");
            Wait.ForIdle();
            inputRangeEnd.SendKeys("90");
            Wait.ForIdle();
            inputMaximum.SendKeys("100");
            Wait.ForIdle();

            submitAll.Click();
            Wait.ForIdle();

            var currentStepFrequency = new TextBlock(FindElement.ById("currentStepFrequency"));
            var currentMinimum = new TextBlock(FindElement.ById("currentMinimum"));
            var currentRangeStart = new TextBlock(FindElement.ById("currentRangeStart"));
            var currentRangeEnd = new TextBlock(FindElement.ById("currentRangeEnd"));
            var currentMaximum = new TextBlock(FindElement.ById("currentMaximum"));

            Verify.AreEqual("1", currentStepFrequency.GetText());
            Verify.AreEqual("0", currentMinimum.GetText());
            Verify.AreEqual("10", currentRangeStart.GetText());
            Verify.AreEqual("90", currentRangeEnd.GetText());
            Verify.AreEqual("100", currentMaximum.GetText());
        }
    }
}
