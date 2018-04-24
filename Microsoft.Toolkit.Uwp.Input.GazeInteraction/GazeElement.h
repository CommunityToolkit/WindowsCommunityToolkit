//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "IGazeFilter.h"
#include "GazeCursor.h"
#include "StateChangedEventArgs.h"
#include "GazeInvokedRoutedEventArgs.h"
#include "GazeProgressEventArgs.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Devices::Enumeration;
using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::UI::Core;
using namespace Windows::Devices::Input::Preview;

BEGIN_NAMESPACE_GAZE_INPUT


public ref class GazeElement sealed : public DependencyObject
{
private:
    static DependencyProperty^ const s_hasAttentionProperty;
    static DependencyProperty^ const s_invokeProgressProperty;
public:
    static property DependencyProperty^ HasAttentionProperty { DependencyProperty^ get() { return s_hasAttentionProperty; } }
    static property DependencyProperty^ InvokeProgressProperty { DependencyProperty^ get() { return s_invokeProgressProperty; } }

    event EventHandler<StateChangedEventArgs^>^ StateChanged;
    event EventHandler<GazeInvokedRoutedEventArgs^>^ Invoked;
	event EventHandler<GazeProgressEventArgs^>^ ProgressFeedback;

    void RaiseStateChanged(Object^ sender, StateChangedEventArgs^ args) { StateChanged(sender, args); }

    void RaiseInvoked(Object^ sender, GazeInvokedRoutedEventArgs^ args)
    {
        Invoked(sender, args);
    }

	bool RaiseProgressFeedback(Object^ sender, GazeProgressState state, TimeSpan elapsedTime, TimeSpan triggerTime)
	{
		auto args = ref new GazeProgressEventArgs(state, elapsedTime, triggerTime);
		ProgressFeedback(sender, args);
		return args->Handled;
	}
};

END_NAMESPACE_GAZE_INPUT