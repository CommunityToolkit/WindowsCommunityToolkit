using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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
    public class Tests
    {
        public static TestApplicationInfo WinUICsUWPSampleApp
        {
            get
            {
                var arch = RuntimeInformation.ProcessArchitecture.ToString();

                string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string baseDirectory = Path.Combine(Directory.GetParent(assemblyDir).Parent.Parent.Parent.FullName, "UITests.App");

                return new TestApplicationInfo(
                    "3568ebdf-5b6b-4ddd-bb17-462d614ba50f",
                    "3568ebdf-5b6b-4ddd-bb17-462d614ba50f_gspb8g6x97k2t!App",
                    "3568ebdf-5b6b-4ddd-bb17-462d614ba50f_gspb8g6x97k2t",
                    "UITests.App",
                    "UITests.App.exe",
                    "UITests.App_1.0.0.0_" + arch + "_Debug",
                    "78851a63d7f5e9d97f416a5ce3ffae6d1c38744f",
                    baseDirectory);
            }
        }

        public static TestSetupHelper.TestSetupHelperOptions TestSetupHelperOptions
        {
            get
            {
                return new TestSetupHelper.TestSetupHelperOptions
                {
                    AutomationIdOfSafeItemToClick = string.Empty
                };
            }
        }

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

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void SimpleLaunchTest()
        {
            var button = new Button(FindElement.ByName("Click Me"));
            var textBlock = new TextBlock(FindElement.ById("textBlock"));

            Verify.IsNotNull(button);

            Verify.AreEqual(string.Empty, textBlock.GetText());

            button.Click();

            Wait.ForIdle();

            Verify.AreEqual("Clicked", textBlock.GetText());
        }
    }
}