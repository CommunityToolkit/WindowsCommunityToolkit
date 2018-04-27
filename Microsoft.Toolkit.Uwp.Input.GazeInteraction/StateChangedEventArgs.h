//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "IGazeFilter.h"
#include "GazeCursor.h"
#include "PointerState.h"

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

public ref struct StateChangedEventArgs sealed
{
    property UIElement^ HitTarget {UIElement^ get() { return _hitTarget; }}
    property GazeInteraction::PointerState PointerState {GazeInteraction::PointerState get() { return _pointerState; }}
    property TimeSpan ElapsedTime {TimeSpan get() { return _elapsedTime; }}

internal:

    StateChangedEventArgs(UIElement^ target, GazeInteraction::PointerState state, TimeSpan elapsedTime)
    {
        _hitTarget = target;
        _pointerState = state;
        _elapsedTime = elapsedTime;
    }

private:
    UIElement ^ _hitTarget;
    GazeInteraction::PointerState _pointerState;
    TimeSpan _elapsedTime;
};

END_NAMESPACE_GAZE_INPUT