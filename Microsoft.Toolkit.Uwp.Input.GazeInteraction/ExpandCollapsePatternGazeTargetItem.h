#pragma once
#include "ExpandCollapsePatternGazeTargetItem.g.h"
#include "GazeTargetItem.h"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    struct ExpandCollapsePatternGazeTargetItem : ExpandCollapsePatternGazeTargetItemT<ExpandCollapsePatternGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>
    {
        ExpandCollapsePatternGazeTargetItem() = default;

        ExpandCollapsePatternGazeTargetItem(Microsoft::UI::Xaml::UIElement const& element)
            : ExpandCollapsePatternGazeTargetItemT<ExpandCollapsePatternGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>(element)
        {}

        void Invoke();
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct ExpandCollapsePatternGazeTargetItem : ExpandCollapsePatternGazeTargetItemT<ExpandCollapsePatternGazeTargetItem, implementation::ExpandCollapsePatternGazeTargetItem>
    {
    };
}
