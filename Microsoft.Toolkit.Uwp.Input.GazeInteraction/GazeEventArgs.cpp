//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeEventArgs.h"
#include "GazeEventArgs.g.cpp"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
	GazeEventArgs::GazeEventArgs() :
		m_handled{ false },
		m_location{},
		m_timestamp{}
	{
	}

	bool GazeEventArgs::Handled()
	{
		return m_handled;
	}

	void GazeEventArgs::Handled(bool value)
	{
		m_handled = value;
	}

	Windows::Foundation::Point GazeEventArgs::Location()
	{
		return m_location;
	}

	Windows::Foundation::TimeSpan GazeEventArgs::Timestamp()
	{
		return m_timestamp;
	}

	void GazeEventArgs::Set(Windows::Foundation::Point const& location, Windows::Foundation::TimeSpan const& timestamp)
	{
		m_handled = false;
		m_location = location;
		m_timestamp = timestamp;
	}
}
