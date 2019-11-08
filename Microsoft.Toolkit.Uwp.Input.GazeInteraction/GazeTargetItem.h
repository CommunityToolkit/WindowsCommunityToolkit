//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "DwellProgressState.h"
#include "GazeInput.h"
#include "PointerState.h"
#include <DwellProgressState.h>

using namespace winrt::Windows::Foundation;
using namespace winrt::Microsoft::UI::Xaml;
using namespace winrt::Microsoft::UI::Xaml::Controls::Primitives;

BEGIN_NAMESPACE_GAZE_INPUT

class GazeTargetItem abstract
{
	TimeSpan DetailedTime() { return _detailedTime; }
	void DetailedTime(TimeSpan const& value) { _detailedTime = value; }
	TimeSpan OverflowTime() { return _overflowTime; }
	void OverflowTime(TimeSpan const& value) { _overflowTime = value; }
	TimeSpan ElapsedTime() { return DetailedTime() + OverflowTime(); }
	TimeSpan NextStateTime() { return _nextStateTime; }
	void NextStateTime(TimeSpan const& value) { _nextStateTime = value; }
	TimeSpan LastTimestamp() { return _lastTimestamp; }
	void LastTimestamp(TimeSpan const& value) { _lastTimestamp = value; }
	PointerState ElementState() { return _elementState; }
	void ElementState(PointerState const& value) { _elementState = value; }
	UIElement TargetElement() { return _targetElement; }
	void TargetElement(UIElement const& value) { _targetElement = value; }
	int RepeatCount() { return _repeatCount; }
	void RepeatCount(int const& value) { _repeatCount = value; }
	int MaxDwellRepeatCount() { return _maxDwellRepeatCount; }
	void MaxDwellRepeatCount(int const& value) { _maxDwellRepeatCount = value; }

	GazeTargetItem(UIElement target)
	{
		TargetElement(target);
	}

	static GazeTargetItem GetOrCreate(UIElement element);

	virtual void Invoke() = 0;

	virtual bool IsInvokable() { return true; }

	void Reset(TimeSpan nextStateTime)
	{
		ElementState(PointerState::PreEnter);
		DetailedTime(TimeSpanZero);
		OverflowTime(TimeSpanZero);
		NextStateTime(nextStateTime);
		RepeatCount(0);
		MaxDwellRepeatCount(GazeInput::GetMaxDwellRepeatCount(TargetElement()));
	}

	void GiveFeedback()
	{
		if (_nextStateTime != NextStateTime())
		{
			_prevStateTime = _nextStateTime;
			_nextStateTime = NextStateTime();
		}

		if (ElementState() != _notifiedPointerState)
		{
			switch (ElementState())
			{
			case PointerState::Enter:
				RaiseProgressEvent(DwellProgressState::Fixating);
				break;

			case PointerState::Dwell:
			case PointerState::Fixation:
				RaiseProgressEvent(DwellProgressState::Progressing);
				break;

			case PointerState::Exit:
			case PointerState::PreEnter:
				RaiseProgressEvent(DwellProgressState::Idle);
				break;
			}

			_notifiedPointerState = ElementState();
		}
		else if (ElementState() == PointerState::Dwell || ElementState() == PointerState::Fixation)
		{
			if (RepeatCount() <= MaxDwellRepeatCount())
			{
				RaiseProgressEvent(DwellProgressState::Progressing);
			}
			else
			{
				RaiseProgressEvent(DwellProgressState::Complete);
			}
		}
	}

private:

	TimeSpan _detailedTime;
	TimeSpan _overflowTime;
	TimeSpan _nextStateTime;
	TimeSpan _lastTimestamp;
	PointerState _elementState;
	UIElement _targetElement;
	int _repeatCount;
	int _maxDwellRepeatCount;

	void RaiseProgressEvent(DwellProgressState state);

	PointerState _notifiedPointerState = PointerState::Exit;
	TimeSpan _prevStateTime;
	TimeSpan _nextStateTime;
	DwellProgressState _notifiedProgressState = DwellProgressState::Idle;
	Popup _feedbackPopup;
};

END_NAMESPACE_GAZE_INPUT