//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "StateChangedEventArgs.g.h"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    /// <summary>
    /// This parameter is passed to the StateChanged event.
    /// </summary>
    struct StateChangedEventArgs : StateChangedEventArgsT<StateChangedEventArgs>
    {
    public:
        StateChangedEventArgs();

        StateChangedEventArgs(Microsoft::UI::Xaml::UIElement const& target, Microsoft::Toolkit::Uwp::Input::GazeInteraction::PointerState const& state, Windows::Foundation::TimeSpan const& elapsedTime);

        /// <summary>
        /// The state of user's gaze with respect to a control
        /// </summary>
        Microsoft::Toolkit::Uwp::Input::GazeInteraction::PointerState PointerState();

        /// <summary>
        /// Elapsed time since the last state
        /// </summary>
        Windows::Foundation::TimeSpan ElapsedTime();

    private:
        winrt::Microsoft::UI::Xaml::UIElement _hitTarget;
        ::Microsoft::Toolkit::Uwp::Input::GazeInteraction::PointerState _pointerState{ ::Microsoft::Toolkit::Uwp::Input::GazeInteraction::PointerState::Exit };
        Windows::Foundation::TimeSpan _elapsedTime;
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct StateChangedEventArgs : StateChangedEventArgsT<StateChangedEventArgs, implementation::StateChangedEventArgs>
    {
    };
}
