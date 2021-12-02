// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Tests.MUXControls.InteractionTests.Common;
using Microsoft.UI.Xaml.Tests.MUXControls.InteractionTests.Infra;
using Microsoft.Windows.Apps.Test.Automation;
using Microsoft.Windows.Apps.Test.Foundation;
using Microsoft.Windows.Apps.Test.Foundation.Controls;

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
    public class ConstrainedBoxTest : UITestBase
    {
        [ClassInitialize]
        [TestProperty("RunAs", "User")]
        [TestProperty("Classification", "ScenarioTestSuite")]
        [TestProperty("Platform", "Any")]
        public static void ClassInitialize(TestContext testContext)
        {
            TestEnvironment.Initialize(testContext, UITestsAppSampleApp);
        }

        [TestMethod]
        [TestPage("ConstrainedBoxTestPage")]
        public void Test_AspectRatioBoundToInteger_Placeholder()
        {
            // The test is if the AspectRatio can be bound to integer
            // This test method acts as a placeholder, to spawn the XAML test page
            //       and test the binding to an integer
        }
    }
}