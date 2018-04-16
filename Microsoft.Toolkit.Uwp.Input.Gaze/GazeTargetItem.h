//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "GazePointerState.h"
#include "GazeApi.h"

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
};

END_NAMESPACE_GAZE_INPUT