//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "GazeHidUsages.h"

using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::Devices::Input::Preview;

BEGIN_NAMESPACE_GAZE_INPUT

namespace GazeHidParsers {
    public ref class GazeHidPosition sealed
    {
    public:
        property long long X;
        property long long Y;
        property long long Z;
    };

    public ref class GazeHidPositions sealed
    {
    public:
        property GazeHidPosition^ LeftEyePosition;
        property GazeHidPosition^ RightEyePosition;
        property GazeHidPosition^ HeadPosition;
        property GazeHidPosition^ HeadRotation;
    };

    public ref class GazeHidPositionParser sealed
    {
    public:
        GazeHidPositionParser(GazeDevicePreview ^ gazeDevice, uint16 usage);

        GazeHidPosition^ GetPosition(HidInputReport ^ report);

    private:
        HidNumericControlDescription ^ _X = nullptr;
        HidNumericControlDescription ^ _Y = nullptr;
        HidNumericControlDescription ^ _Z = nullptr;
        uint16 _usage                     = 0x0000;
    };

    public ref class GazeHidRotationParser sealed
    {
    public:
        GazeHidRotationParser(GazeDevicePreview ^ gazeDevice, uint16 usage);

        GazeHidPosition^ GetRotation(HidInputReport^ report);

    private:
        HidNumericControlDescription ^ _X = nullptr;
        HidNumericControlDescription ^ _Y = nullptr;
        HidNumericControlDescription ^ _Z = nullptr;
        uint16 _usage                     = 0x0000;
    };

    public ref class GazeHidPositionsParser sealed
    {
    public:
        GazeHidPositionsParser(GazeDevicePreview ^ gazeDevice);

        GazeHidPositions^ GetGazeHidPositions(HidInputReport ^ report);

    private:
        GazeHidPositionParser ^ _leftEyePositionParser;
        GazeHidPositionParser ^ _rightEyePositionParser;
        GazeHidPositionParser ^ _headPositionParser;
        GazeHidRotationParser ^ _headRotationParser;
    };
}

END_NAMESPACE_GAZE_INPUT
