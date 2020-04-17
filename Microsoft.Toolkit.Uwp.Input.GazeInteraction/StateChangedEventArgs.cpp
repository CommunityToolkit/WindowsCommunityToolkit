#include "pch.h"
#include "StateChangedEventArgs.h"
#include "StateChangedEventArgs.g.cpp"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    StateChangedEventArgs::StateChangedEventArgs() :
        _hitTarget{ nullptr }
    { }

    StateChangedEventArgs::StateChangedEventArgs(Microsoft::UI::Xaml::UIElement const& target, Microsoft::Toolkit::Uwp::Input::GazeInteraction::PointerState const& state, Windows::Foundation::TimeSpan const& elapsedTime) :
        _hitTarget{ target },
        _pointerState{ state },
        _elapsedTime{ elapsedTime }
    {
    }

    Microsoft::Toolkit::Uwp::Input::GazeInteraction::PointerState StateChangedEventArgs::PointerState()
    {
        return _pointerState;
    }

    Windows::Foundation::TimeSpan StateChangedEventArgs::ElapsedTime()
    {
        return _elapsedTime;
    }
}
