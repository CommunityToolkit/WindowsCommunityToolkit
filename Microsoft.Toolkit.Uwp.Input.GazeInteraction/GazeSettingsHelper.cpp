//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeSettingsHelper.h"

using namespace concurrency;
using namespace Windows::ApplicationModel::AppService;
using namespace Windows::Foundation;

BEGIN_NAMESPACE_GAZE_INPUT

GazeSettingsHelper::GazeSettingsHelper()
{
}

Windows::Foundation::IAsyncAction^ GazeSettingsHelper::RetrieveSharedSettings(ValueSet^ settings)
{
    return create_async([settings]{
        // Setup a new app service connection
        AppServiceConnection^ connection = ref new AppServiceConnection();
        connection->AppServiceName = "com.microsoft.ectksettings";
        connection->PackageFamilyName = "Microsoft.EyeControlToolkitSettings_s9y1p3hwd5qda";

        // open the connection
        create_task(connection->OpenAsync()).then([settings, connection](AppServiceConnectionStatus status)
        {
            switch (status)
            {
            case AppServiceConnectionStatus::Success:
                // The new connection opened successfully
                // Set up the inputs and send a message to the service
                create_task(connection->SendMessageAsync(ref new ValueSet())).then([settings](AppServiceResponse^ response)
                {
                    switch (response->Status)
                    {
                    case AppServiceResponseStatus::Success:
                        for each (auto item in response->Message)
                        {
                            settings->Insert(item->Key, item->Value);
                        }
                        break;
                    default:
                    case AppServiceResponseStatus::Failure:
                    case AppServiceResponseStatus::ResourceLimitsExceeded:
                    case AppServiceResponseStatus::Unknown:
                        break;
                    }
                }); // create_task(connection->SendMessageAsync(inputs))
                break;

            default:
            case AppServiceConnectionStatus::AppNotInstalled:
            case AppServiceConnectionStatus::AppUnavailable:
            case AppServiceConnectionStatus::AppServiceUnavailable:
            case AppServiceConnectionStatus::Unknown:
                break;
            }
        }); // create_task(connection->OpenAsync())
    }); // create_async()
}

END_NAMESPACE_GAZE_INPUT
