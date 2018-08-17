#pragma once

#include "GazeHidUsages.h"

using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::Devices::Input::Preview;

BEGIN_NAMESPACE_GAZE_INPUT

namespace GazeHidParsers {
    public ref class Long3 sealed
    {
    public:
        long X;
        long Y;
        long Z;
    };

#pragma region GazeHidPositionParser
    public ref class GazeHidPositionParser sealed
    {
    public:
        GazeHidPositionParser(GazeDevicePreview ^ gazeDevice, uint16 usage);

        Long3^ GetPosition(HidInputReport ^ report);

    private:
        HidNumericControlDescription ^ _X = nullptr;
        HidNumericControlDescription ^ _Y = nullptr;
        HidNumericControlDescription ^ _Z = nullptr;
        uint16 _usage                     = 0x0000;
    };
#pragma endregion GazeHidPositionParser

#pragma region GazeHidRotationParser
    public ref class GazeHidRotationParser sealed
    {
    public:
        GazeHidRotationParser(GazeDevicePreview ^ gazeDevice, uint16 usage);

        Long3^ GetRotation(HidInputReport^ report);

    private:
        HidNumericControlDescription ^ _X = nullptr;
        HidNumericControlDescription ^ _Y = nullptr;
        HidNumericControlDescription ^ _Z = nullptr;
        uint16 _usage                     = 0x0000;
    };
#pragma endregion GazeHidRotationParser

    class GazeHidParsers {
    public:
        static HidNumericControlDescription ^ GetGazeUsageFromCollectionId(
            GazeDevicePreview^ gazeDevice,
            uint16 childUsageId,
            uint16 parentUsageId
        );
    };
}

END_NAMESPACE_GAZE_INPUT
