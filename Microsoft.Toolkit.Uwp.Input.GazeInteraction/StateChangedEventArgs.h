//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "PointerState.h"

using namespace Windows::UI::Xaml;

BEGIN_NAMESPACE_GAZE_INPUT

/// <summary>
/// TODO: harishsk
/// </summary>
public ref struct StateChangedEventArgs sealed
{
    /// <summary>
    /// TODO: harishsk
    /// </summary>
    property UIElement^ HitTarget {UIElement^ get() { return _hitTarget; }}

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    property GazeInteraction::PointerState PointerState {GazeInteraction::PointerState get() { return _pointerState; }}

    /// <summary>
    /// TODO: harishsk
    /// </summary>
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