//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "GazePointerState.h"
#include "GazeApi.h"
#include "GazeProgressState.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Devices::Enumeration;
using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::UI::Core;
using namespace Windows::Devices::Input::Preview;

namespace Shapes = Windows::UI::Xaml::Shapes;

BEGIN_NAMESPACE_GAZE_INPUT

ref struct GazeTargetItem sealed
{
    property int ElapsedTime;
    property int NextStateTime;
    property int64 LastTimestamp;
    property GazePointerState ElementState;
    property UIElement^ TargetElement;
    property int RepeatCount;
    property int MaxRepeatCount;

    GazeTargetItem(UIElement^ target)
    {
        TargetElement = target;
    }

    void Reset(int nextStateTime)
    {
        ElementState = GazePointerState::PreEnter;
        ElapsedTime = 0;
        NextStateTime = nextStateTime;
        RepeatCount = 0;
        MaxRepeatCount = GazeApi::GetMaxRepeatCount(TargetElement);
    }

    void GiveFeedback()
    {
        if (_nextStateTime != NextStateTime)
        {
            _prevStateTime = _nextStateTime;
            _nextStateTime = NextStateTime;
        }

        if (ElementState != _notifiedPointerState)
        {
            switch (ElementState)
            {
            case GazePointerState::Dwell:
                RaiseProgressEvent(GazeProgressState::Progressing);
                break;

            case GazePointerState::Exit:
            case GazePointerState::PreEnter:
                RaiseProgressEvent(GazeProgressState::Idle);
                break;
            }

            _notifiedPointerState = ElementState;
        }
        else if (ElementState == GazePointerState::Dwell)
        {
            if (RepeatCount <= MaxRepeatCount)
            {
                RaiseProgressEvent(GazeProgressState::Progressing);
            }
            else
            {
                RaiseProgressEvent(GazeProgressState::Complete);
            }
        }
    }

private:

    void RaiseProgressEvent(GazeProgressState state)
    {
        switch (state)
        {
        case GazeProgressState::Idle:
            if (_notifiedProgressState != state)
            {
                Debug::WriteLine(L"Now in Idle state");
            }
            break;
        case GazeProgressState::Progressing:
            Debug::WriteLine(L"Now progressing %f", ((double)(ElapsedTime - _prevStateTime)) / (_nextStateTime - _prevStateTime));
            break;
        case GazeProgressState::Complete:
            if (_notifiedProgressState != state)
            {
                Debug::WriteLine(L"Now complete");
            }
        }

        _notifiedProgressState = state;
    }

    GazePointerState _notifiedPointerState = GazePointerState::Exit;
    int64 _prevStateTime;
    int64 _nextStateTime;
    GazeProgressState _notifiedProgressState = GazeProgressState::Idle;
};

END_NAMESPACE_GAZE_INPUT