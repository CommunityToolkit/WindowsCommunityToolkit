//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

BEGIN_NAMESPACE_GAZE_INPUT

#pragma region OneEuroFilter
const float EUROFILTER_DEFAULT_BETA = 5.0f;
const float EUROFILTER_DEFAULT_CUTOFF = 0.1f;
const float EUROFILTER_DEFAULT_VELOCITY_CUTOFF = 1.0f;
#pragma endregion OneEuroFilter

#pragma region GazePointer
// units in microseconds
const int DEFAULT_FIXATION_DELAY = 400000;
const int DEFAULT_DWELL_DELAY = 800000;
const int DEFAULT_REPEAT_DELAY = MAXINT;
const int DEFAULT_ENTER_EXIT_DELAY = 50000;
const int DEFAULT_MAX_HISTORY_DURATION = 3000000;
const int MAX_SINGLE_SAMPLE_DURATION = 100000;

const int GAZE_IDLE_TIME = 2500000;
#pragma endregion GazePointer

#pragma region GazeCursor
const int DEFAULT_CURSOR_RADIUS = 5;
const bool DEFAULT_CURSOR_VISIBILITY = true;
#pragma endregion GazePointer

public ref class GazeSettings sealed
{
public:

    static property GazeSettings^ Instance
    {
        GazeSettings^ get()
        {
            static GazeSettings^ instance = ref new GazeSettings();
            return instance;
        }
    }

    virtual ~GazeSettings();

    static Windows::Foundation::IAsyncAction^ RetrieveSharedSettings(GazeSettings^ gazeSettings);

#pragma region OneEuroFilter
    property float OneEuroFilter_Beta
    {
        float get() { return _oneEurofilter_beta; }
        void set(float value)
        {
            _oneEurofilter_beta = value;
        }
    }

    property float OneEuroFilter_Cutoff
    {
        float get() { return _oneEurofilter_cutoff; }
        void set(float value)
        {
            _oneEurofilter_cutoff = value;
        }
    }

    property float OneEuroFilter_Velocity_Cutoff
    {
        float get() { return _oneEurofilter_velocity_cutoff; }
        void set(float value)
        {
            _oneEurofilter_velocity_cutoff = value;
        }
    }
#pragma endregion OneEuroFilter

#pragma region GazePointer
    property int GazePointer_Fixation_Delay
    {
        int get() { return _gazePointer_fixation_delay; }
        void set(int value)
        {
            _gazePointer_fixation_delay = value;
        }
    }

    property int GazePointer_Dwell_Delay
    {
        int get() { return _gazePointer_dwell_delay; }
        void set(int value)
        {
            _gazePointer_dwell_delay = value;
        }
    }

    property int GazePointer_Repeat_Delay
    {
        int get() { return _gazePointer_repeat_delay; }
        void set(int value)
        {
            _gazePointer_repeat_delay = value;
        }
    }

    property int GazePointer_Enter_Exit_Delay
    {
        int get() { return _gazePointer_enter_exit_delay; }
        void set(int value)
        {
            _gazePointer_enter_exit_delay = value;
        }
    }

    property int GazePointer_Max_History_Duration
    {
        int get() { return _gazePointer_max_history_duration; }
        void set(int value)
        {
            _gazePointer_max_history_duration = value;
        }
    }

    property int GazePointer_Max_Single_Sample_Duration
    {
        int get() { return _gazePointer_max_single_sample_duration; }
        void set(int value)
        {
            _gazePointer_max_single_sample_duration = value;
        }
    }

    property int GazePointer_Gaze_Idle_Time
    {
        int get() { return _gazePointer_gaze_idle_time; }
        void set(int value)
        {
            _gazePointer_gaze_idle_time = value;
        }
    }
#pragma endregion GazePointer

#pragma region GazeCursor
    property int GazeCursor_Cursor_Radius
    {
        int get() { return _gazeCursor_cursor_radius; }
        void set(int value)
        {
            _gazeCursor_cursor_radius = value;
        }
    }

    property bool GazeCursor_Cursor_Visibility
    {
        bool get() { return _gazeCursor_cursor_visibility; }
        void set(bool value)
        {
            _gazeCursor_cursor_visibility = value;
        }
    }
#pragma endregion GazeCursor

private:
    GazeSettings();

    // from OneEuroFilter.h
    float _oneEurofilter_beta;
    float _oneEurofilter_cutoff;
    float _oneEurofilter_velocity_cutoff;

    // from GazePointer.h
    int _gazePointer_fixation_delay;
    int _gazePointer_dwell_delay;
    int _gazePointer_repeat_delay;
    int _gazePointer_enter_exit_delay;
    int _gazePointer_max_history_duration;
    int _gazePointer_max_single_sample_duration;
    int _gazePointer_gaze_idle_time;

    // from GazeCursor.h
    int _gazeCursor_cursor_radius;
    bool _gazeCursor_cursor_visibility;
};

END_NAMESPACE_GAZE_INPUT
