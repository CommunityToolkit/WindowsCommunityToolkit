#pragma once
#include "ComboBoxItemGazeTargetItem.g.h"
#include "GazeTargetItem.h"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    struct ComboBoxItemGazeTargetItem : ComboBoxItemGazeTargetItemT<ComboBoxItemGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>
    {
        ComboBoxItemGazeTargetItem() = default;

        ComboBoxItemGazeTargetItem(Microsoft::UI::Xaml::UIElement const& element)
            : ComboBoxItemGazeTargetItemT<ComboBoxItemGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>(element)
        {}

        void Invoke();
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct ComboBoxItemGazeTargetItem : ComboBoxItemGazeTargetItemT<ComboBoxItemGazeTargetItem, implementation::ComboBoxItemGazeTargetItem>
    {
    };
}
