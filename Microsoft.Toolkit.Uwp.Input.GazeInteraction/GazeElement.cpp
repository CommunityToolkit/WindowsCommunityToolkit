//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeElement.h"
#include "DwellProgressEventArgs.h"
#include "StateChangedEventArgs.h"
#include "DwellInvokedRoutedEventArgs.h"
#include "winrt/Windows.Foundation.h"
#include "GazeElement.g.cpp"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    winrt::event_token GazeElement::StateChanged(Windows::Foundation::EventHandler<Microsoft::Toolkit::Uwp::Input::GazeInteraction::StateChangedEventArgs> const& handler)
    {
        return m_stateChanged.add(handler);
    }
    void GazeElement::StateChanged(winrt::event_token const& token) noexcept
    {
        m_stateChanged.remove(token);
    }
    winrt::event_token GazeElement::DwellProgressFeedback(Windows::Foundation::EventHandler<Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellProgressEventArgs> const& handler)
    {
        return m_dwellProgressFeedback.add(handler);
    }
    void GazeElement::DwellProgressFeedback(winrt::event_token const& token) noexcept
    {
        m_dwellProgressFeedback.remove(token);
    }
    winrt::event_token GazeElement::Invoked(Windows::Foundation::EventHandler<Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellInvokedRoutedEventArgs> const& handler)
    {
        return m_invoked.add(handler);
    }
    void GazeElement::Invoked(winrt::event_token const& token) noexcept
    {
        m_invoked.remove(token);
    }

    void GazeElement::RaiseStateChanged(Windows::Foundation::IInspectable const& sender, Microsoft::Toolkit::Uwp::Input::GazeInteraction::StateChangedEventArgs const& args)
    {
        m_stateChanged(sender, args);
    }

    void GazeElement::RaiseInvoked(Windows::Foundation::IInspectable const& sender, Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellInvokedRoutedEventArgs const& args)
    {
        m_invoked(sender, args);
    }

    bool GazeElement::RaiseProgressFeedback(Windows::Foundation::IInspectable const& sender, Microsoft::Toolkit::Uwp::Input::GazeInteraction::DwellProgressState const& state, Windows::Foundation::TimeSpan const& elapsedTime, Windows::Foundation::TimeSpan const& triggerTime)
    {
        auto args = winrt::make<DwellProgressEventArgs>(state, elapsedTime, triggerTime);
        m_dwellProgressFeedback(sender, args);
        return args.Handled();
    }
}
