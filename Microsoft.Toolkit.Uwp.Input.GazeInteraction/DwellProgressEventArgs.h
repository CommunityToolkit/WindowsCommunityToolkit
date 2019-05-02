//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "DwellProgressState.h"

BEGIN_NAMESPACE_GAZE_INPUT

/// <summary>
/// This parameter is passed to the GazeElement.DwellProgressFeedback event. The event is fired to inform the application of the user's progress towards completing dwelling on a control
/// </summary>
public ref class DwellProgressEventArgs : public RoutedEventArgs
{
public:

    /// <summary>
    /// An enum that reflects the current state of dwell progress
    /// </summary>
    property DwellProgressState State { DwellProgressState get() { return _state; }}

    /// <summary>
    /// A value between 0 and 1 that reflects the fraction of progress towards completing dwell
    /// </summary>
    property double Progress { double get() { return _progress; }}

    /// <summary>
    /// A parameter for the application to set to true if it handles the event. If this parameter is set to true, the library suppresses default animation for dwell feedback on the control
    /// </summary>
    property bool Handled;

internal:
	DwellProgressEventArgs(DwellProgressState state, TimeSpan elapsedDuration, TimeSpan triggerDuration)
	{
        _state = state;
        _progress = ((double)elapsedDuration.Duration) / triggerDuration.Duration;
	}
private:
    DwellProgressState _state;
    double _progress;
};

END_NAMESPACE_GAZE_INPUT