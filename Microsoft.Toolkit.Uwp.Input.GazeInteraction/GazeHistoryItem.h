//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "GazeHistoryItem.g.h"

using namespace winrt::Windows::Foundation;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    struct GazeHistoryItem : GazeHistoryItemT<GazeHistoryItem>
    {
    public:
        GazeHistoryItem() = default;

        Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem HitTarget();
        void HitTarget(Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem const& value);
        Windows::Foundation::TimeSpan Timestamp();
        void Timestamp(Windows::Foundation::TimeSpan const& value);
        Windows::Foundation::TimeSpan Duration();
        void Duration(Windows::Foundation::TimeSpan const& value);

    private:
        Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem m_hitTarget{ nullptr };
        Windows::Foundation::TimeSpan m_timestamp;
        Windows::Foundation::TimeSpan m_duration;
    };
}
