#include "pch.h"
#include "TogglePatternGazeTargetItem.h"
#include "TogglePatternGazeTargetItem.g.cpp"
#include "winrt/Microsoft.UI.Xaml.Automation.Peers.h"
#include "winrt/Microsoft.UI.Xaml.Automation.Provider.h"

using namespace winrt;
using namespace winrt::Microsoft::UI::Xaml::Automation::Peers;
using namespace winrt::Microsoft::UI::Xaml::Automation::Provider;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    void TogglePatternGazeTargetItem::Invoke()
    {
        auto peer = FrameworkElementAutomationPeer::FromElement(TargetElement());
        auto provider = peer.GetPattern(PatternInterface::Toggle).try_as<IToggleProvider>();
        provider.Toggle();
    }
}
