//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "IGazeFilter.h"
#include "GazeCursor.h"
#include "StateChangedEventArgs.h"
#include "DwellInvokedRoutedEventArgs.h"
#include "DwellProgressEventArgs.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Devices::Enumeration;
using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::UI::Core;
using namespace Windows::Devices::Input::Preview;

BEGIN_NAMESPACE_GAZE_INPUT


/// <summary>
/// TODO: harishsk
/// </summary>
public ref class GazeElement sealed : public DependencyObject
{
public:

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    event EventHandler<StateChangedEventArgs^>^ StateChanged;

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    event EventHandler<DwellInvokedRoutedEventArgs^>^ Invoked;

    /// <summary>
    /// TODO: harishsk
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