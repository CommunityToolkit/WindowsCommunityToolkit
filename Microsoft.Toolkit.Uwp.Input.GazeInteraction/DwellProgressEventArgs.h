//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "DwellProgressEventArgs.g.h"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    /// <summary>
    /// This parameter is passed to the GazeElement.DwellProgressFeedback event. The event is fired to inform the application of the user's progress towards completing dwelling on a control
    /// </summary>
    struct DwellProgressEventArgs : DwellProgressEventArgsT<DwellProgressEventArgs>
    {
    public:
        DwellProgressEventArgs() = default;

        DwellProgressEventArgs(Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellProgressState const& state, Windows::Foundation::TimeSpan const& elapsedDuration, Windows::Foundation::TimeSpan const& triggerDuration);

        /// <summary>
        /// An enum that reflects the current state of dwell progress
        /// </summary>
        DwellProgressState State();
        
        /// <summary>
        /// A parameter for the application to set to true if it handles the event. If this parameter is set to true, the library suppresses default animation for dwell feedback on the control
        /// </summary>
        bool Handled();

        void Handled(bool value);

        /// <summary>
        /// A value between 0 and 1 that reflects the fraction of progress towards completing dwell
        /// </summary>
        double Progress();

    private:
        DwellProgressState _state{ DwellProgressState::Idle };
        double _progress;
        bool _handled{ false };
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct DwellProgressEventArgs : DwellProgressEventArgsT<DwellProgressEventArgs, implementation::DwellProgressEventArgs>
    {
    };
}
