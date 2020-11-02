// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
    // This is marked as a test class to make sure our AssemblyInitialize and AssemblyCleanup
    // fixtures get executed.  It won't actually host any tests.
    [TestClass]
    public class TestAssembly
    {
        public static TestApplicationInfo WinUICsUWPSampleApp
        {
            get
            {
                string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string baseDirectory = Path.Combine(Directory.GetParent(assemblyDir).Parent.Parent.Parent.Parent.FullName, "UITests.App");

                Log.Comment($"Base Package Search Directory = \"{baseDirectory}\"");

                var exclude = new[] { "Microsoft.NET.CoreRuntime", "Microsoft.VCLibs", "Microsoft.UI.Xaml", "Microsoft.NET.CoreFramework.Debug" };
                var files = Directory.GetFiles(baseDirectory, "*.msix", SearchOption.AllDirectories).Where(f => !exclude.Any(Path.GetFileNameWithoutExtension(f).Contains));

                if (files.Count() == 0)
                {
                    throw new Exception(string.Format("Failed to find '*.msix' in {0}'!", baseDirectory));
                }

                string mostRecentlyBuiltPackage = string.Empty;
                DateTime timeMostRecentlyBuilt = DateTime.MinValue;

                foreach (string file in files)
                {
                    DateTime fileWriteTime = File.GetLastWriteTime(file);

                    if (fileWriteTime > timeMostRecentlyBuilt)
                    {
                        timeMostRecentlyBuilt = fileWriteTime;
                        mostRecentlyBuiltPackage = file;
                    }
                }

                return new TestApplicationInfo(
                    "UITests.App",
                    "3568ebdf-5b6b-4ddd-bb17-462d614ba50f_gspb8g6x97k2t!App",
                    "3568ebdf-5b6b-4ddd-bb17-462d614ba50f_gspb8g6x97k2t",
                    "UITests.App",
                    "UITests.App.exe",
                    mostRecentlyBuiltPackage.Replace(".msix", string.Empty),
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

        [AssemblyInitialize]
        [TestProperty("CoreClrProfile", ".")]
        [TestProperty("RunFixtureAs:Assembly", "ElevatedUserOrSystem")]
        [TestProperty("RunAs", "User")]
        [TestProperty("Classification", "ScenarioTestSuite")]
        [TestProperty("Platform", "Any")]
        public static void AssemblyInitialize(TestContext testContext)
        {
            TestEnvironment.AssemblyInitialize(testContext, "UITests.App.pfx");
            TestEnvironment.Initialize(testContext, WinUICsUWPSampleApp);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            TestEnvironment.AssemblyCleanupWorker(WinUICsUWPSampleApp);
            TestEnvironment.AssemblyCleanup();
        }
    }
}