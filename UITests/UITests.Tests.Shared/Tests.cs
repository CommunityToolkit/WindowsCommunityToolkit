using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
                string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string baseDirectory = Path.Combine(Directory.GetParent(assemblyDir).Parent.Parent.Parent.FullName, "UITests.App");

                var exclude = new[] { "Microsoft.NET.CoreRuntime", "Microsoft.VCLibs" };
                var files = Directory.GetFiles(baseDirectory, "*.appx", SearchOption.AllDirectories).Where(f => !exclude.Any(Path.GetFileNameWithoutExtension(f).Contains));

                if (files.Count() == 0)
                {
                    throw new Exception(string.Format("Failed to find '*.appx' in {0}'!", baseDirectory));
                }

                string mostRecentlyBuiltAppx = string.Empty;
                DateTime timeMostRecentlyBuilt = DateTime.MinValue;

                foreach (string file in files)
                {
                    DateTime fileWriteTime = File.GetLastWriteTime(file);

                    if (fileWriteTime > timeMostRecentlyBuilt)
                    {
                        timeMostRecentlyBuilt = fileWriteTime;
                        mostRecentlyBuiltAppx = file;
                    }
                }

                return new TestApplicationInfo(
                    "UITests.App",
                    "3568ebdf-5b6b-4ddd-bb17-462d614ba50f_gspb8g6x97k2t!App",
                    "3568ebdf-5b6b-4ddd-bb17-462d614ba50f_gspb8g6x97k2t",
                    "UITests.App",
                    "UITests.App.exe",
                    mostRecentlyBuiltAppx.Replace(".appx", string.Empty),
                    "24d62f3b13b8b9514ead9c4de48cc30f7cc6151d",
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