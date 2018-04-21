//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "GazeProgressState.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Devices::Enumeration;
using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::UI::Core;
using namespace Windows::Devices::Input::Preview;

BEGIN_NAMESPACE_GAZE_INPUT

public ref class GazeProgressEventArgs : public RoutedEventArgs
{
public:
	property GazeProgressState State;
	property int64 ElapsedTicks;
	property int64 TriggerTicks;
	property bool Handled;
internal:
	GazeProgressEventArgs(GazeProgressState state, int64 elapsedTicks, int64 triggerTicks)
	{
		State = state;
		ElapsedTicks = elapsedTicks;
		TriggerTicks = triggerTicks;
	}
};

END_NAMESPACE_GAZE_INPUT