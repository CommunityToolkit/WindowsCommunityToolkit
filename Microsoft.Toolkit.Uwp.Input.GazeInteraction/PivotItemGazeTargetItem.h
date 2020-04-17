#pragma once
#include "PivotItemGazeTargetItem.g.h"
#include "GazeTargetItem.h"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    struct PivotItemGazeTargetItem : PivotItemGazeTargetItemT<PivotItemGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>
    {
        PivotItemGazeTargetItem() = default;

        PivotItemGazeTargetItem(Microsoft::UI::Xaml::UIElement const& element)
            : PivotItemGazeTargetItemT<PivotItemGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>(element)
        {}

        void Invoke();
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct PivotItemGazeTargetItem : PivotItemGazeTargetItemT<PivotItemGazeTargetItem, implementation::PivotItemGazeTargetItem>
    {
    };
}
