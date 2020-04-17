#pragma once
#include "InvokePatternGazeTargetItem.g.h"
#include "GazeTargetItem.h"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    struct InvokePatternGazeTargetItem : InvokePatternGazeTargetItemT<InvokePatternGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>
    {
        InvokePatternGazeTargetItem() = default;

        InvokePatternGazeTargetItem(Microsoft::UI::Xaml::UIElement const& element)
            : InvokePatternGazeTargetItemT<InvokePatternGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>(element)
        {}

        void Invoke();
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct InvokePatternGazeTargetItem : InvokePatternGazeTargetItemT<InvokePatternGazeTargetItem, implementation::InvokePatternGazeTargetItem>
    {
    };
}
