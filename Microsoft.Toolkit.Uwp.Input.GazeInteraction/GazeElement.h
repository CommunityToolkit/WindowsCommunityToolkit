//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "GazeElement.g.h"
#include "DwellInvokedRoutedEventArgs.h"
#include "DwellProgressEventArgs.h"
#include "StateChangedEventArgs.h"

using namespace winrt::Windows::Foundation;
using namespace winrt::Microsoft::UI::Xaml;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    /// <summary>
    /// Surrogate object attached to controls allowing subscription to per-control gaze events.
    /// </summary>
    struct GazeElement : GazeElementT<GazeElement>
    {
        GazeElement() = default;

        winrt::event_token StateChanged(Windows::Foundation::EventHandler<Microsoft::Toolkit::Uwp::Input::GazeInteraction::StateChangedEventArgs> const& handler);
        void StateChanged(winrt::event_token const& token) noexcept;
        winrt::event_token DwellProgressFeedback(Windows::Foundation::EventHandler<Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellProgressEventArgs> const& handler);
        void DwellProgressFeedback(winrt::event_token const& token) noexcept;
        winrt::event_token Invoked(Windows::Foundation::EventHandler<Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellInvokedRoutedEventArgs> const& handler);
        void Invoked(winrt::event_token const& token) noexcept;

        void RaiseStateChanged(Windows::Foundation::IInspectable const& sender, Microsoft::Toolkit::Uwp::Input::GazeInteraction::StateChangedEventArgs const& args);
        void RaiseInvoked(Windows::Foundation::IInspectable const& sender, Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellInvokedRoutedEventArgs const& args);
        bool RaiseProgressFeedback(Windows::Foundation::IInspectable const& sender, Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellProgressState const& state, Windows::Foundation::TimeSpan const& elapsedTime, Windows::Foundation::TimeSpan const& triggerTime);

        /// <summary>
        /// This event is fired when the state of the user's gaze on a control has changed
        /// </summary>
        winrt::event<EventHandler<Microsoft::Toolkit::Uwp::Input::GazeInteraction::StateChangedEventArgs>> m_stateChanged;

        /// <summary>
        /// This event is fired when the user completed dwelling on a control and the control is about to be invoked by default. This event is fired to give the application an opportunity to prevent default invocation
        /// </summary>
        winrt::event<EventHandler<Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellInvokedRoutedEventArgs>> m_invoked;

        /// <summary>
        /// This event is fired to inform the application of the progress towards dwell
        /// </summary>
        winrt::event<EventHandler<Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellProgressEventArgs>> m_dwellProgressFeedback;
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct GazeElement : GazeElementT<GazeElement, implementation::GazeElement>
    {
    };
}
