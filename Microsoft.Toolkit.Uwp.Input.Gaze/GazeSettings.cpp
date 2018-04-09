//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeSettings.h"

using namespace concurrency;
using namespace Windows::ApplicationModel::AppService;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Data::Json;

BEGIN_NAMESPACE_GAZE_INPUT

GazeSettings::GazeSettings()
{
    // from OneEuroFilter.h
    _oneEurofilter_beta = EUROFILTER_DEFAULT_BETA;
    _oneEurofilter_cutoff = EUROFILTER_DEFAULT_CUTOFF;
    _oneEurofilter_velocity_cutoff = EUROFILTER_DEFAULT_VELOCITY_CUTOFF;

    // from GazePointer.h
    _gazePointer_fixation_delay = DEFAULT_FIXATION_DELAY;
    _gazePointer_dwell_delay = DEFAULT_DWELL_DELAY;
    _gazePointer_repeat_delay = DEFAULT_REPEAT_DELAY;
    _gazePointer_enter_exit_delay = DEFAULT_ENTER_EXIT_DELAY;
    _gazePointer_max_history_duration = DEFAULT_MAX_HISTORY_DURATION;
    _gazePointer_max_single_sample_duration = MAX_SINGLE_SAMPLE_DURATION;
    _gazePointer_gaze_idle_time = GAZE_IDLE_TIME;

    // from GazeCursor.h
    _gazeCursor_cursor_radius = DEFAULT_CURSOR_RADIUS;
    _gazeCursor_cursor_visibility = DEFAULT_CURSOR_VISIBILITY;
}

GazeSettings::~GazeSettings()
{
}

Windows::Foundation::IAsyncAction^ GazeSettings::RetrieveSharedSettings(GazeSettings^ gazeSettings)
{
    return create_async([gazeSettings]{
        // Setup a new app service connection
        AppServiceConnection^ connection = ref new AppServiceConnection();
        connection->AppServiceName = "com.microsoft.ectksettings";
        connection->PackageFamilyName = "Microsoft.EyeControlToolkitSettings_s9y1p3hwd5qda";

        // open the connection
        create_task(connection->OpenAsync()).then([gazeSettings, connection](AppServiceConnectionStatus status)
        {
            switch (status)
            {
            case AppServiceConnectionStatus::Success:
                // The new connection opened successfully
                // Set up the inputs and send a message to the service
                create_task(connection->SendMessageAsync(ref new ValueSet())).then([gazeSettings](AppServiceResponse^ response)
                {
                    switch (response->Status)
                    {
                    case AppServiceResponseStatus::Success:
                        {
                            auto message = response->Message;

                            // TODO Consider serializing the settings object so it can simply be rehydrated.
                            gazeSettings->OneEuroFilter_Beta = (float)(message->Lookup("OneEuroFilter_Beta"));
                            gazeSettings->OneEuroFilter_Cutoff = (float)(message->Lookup("OneEuroFilter_Cutoff"));
                            gazeSettings->OneEuroFilter_Velocity_Cutoff = (float)(message->Lookup("OneEuroFilter_Velocity_Cutoff"));

                            gazeSettings->GazePointer_Fixation_Delay = (int)(message->Lookup("GazePointer_Fixation_Delay"));
                            gazeSettings->GazePointer_Dwell_Delay = (int)(message->Lookup("GazePointer_Dwell_Delay"));
                            gazeSettings->GazePointer_Repeat_Delay = (int)(message->Lookup("GazePointer_Repeat_Delay"));
                            gazeSettings->GazePointer_Enter_Exit_Delay = (int)(message->Lookup("GazePointer_Enter_Exit_Delay"));
                            gazeSettings->GazePointer_Max_History_Duration = (int)(message->Lookup("GazePointer_Max_History_Duration"));
                            gazeSettings->GazePointer_Max_Single_Sample_Duration = (int)(message->Lookup("GazePointer_Max_Single_Sample_Duration"));
                            gazeSettings->GazePointer_Gaze_Idle_Time = (int)(message->Lookup("GazePointer_Gaze_Idle_Time"));

                            gazeSettings->GazeCursor_Cursor_Radius = (int)(message->Lookup("GazeCursor_Cursor_Radius"));
                            gazeSettings->GazeCursor_Cursor_Visibility = (bool)(message->Lookup("GazeCursor_Cursor_Visibility"));
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
