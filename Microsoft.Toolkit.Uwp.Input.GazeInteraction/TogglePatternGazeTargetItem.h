#pragma once
#include "TogglePatternGazeTargetItem.g.h"
#include "GazeTargetItem.h"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    struct TogglePatternGazeTargetItem : TogglePatternGazeTargetItemT<TogglePatternGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>
    {
        TogglePatternGazeTargetItem() = default;

        TogglePatternGazeTargetItem(Microsoft::UI::Xaml::UIElement const& element)
            : TogglePatternGazeTargetItemT<TogglePatternGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>(element)
        {}

        void Invoke();
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct TogglePatternGazeTargetItem : TogglePatternGazeTargetItemT<TogglePatternGazeTargetItem, implementation::TogglePatternGazeTargetItem>
    {
    };
}
