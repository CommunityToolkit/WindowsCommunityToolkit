//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "PointerState.h"

using namespace winrt::Microsoft::UI::Xaml;

BEGIN_NAMESPACE_GAZE_INPUT

/// <summary>
/// This parameter is passed to the StateChanged event.
/// </summary>
struct StateChangedEventArgs sealed
{
public:
    /// <summary>
    /// The state of user's gaze with respect to a control
    /// </summary>
    PointerState PointerState() { return _pointerState; }

    /// <summary>
    /// Elapsed time since the last state
    /// </summary>
    TimeSpan ElapsedTime () { return _elapsedTime; }

    StateChangedEventArgs() { }

    StateChangedEventArgs(UIElement target, PointerState state, TimeSpan elapsedTime)
    {
        _hitTarget = target;
        _pointerState = state;
        _elapsedTime = elapsedTime;
    }

private:
    UIElement _hitTarget;
    PointerState _pointerState;
    TimeSpan _elapsedTime;
};

END_NAMESPACE_GAZE_INPUT