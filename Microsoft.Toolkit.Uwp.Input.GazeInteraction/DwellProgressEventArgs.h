//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "DwellProgressState.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Devices::Enumeration;
using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::UI::Core;
using namespace Windows::Devices::Input::Preview;

BEGIN_NAMESPACE_GAZE_INPUT

public ref class DwellProgressEventArgs : public RoutedEventArgs
{
public:
	property DwellProgressState State;
	property TimeSpan ElapsedDuration;
	property TimeSpan TriggerDuration;
	property bool Handled;
internal:
	DwellProgressEventArgs(DwellProgressState state, TimeSpan elapsedDuration, TimeSpan triggerDuration)
	{
		State = state;
		ElapsedDuration = elapsedDuration;
		TriggerDuration = triggerDuration;
	}
};

END_NAMESPACE_GAZE_INPUT