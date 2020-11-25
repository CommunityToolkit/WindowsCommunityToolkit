// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace UITests.App
{
    /// <summary>
    /// This file contains part of the app related to the AppService for communication between this test host and the test harness processes.
    /// </summary>
    public sealed partial class App
    {
        private AppServiceConnection _appServiceConnection;
        private BackgroundTaskDeferral _appServiceDeferral;

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);
            IBackgroundTaskInstance taskInstance = args.TaskInstance;
            AppServiceTriggerDetails appService = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            _appServiceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnAppServicesCanceled;
            _appServiceConnection = appService.AppServiceConnection;
            _appServiceConnection.RequestReceived += OnAppServiceRequestReceived;
            _appServiceConnection.ServiceClosed += AppServiceConnection_ServiceClosed;
        }

        private async void OnAppServiceRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            AppServiceDeferral messageDeferral = args.GetDeferral();
            ValueSet message = args.Request.Message;
            string cmd = message["Command"] as string;

            try
            {
                // Return the data to the caller.
                if (cmd == "Start")
                {
                    var pageName = message["Page"] as string;

                    Debug.WriteLine("[Host] Received request for Page: " + pageName);

                    ValueSet returnMessage = new ValueSet();
                    returnMessage.Add("Status", "OK");
                    await args.Request.SendResponseAsync(returnMessage);
                }
            }
            catch (Exception e)
            {
                // Your exception handling code here.
            }
            finally
            {
                // Complete the deferral so that the platform knows that we're done responding to the app service call.
                // Note: for error handling: this must be called even if SendResponseAsync() throws an exception.
                messageDeferral.Complete();
            }
        }

        private void OnAppServicesCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _appServiceDeferral.Complete();
        }

        private void AppServiceConnection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            _appServiceDeferral.Complete();
        }
    }
}
