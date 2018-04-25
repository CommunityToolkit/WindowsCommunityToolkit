//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "PointerState.h"
#include "GazeInput.h"
#include "GazeProgressState.h"

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

ref struct GazeTargetItem
{
internal:
    static property GazeTargetItem^ NonInvokable{ GazeTargetItem^ get(); }

	property TimeSpan DetailedTime;
	property TimeSpan OverflowTime;
	property TimeSpan ElapsedTime { TimeSpan get() { return DetailedTime + OverflowTime; } }
	property TimeSpan NextStateTime;
	property TimeSpan LastTimestamp;
	property PointerState ElementState;
	property UIElement^ TargetElement;
	property int RepeatCount;
	property int MaxDwellRepeatCount;

	GazeTargetItem(UIElement^ target)
	{
		TargetElement = target;
	}

    static GazeTargetItem^ GetOrCreate(UIElement^ element);

    virtual void Invoke() = 0;

    virtual property bool IsInvokable { bool get() { return true; } }

	void Reset(TimeSpan nextStateTime)
	{
		ElementState = PointerState::PreEnter;
		DetailedTime = TimeSpanZero;
		OverflowTime = TimeSpanZero;
		NextStateTime = nextStateTime;
		RepeatCount = 0;
		MaxDwellRepeatCount = GazeInput::GetMaxDwellRepeatCount(TargetElement);
	}

	void GiveFeedback()
	{
		if (_nextStateTime != NextStateTime)
		{
			_prevStateTime = _nextStateTime;
			_nextStateTime = NextStateTime;
		}

		if (ElementState != _notifiedPointerState)
		{
			switch (ElementState)
			{
			case PointerState::Dwell:
			case PointerState::Fixation:
				RaiseProgressEvent(GazeProgressState::Progressing);
				break;

			case PointerState::Exit:
			case PointerState::PreEnter:
				RaiseProgressEvent(GazeProgressState::Idle);
				break;
			}

			_notifiedPointerState = ElementState;
		}
		else if (ElementState == PointerState::Dwell || ElementState == PointerState::Fixation)
		{
			if (RepeatCount <= MaxDwellRepeatCount)
			{
				RaiseProgressEvent(GazeProgressState::Progressing);
			}
			else
			{
				RaiseProgressEvent(GazeProgressState::Complete);
			}
		}
	}

private:

	void RaiseProgressEvent(GazeProgressState state);

	PointerState _notifiedPointerState = PointerState::Exit;
	TimeSpan _prevStateTime;
	TimeSpan _nextStateTime;
	GazeProgressState _notifiedProgressState = GazeProgressState::Idle;
	Popup^ _feedbackPopup;
};

END_NAMESPACE_GAZE_INPUT