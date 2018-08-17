#pragma once

#include "GazeHidUsages.h"

using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::Devices::Input::Preview;

BEGIN_NAMESPACE_GAZE_INPUT

namespace GazeHidParsers {
    public ref class LongLong3 sealed
    {
    private:
        long long _X;
        long long _Y;
        long long _Z;

    public:
        property long long X
        {
            long long get() {
                return _X;
            }
            void set(long long value) {
                _X = value;
            }
        }

        property long long Y
        {
            long long get() {
                return _Y;
            }
            void set(long long value) {
                _Y = value;
            }
        }

        property long long Z
        {
            long long get() {
                return _Z;
            }
            void set(long long value) {
                _Z = value;
            }
        }
    };

#pragma region GazeHidPositionParser
    public ref class GazeHidPositionParser sealed
    {
    public:
        GazeHidPositionParser(GazeDevicePreview ^ gazeDevice, uint16 usage);

        LongLong3^ GetPosition(HidInputReport ^ report);

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

        LongLong3^ GetRotation(HidInputReport^ report);

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
