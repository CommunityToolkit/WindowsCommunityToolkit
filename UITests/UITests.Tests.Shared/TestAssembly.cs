// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
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
        private static AppServiceConnection CommunicationService { get; set; }

        [AssemblyInitialize]
        [TestProperty("CoreClrProfile", ".")]
        [TestProperty("RunFixtureAs:Assembly", "ElevatedUserOrSystem")]
        public static void AssemblyInitialize(TestContext testContext)
        {
            TestEnvironment.AssemblyInitialize(testContext, "UITests.App.pfx");
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            TestEnvironment.AssemblyCleanupWorker(UITestBase.WinUICsUWPSampleApp);
        }

        private static async Task InitalizeComService()
        {
            CommunicationService = new AppServiceConnection();

            CommunicationService.RequestReceived += CommunicationService_RequestReceived;
            CommunicationService.ServiceClosed += CommunicationService_ServiceClosed;

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

        private static void CommunicationService_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            Log.Warning("[Harness] Communication Service Closed! AppServiceClosedStatus: {0}", args.Status.ToString());
        }

        internal static Task<bool> OpenPage(string pageName)
        {
            Log.Comment("[Harness] Sending Host Page Request: {0}", pageName);

            return SendMessageToApp(new()
            {
                { "Command", "OpenPage" },
                { "Page", pageName }
            });
        }

        internal static async Task<int?> TryGetHostDpi()
        {
            Log.Comment("[Harness] Requesting Dpi form host...");
            var response = await SendCustomMessageToApp(new() { { "Command", "Get" }, { "Key", "Dpi" } });

            Log.Comment("[Harness] Response AppServiceResponseStatus({0})", response.Status.ToString());

            return response.Status == AppServiceResponseStatus.Success
                && TryGetValueAndLog(response.Message, "Status", out string status) && status == "OK"
                && TryGetValueAndLog(response.Message, "Dpi", out int val)
                ? val
                : null;
        }

        internal static async Task<AppServiceResponse> SendCustomMessageToApp(ValueSet message)
        {
            if (CommunicationService is null)
            {
                await InitalizeComService();
            }

            return await CommunicationService.SendMessageAsync(message);
        }

        private static async Task<bool> SendMessageToApp(ValueSet message)
        {
            if (CommunicationService is null)
            {
                await InitalizeComService();
            }

            var response = await CommunicationService.SendMessageAsync(message);

            return CheckResponseStatusOK(response);
        }

        internal static bool CheckResponseStatusOK(AppServiceResponse response)
        {
            Log.Comment("[Harness] Response AppServiceResponseStatus({0})", response.Status.ToString());

            return response.Status == AppServiceResponseStatus.Success
                && TryGetValueAndLog(response.Message, "Status", out string val)
                && val == "OK";
        }

        private static bool TryGetValueAndLog<T>(ValueSet message, string key, out T value)
        {
            value = default;

            if(!message.TryGetValue(key, out var oVal))
            {
                Log.Comment("[Harness] Reponse Message did not contain key {0}", key);
                return false;
            }

            if(oVal is not T tVal)
            {
                Log.Comment("[Harness] Reponse Message contained key {0}, but the value was not of expected type {1}. Value : ", key, nameof(T));
                return false;
            }

            value = tVal;
            Log.Comment("[Harness] Reponse Message contained key {0} with value {1}", key, tVal.ToString());
            return true;
        }

        private static void CommunicationService_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var messageDeferral = args.GetDeferral();
            var message = args.Request.Message;
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
    }
}