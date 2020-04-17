#include "pch.h"
#include "NonInvokeGazeTargetItem.h"
#include "NonInvokeGazeTargetItem.g.cpp"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    bool NonInvokeGazeTargetItem::IsInvokable()
    {
        return false;
    }

    void NonInvokeGazeTargetItem::Invoke()
    {
    }
}
