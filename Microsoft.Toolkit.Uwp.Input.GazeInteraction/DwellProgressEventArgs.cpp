//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "DwellProgressEventArgs.h"
#include "DwellProgressEventArgs.g.cpp"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
	DwellProgressEventArgs::DwellProgressEventArgs(Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellProgressState const& state, Windows::Foundation::TimeSpan const& elapsedDuration, Windows::Foundation::TimeSpan const& triggerDuration) :
		_state{ state },
		_progress{ static_cast<double>(elapsedDuration / triggerDuration) }
	{
	}

	Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellProgressState DwellProgressEventArgs::State()
	{
		return _state;
	}

	double DwellProgressEventArgs::Progress()
	{
		return _progress;
	}

	bool DwellProgressEventArgs::Handled()
	{
		return _handled;
	}

	void DwellProgressEventArgs::Handled(bool value)
	{
		_handled = value;
	}
}
