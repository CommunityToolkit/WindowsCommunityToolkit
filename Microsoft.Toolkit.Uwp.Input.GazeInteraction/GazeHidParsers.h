//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "GazeHidUsages.h"

using namespace winrt::Windows::Devices::HumanInterfaceDevice;
using namespace winrt::Windows::Devices::Input::Preview;

BEGIN_NAMESPACE_GAZE_INPUT

namespace GazeHidParsers {
	class GazeHidPosition sealed
	{
	public:
		long long X;
		long long Y;
		long long Z;
	};

	class GazeHidPositions sealed
	{
	public:
		GazeHidPosition LeftEyePosition;
		GazeHidPosition RightEyePosition;
		GazeHidPosition HeadPosition;
		GazeHidPosition HeadRotation;
	};

	class GazeHidPositionParser sealed
	{
	public:
		GazeHidPositionParser(GazeDevicePreview  gazeDevice, unsigned short usage);

		GazeHidPosition GetPosition(HidInputReport  report);

	private:
		HidNumericControlDescription  _X = nullptr;
		HidNumericControlDescription  _Y = nullptr;
		HidNumericControlDescription  _Z = nullptr;
		unsigned short _usage = 0x0000;
	};

	class GazeHidRotationParser sealed
	{
	public:
		GazeHidRotationParser(GazeDevicePreview  gazeDevice, unsigned short usage);

		GazeHidPosition GetRotation(const HidInputReport& report);

	private:
		HidNumericControlDescription _X;
		HidNumericControlDescription _Y;
		HidNumericControlDescription _Z;
		unsigned short _usage = 0x0000;
	};

	class GazeHidPositionsParser sealed
	{
	public:
		GazeHidPositionsParser(GazeDevicePreview  gazeDevice);

		GazeHidPositions GetGazeHidPositions(HidInputReport  report);

	private:
		GazeHidPositionParser  _leftEyePositionParser;
		GazeHidPositionParser  _rightEyePositionParser;
		GazeHidPositionParser  _headPositionParser;
		GazeHidRotationParser  _headRotationParser;
	};
}

END_NAMESPACE_GAZE_INPUT
