#pragma once
#include "SelectionItemPatternGazeTargetItem.g.h"
#include "GazeTargetItem.h"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    struct SelectionItemPatternGazeTargetItem : SelectionItemPatternGazeTargetItemT<SelectionItemPatternGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>
    {
        SelectionItemPatternGazeTargetItem() = default;

        SelectionItemPatternGazeTargetItem(Microsoft::UI::Xaml::UIElement const& element)
            : SelectionItemPatternGazeTargetItemT<SelectionItemPatternGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>(element)
        {}

        void Invoke();
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct SelectionItemPatternGazeTargetItem : SelectionItemPatternGazeTargetItemT<SelectionItemPatternGazeTargetItem, implementation::SelectionItemPatternGazeTargetItem>
    {
    };
}
