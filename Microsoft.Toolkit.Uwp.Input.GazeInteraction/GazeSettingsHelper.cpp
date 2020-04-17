//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeSettingsHelper.h"

#include <winrt/Windows.ApplicationModel.AppService.h>

using namespace winrt::Windows::ApplicationModel::AppService;
using namespace winrt::Windows::Foundation;

BEGIN_NAMESPACE_GAZE_INPUT

GazeSettingsHelper::GazeSettingsHelper()
{
}

winrt::Windows::Foundation::IAsyncAction GazeSettingsHelper::RetrieveSharedSettings(ValueSet settings)
{
    // Setup a new app service connection
    AppServiceConnection connection;
    connection.AppServiceName(L"com.microsoft.ectksettings");
    connection.PackageFamilyName(L"Microsoft.EyeControlToolkitSettings_s9y1p3hwd5qda");

    // open the connection

    AppServiceConnectionStatus status{ co_await connection.OpenAsync() };
    AppServiceResponse response = nullptr;
    switch (status)
    {
        case AppServiceConnectionStatus::Success:
            // The new connection opened successfully
            // Set up the inputs and send a message to the service
            response = co_await connection.SendMessageAsync(ValueSet());
            break;

        default:
        case AppServiceConnectionStatus::AppNotInstalled:
        case AppServiceConnectionStatus::AppUnavailable:
        case AppServiceConnectionStatus::AppServiceUnavailable:
        case AppServiceConnectionStatus::Unknown:
            break;
    }
    if (response == nullptr)
    {
        co_return;
    }

    switch (response.Status())
    {
        case AppServiceResponseStatus::Success:
            for (auto item : response.Message())
            {
                settings.Insert(item.Key(), item.Value());
            }
            break;
        default:
        case AppServiceResponseStatus::Failure:
        case AppServiceResponseStatus::ResourceLimitsExceeded:
        case AppServiceResponseStatus::Unknown:
            break;
    }
}

END_NAMESPACE_GAZE_INPUT
