#pragma once
#include "NonInvokeGazeTargetItem.g.h"
#include "GazeTargetItem.h"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    struct NonInvokeGazeTargetItem : NonInvokeGazeTargetItemT<NonInvokeGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>
    {
        NonInvokeGazeTargetItem()
            : NonInvokeGazeTargetItemT<NonInvokeGazeTargetItem, Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation::GazeTargetItem>(Page())
        {}

        bool IsInvokable();

        void Invoke();
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct NonInvokeGazeTargetItem : NonInvokeGazeTargetItemT<NonInvokeGazeTargetItem, implementation::NonInvokeGazeTargetItem>
    {
    };
}
