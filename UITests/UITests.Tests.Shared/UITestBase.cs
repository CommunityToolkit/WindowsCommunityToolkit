// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Windows.Apps.Test.Foundation.Controls;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
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
    public abstract class UITestBase
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
                    testAppPackageName: "UITests.App",
                    testAppName: "3568ebdf-5b6b-4ddd-bb17-462d614ba50f_gspb8g6x97k2t!App",
                    testAppPackageFamilyName: "3568ebdf-5b6b-4ddd-bb17-462d614ba50f_gspb8g6x97k2t",
                    testAppMainWindowTitle: "UITests.App",
                    processName: "UITests.App.exe",
                    installerName: mostRecentlyBuiltPackage.Replace(".msix", string.Empty),
                    certSerialNumber: "24d62f3b13b8b9514ead9c4de48cc30f7cc6151d",
                    baseAppxDir: baseDirectory);
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

        public TestContext TestContext { get; set; }

        private AppServiceConnection CommunicationService { get; set; }

        [TestInitialize]
        public async Task TestInitialize()
        {
            PreTestSetup();

#if USING_TAEF
            var fullTestName = TestContext.TestName;
            var lastDotIndex = fullTestName.LastIndexOf('.');
            var testName = fullTestName.Substring(lastDotIndex + 1);
            var theClassName = fullTestName.Substring(0, lastDotIndex);
#else
            var testName = TestContext.TestName;
            var theClassName = TestContext.FullyQualifiedTestClassName;
#endif
            var currentlyRunningClassType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(f => f.FullName == theClassName);
            if (!(Type.GetType(theClassName) is Type type))
            {
                Verify.Fail("Type is null. TestClassName : " + theClassName);
                return;
            }

            if (!(type.GetMethod(testName) is MethodInfo method))
            {
                Verify.Fail("Mothod is null. TestClassName : " + theClassName + " Testname: " + testName);
                return;
            }

            if (!(method.GetCustomAttribute(typeof(TestPageAttribute), true) is TestPageAttribute attribute))
            {
                Verify.Fail("Attribute is null. TestClassName : " + theClassName);
                return;
            }

            var pageName = attribute.XamlFile;

            Log.Comment("[Harness] Sending Host Page Request: {0}", pageName);

            // Make the connection if we haven't already.
            if (CommunicationService == null)
            {
                CommunicationService = new AppServiceConnection();

                CommunicationService.RequestReceived += this.CommunicationService_RequestReceived;

                // Here, we use the app service name defined in the app service
                // provider's Package.appxmanifest file in the <Extension> section.
                CommunicationService.AppServiceName = "TestHarnessCommunicationService";

                // Use Windows.ApplicationModel.Package.Current.Id.FamilyName
                // within the app service provider to get this value.
                CommunicationService.PackageFamilyName = "3568ebdf-5b6b-4ddd-bb17-462d614ba50f_gspb8g6x97k2t";

                var status = await CommunicationService.OpenAsync();

                if (status != AppServiceConnectionStatus.Success)
                {
                    Log.Error("Failed to connect to App Service host.");
                    CommunicationService = null;
                    throw new Exception("Failed to connect to App Service host.");
                }
            }

            // Call the service.
            var message = new ValueSet();
            message.Add("Command", "Start");
            message.Add("Page", pageName);

            AppServiceResponse response = await CommunicationService.SendMessageAsync(message);
            string result = string.Empty;

            if (response.Status == AppServiceResponseStatus.Success)
            {
                // Get the data  that the service sent to us.
                if (response.Message["Status"] as string == "OK")
                {
                    Log.Comment("[Harness] Received Host Ready with Page: {0}", pageName);
                    Wait.ForIdle();
                    Log.Comment("[Harness] Starting Test for {0}...", pageName);
                    return;
                }
            }

            // Error case, we didn't get confirmation of test starting.
            throw new InvalidOperationException("Test host didn't confirm test ready to execute page: " + pageName);
        }

        private void CommunicationService_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            AppServiceDeferral messageDeferral = args.GetDeferral();
            ValueSet message = args.Request.Message;
            string cmd = message["Command"] as string;

            try
            {
                // Return the data to the caller.
                if (cmd == "Log")
                {
                    string level = message["Level"] as string;
                    string msg = message["Message"] as string;

                    switch (level)
                    {
                        case "Comment":
                            Log.Comment("[Host] {0}", msg);
                            break;
                        case "Warning":
                            Log.Warning("[Host] {0}", msg);
                            break;
                        case "Error":
                            Log.Error("[Host] {0}", msg);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Exception receiving message: {0}", e.Message);
            }
            finally
            {
                // Complete the deferral so that the platform knows that we're done responding to the app service call.
                // Note: for error handling: this must be called even if SendResponseAsync() throws an exception.
                messageDeferral.Complete();
            }
        }

        // This will reset the test for each run (as from original WinUI https://github.com/microsoft/microsoft-ui-xaml/blob/master/test/testinfra/MUXTestInfra/Infra/TestHelpers.cs)
        // We construct it so it doesn't try to run any tests since we use the AppService Bridge to complete
        // our loading.
        private void PreTestSetup()
        {
            _ = new TestSetupHelper(new string[] { }, new TestSetupHelper.TestSetupHelperOptions()
            {
                AutomationIdOfSafeItemToClick = null
            });
        }
    }
}
