// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.UI.Xaml.Tests.MUXControls.InteractionTests.Common;
using Microsoft.UI.Xaml.Tests.MUXControls.InteractionTests.Infra;
using Microsoft.Windows.Apps.Test.Foundation.Controls;
using UITests.App.Protos;
using Windows.Foundation.Collections;

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
        public static TestApplicationInfo UITestsAppSampleApp
        {
            get
            {
                string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string baseDirectory = Path.Combine(Directory.GetParent(assemblyDir).Parent.Parent.Parent.Parent.FullName, "UITests.App.Package");

                Log.Comment($"Base Package Search Directory = \"{baseDirectory}\"");

#if USING_TAEF
                string testAppName = "3568ebdf-5b6b-4ddd-bb17-462d614ba50f_gspb8g6x97k2t!App";
                string installerName = "UITests.App";
#else
                var exclude = new[] { "Microsoft.ProjectReunion", "Microsoft.VCLibs" };
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

                string testAppName = "3568ebdf-5b6b-4ddd-bb17-462d614ba50f_gspb8g6x97k2t!App";
                string installerName = mostRecentlyBuiltPackage.Replace(".msix", string.Empty);
#endif

                return new TestApplicationInfo(
                    testAppPackageName: "UITests.App",
                    testAppName: testAppName,
                    testAppPackageFamilyName: "3568ebdf-5b6b-4ddd-bb17-462d614ba50f_gspb8g6x97k2t",
                    testAppMainWindowTitle: "UITests.App",
                    processName: "UITests.App.exe",
                    installerName: installerName,
                    isUwpApp: false,
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

        private GrpcChannel _channel;

        private AppService.AppServiceClient _communicationService;

        private CancellationTokenSource _subscribeLogTokenSource;
        private Task _subscribeLogTask;
        private AsyncServerStreamingCall<LogUpdate> _logStream;

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
            if (_channel == null)
            {
                Log.Comment("[Harness] Trying to connect...");

                _channel = GrpcChannel.ForAddress(
                    "https://localhost:5001",
                    new GrpcChannelOptions
                    {
                        HttpHandler = new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        }
                    });

                Log.Comment("[Harness] Trying to connect 2...");

                _communicationService = new AppService.AppServiceClient(_channel);

                Log.Comment("[Harness] Connected!");

                _logStream = _communicationService.SubscribeLog(new SubscribeLogRequest());

                _subscribeLogTokenSource = new CancellationTokenSource();

                static async Task SubscribeLog(IAsyncStreamReader<LogUpdate> stream, CancellationToken token)
                {
                    try
                    {
                        await foreach (var update in stream.ReadAllAsync(token))
                        {
                            switch (update.Level)
                            {
                                case "Comment":
                                    Log.Comment("[Host] {0}", update.Message);
                                    break;
                                case "Warning":
                                    Log.Warning("[Host] {0}", update.Message);
                                    break;
                                case "Error":
                                    Log.Error("[Host] {0}", update.Message);
                                    break;
                            }
                        }
                    }
                    catch (RpcException e) when (e.StatusCode == StatusCode.Cancelled)
                    {
                        return;
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Finished.");
                    }
                }

                _subscribeLogTask = SubscribeLog(_logStream.ResponseStream, _subscribeLogTokenSource.Token);
            }

            Log.Comment("[Harness] Calling start!");

            // Call the service.
            var response = await _communicationService.StartAsync(new StartRequest
            {
                PageName = pageName
            });
            string result = string.Empty;

            // Get the data  that the service sent to us.
            if (response.Status == "OK")
            {
                Log.Comment("[Harness] Received Host Ready with Page: {0}", pageName);
                Wait.ForIdle();
                Log.Comment("[Harness] Starting Test for {0}...", pageName);
                return;
            }

            // Error case, we didn't get confirmation of test starting.
            throw new InvalidOperationException("Test host didn't confirm test ready to execute page: " + pageName);
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            _subscribeLogTokenSource.Cancel();
            await _subscribeLogTask;
            _logStream.Dispose();
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