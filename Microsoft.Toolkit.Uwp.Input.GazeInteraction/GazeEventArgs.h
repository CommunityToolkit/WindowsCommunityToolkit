//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "GazeEventArgs.g.h"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    struct GazeEventArgs : GazeEventArgsT<GazeEventArgs>
    {
    public:
        GazeEventArgs();

        bool Handled();
        void Handled(bool value);
        Windows::Foundation::Point Location();
        Windows::Foundation::TimeSpan Timestamp();
        void Set(Windows::Foundation::Point const& location, Windows::Foundation::TimeSpan const& timestamp);
    private:
        bool m_handled{ false };
        Point m_location;
        TimeSpan m_timestamp;
    };
}