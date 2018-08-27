#include "pch.h"
#include "GazeHidParsers.h"

using namespace Windows::Foundation::Collections;

BEGIN_NAMESPACE_GAZE_INPUT

namespace GazeHidParsers {
#pragma region GazeHidPositionParser
    GazeHidPositionParser::GazeHidPositionParser(GazeDevicePreview ^ gazeDevice, uint16 usage)
    {
        _usage = usage;

        // Find all the position usages from the device's
        // descriptor and store them for easy access
        _X = GazeHidParsers::GetGazeUsageFromCollectionId(gazeDevice, (USHORT)GazeHidUsages::Usage_PositionX, _usage);
        _Y = GazeHidParsers::GetGazeUsageFromCollectionId(gazeDevice, (USHORT)GazeHidUsages::Usage_PositionY, _usage);
        _Z = GazeHidParsers::GetGazeUsageFromCollectionId(gazeDevice, (USHORT)GazeHidUsages::Usage_PositionZ, _usage);
    }

    GazeHidPosition^ GazeHidPositionParser::GetPosition(HidInputReport ^ report)
    {
        GazeHidPosition^ result = nullptr;

        if (_X != nullptr &&
            _Y != nullptr &&
            _Z != nullptr &&
            _usage != 0x0000)
        {
            auto descX = report->GetNumericControlByDescription(_X);
            auto descY = report->GetNumericControlByDescription(_Y);
            auto descZ = report->GetNumericControlByDescription(_Z);

            auto controlDescX = descX->ControlDescription;
            auto controlDescY = descY->ControlDescription;
            auto controlDescZ = descZ->ControlDescription;

            if ((controlDescX->LogicalMaximum < descX->Value || controlDescX->LogicalMinimum > descX->Value) ||
                (controlDescY->LogicalMaximum < descY->Value || controlDescY->LogicalMinimum > descY->Value) ||
                (controlDescZ->LogicalMaximum < descZ->Value || controlDescZ->LogicalMinimum > descZ->Value))
            {
                // One of the values is outside of the valid range.
            }
            else
            {
                result = ref new GazeHidPosition();
                result->X = descX->Value;
                result->Y = descY->Value;
                result->Z = descZ->Value;
            }
        }

        return result;
    }
#pragma endregion GazeHidPositionParser

#pragma region GazeHidRotationParser
    GazeHidRotationParser::GazeHidRotationParser(GazeDevicePreview ^ gazeDevice, uint16 usage)
    {
        _usage = usage;

        // Find all the rotation usages from the device's
        // descriptor and store them for easy access
        _X = GazeHidParsers::GetGazeUsageFromCollectionId(gazeDevice, (USHORT)GazeHidUsages::Usage_RotationX, _usage);
        _Y = GazeHidParsers::GetGazeUsageFromCollectionId(gazeDevice, (USHORT)GazeHidUsages::Usage_RotationY, _usage);
        _Z = GazeHidParsers::GetGazeUsageFromCollectionId(gazeDevice, (USHORT)GazeHidUsages::Usage_RotationZ, _usage);
    }

    GazeHidPosition^ GazeHidRotationParser::GetRotation(HidInputReport ^ report)
    {
        GazeHidPosition^ result = nullptr;

        if (_X != nullptr &&
            _Y != nullptr &&
            _Z != nullptr &&
            _usage != 0x0000)
        {
            auto descX = report->GetNumericControlByDescription(_X);
            auto descY = report->GetNumericControlByDescription(_Y);
            auto descZ = report->GetNumericControlByDescription(_Z);

            auto controlDescX = descX->ControlDescription;
            auto controlDescY = descY->ControlDescription;
            auto controlDescZ = descZ->ControlDescription;

            if ((controlDescX->LogicalMaximum < descX->Value || controlDescX->LogicalMinimum > descX->Value) ||
                (controlDescY->LogicalMaximum < descY->Value || controlDescY->LogicalMinimum > descY->Value) ||
                (controlDescZ->LogicalMaximum < descZ->Value || controlDescZ->LogicalMinimum > descZ->Value))
            {
                // One of the values is outside of the valid range.
            }
            else
            {
                result = ref new GazeHidPosition();
                result->X = descX->Value;
                result->Y = descY->Value;
                result->Z = descZ->Value;
            }
        }

        return result;
    }
#pragma endregion GazeHidRotationParser

#pragma region GazeHidParsers
    HidNumericControlDescription ^ GazeHidParsers::GetGazeUsageFromCollectionId(
        GazeDevicePreview ^ gazeDevice, 
        uint16 childUsageId, 
        uint16 parentUsageId)
    {
        IVectorView<HidNumericControlDescription ^> ^ numericControls = gazeDevice->GetNumericControlDescriptions(
            (USHORT)GazeHidUsages::UsagePage_EyeHeadTracker, childUsageId);

        for (unsigned int i = 0; i < numericControls->Size; i++)
        {
            auto parentCollections = numericControls->GetAt(i)->ParentCollections;
            if (parentCollections->Size > 0 &&
                parentCollections->GetAt(0)->UsagePage == (USHORT)GazeHidUsages::UsagePage_EyeHeadTracker &&
                parentCollections->GetAt(0)->UsageId == parentUsageId)
            {
                return numericControls->GetAt(i);
            }
        }
        return nullptr;
    }
#pragma endregion GazeHidParsers
}

END_NAMESPACE_GAZE_INPUT
