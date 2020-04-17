#include "pch.h"
#include "SelectionItemPatternGazeTargetItem.h"
#include "SelectionItemPatternGazeTargetItem.g.cpp"
#include "winrt/Microsoft.UI.Xaml.Automation.Peers.h"
#include "winrt/Microsoft.UI.Xaml.Automation.Provider.h"

using namespace winrt;
using namespace winrt::Microsoft::UI::Xaml::Automation::Peers;
using namespace winrt::Microsoft::UI::Xaml::Automation::Provider;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    void SelectionItemPatternGazeTargetItem::Invoke()
    {
        auto peer = FrameworkElementAutomationPeer::FromElement(TargetElement());
        auto provider = peer.GetPattern(PatternInterface::SelectionItem).try_as<ISelectionItemProvider>();
        provider.Select();
    }
}
