//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeHistoryItem.h"
#include "GazeHistoryItem.g.cpp"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem GazeHistoryItem::HitTarget()
    {
        return m_hitTarget;
    }

    void GazeHistoryItem::HitTarget(Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem const& value)
    {
        m_hitTarget = value;
    }

    Windows::Foundation::TimeSpan GazeHistoryItem::Timestamp()
    {
        return m_timestamp;
    }
    void GazeHistoryItem::Timestamp(Windows::Foundation::TimeSpan const& value)
    {
        m_timestamp = value;
    }
    Windows::Foundation::TimeSpan GazeHistoryItem::Duration()
    {
        return m_duration;
    }
    void GazeHistoryItem::Duration(Windows::Foundation::TimeSpan const& value)
    {
        m_duration = value;
    }
}
