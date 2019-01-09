//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeHidParsers.h"

using namespace Windows::Foundation::Collections;

BEGIN_NAMESPACE_GAZE_INPUT

namespace GazeHidParsers {
    static HidNumericControlDescription ^ GetGazeUsageFromCollectionId(
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

#pragma region GazeHidPositionParser
    GazeHidPositionParser::GazeHidPositionParser(GazeDevicePreview ^ gazeDevice, uint16 usage)
    {
        _usage = usage;

        // Find all the position usages from the device's
        // descriptor and store them for easy access
        _X = GetGazeUsageFromCollectionId(gazeDevice, (USHORT)GazeHidUsages::Usage_PositionX, _usage);
        _Y = GetGazeUsageFromCollectionId(gazeDevice, (USHORT)GazeHidUsages::Usage_PositionY, _usage);
        _Z = GetGazeUsageFromCollectionId(gazeDevice, (USHORT)GazeHidUsages::Usage_PositionZ, _usage);
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

            if ((controlDescX->LogicalMaximum < descX->ScaledValue || controlDescX->LogicalMinimum > descX->ScaledValue) ||
                (controlDescY->LogicalMaximum < descY->ScaledValue || controlDescY->LogicalMinimum > descY->ScaledValue) ||
                (controlDescZ->LogicalMaximum < descZ->ScaledValue || controlDescZ->LogicalMinimum > descZ->ScaledValue))
            {
                // One of the values is outside of the valid range.
            }
            else
            {
                result = ref new GazeHidPosition();
                result->X = descX->ScaledValue;
                result->Y = descY->ScaledValue;
                result->Z = descZ->ScaledValue;
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
        _X = GetGazeUsageFromCollectionId(gazeDevice, (USHORT)GazeHidUsages::Usage_RotationX, _usage);
        _Y = GetGazeUsageFromCollectionId(gazeDevice, (USHORT)GazeHidUsages::Usage_RotationY, _usage);
        _Z = GetGazeUsageFromCollectionId(gazeDevice, (USHORT)GazeHidUsages::Usage_RotationZ, _usage);
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

            if ((controlDescX->LogicalMaximum < descX->ScaledValue || controlDescX->LogicalMinimum > descX->ScaledValue) ||
                (controlDescY->LogicalMaximum < descY->ScaledValue || controlDescY->LogicalMinimum > descY->ScaledValue) ||
                (controlDescZ->LogicalMaximum < descZ->ScaledValue || controlDescZ->LogicalMinimum > descZ->ScaledValue))
            {
                // One of the values is outside of the valid range.
            }
            else
            {
                result = ref new GazeHidPosition();
                result->X = descX->ScaledValue;
                result->Y = descY->ScaledValue;
                result->Z = descZ->ScaledValue;
            }
        }

        return result;
    }
#pragma endregion GazeHidRotationParser

#pragma region GazeHidPositionsParser
    GazeHidPositionsParser::GazeHidPositionsParser(GazeDevicePreview ^ gazeDevice)
    {
        _leftEyePositionParser  = ref new GazeHidPositionParser(gazeDevice, (USHORT)GazeHidUsages::Usage_LeftEyePosition);
        _rightEyePositionParser = ref new GazeHidPositionParser(gazeDevice, (USHORT)GazeHidUsages::Usage_RightEyePosition);
        _headPositionParser     = ref new GazeHidPositionParser(gazeDevice, (USHORT)GazeHidUsages::Usage_HeadPosition);
        _headRotationParser     = ref new GazeHidRotationParser(gazeDevice, (USHORT)GazeHidUsages::Usage_HeadDirectionPoint);
    }

    GazeHidPositions ^ GazeHidPositionsParser::GetGazeHidPositions(HidInputReport ^ report)
    {
        auto retval = ref new GazeHidPositions();

        retval->LeftEyePosition  = _leftEyePositionParser->GetPosition(report);
        retval->RightEyePosition = _rightEyePositionParser->GetPosition(report);
        retval->HeadPosition     = _headPositionParser->GetPosition(report);
        retval->HeadRotation     = _headRotationParser->GetRotation(report);

        return retval;
    }
#pragma endregion GazeHidPositionsParser
}

END_NAMESPACE_GAZE_INPUT
