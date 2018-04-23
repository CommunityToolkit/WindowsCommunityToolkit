//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "IGazeFilter.h"
#include "GazeCursor.h"
#include "GazePointerState.h"

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

public ref struct GazePointerEventArgs sealed
{
    property UIElement^ HitTarget;
    property GazePointerState PointerState;
    property int64 ElapsedTime;
    property TimeSpan ElapsedTimeSpan { TimeSpan get() { return *new TimeSpan{ 10 * ElapsedTime }; } }

    GazePointerEventArgs(UIElement^ target, GazePointerState state, int64 elapsedTime)
    {
        HitTarget = target;
        PointerState = state;
        ElapsedTime = elapsedTime;
    }
};

END_NAMESPACE_GAZE_INPUT