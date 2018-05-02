//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "DwellInvokedRoutedEventArgs.h"
#include "DwellProgressEventArgs.h"
#include "StateChangedEventArgs.h"

using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;

BEGIN_NAMESPACE_GAZE_INPUT

/// <summary>
/// Surrogate object attached to controls allowing subscription to per-control gaze events.
/// </summary>
public ref class GazeElement sealed : public DependencyObject
{
public:

    /// <summary>
    /// This event is fired when the state of the user's gaze on a control has changed
    /// </summary>
    event EventHandler<StateChangedEventArgs^>^ StateChanged;

    /// <summary>
    /// This event is fired when the user completed dwelling on a control and the control is about to be invoked by default. This event is fired to give the application an opportunity to prevent default invocation
    /// </summary>
    event EventHandler<DwellInvokedRoutedEventArgs^>^ Invoked;

    /// <summary>
    /// This event is fired to inform the application of the progress towards dwell
    /// </summary>
    event EventHandler<DwellProgressEventArgs^>^ DwellProgressFeedback;

internal:

    void RaiseStateChanged(Object^ sender, StateChangedEventArgs^ args) { StateChanged(sender, args); }

    void RaiseInvoked(Object^ sender, DwellInvokedRoutedEventArgs^ args)
    {
        Invoked(sender, args);
    }

    bool RaiseProgressFeedback(Object^ sender, DwellProgressState state, TimeSpan elapsedTime, TimeSpan triggerTime)
    {
        auto args = ref new DwellProgressEventArgs(state, elapsedTime, triggerTime);
        DwellProgressFeedback(sender, args);
        return args->Handled;
    }
};

END_NAMESPACE_GAZE_INPUT