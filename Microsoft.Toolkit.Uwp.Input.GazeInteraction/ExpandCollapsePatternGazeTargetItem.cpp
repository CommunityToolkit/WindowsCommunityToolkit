#include "pch.h"
#include "ExpandCollapsePatternGazeTargetItem.h"
#include "ExpandCollapsePatternGazeTargetItem.g.cpp"
#include "winrt/Microsoft.UI.Xaml.Automation.h"
#include "winrt/Microsoft.UI.Xaml.Automation.Peers.h"
#include "winrt/Microsoft.UI.Xaml.Automation.Provider.h"

using namespace winrt;
using namespace winrt::Microsoft::UI::Xaml::Automation;
using namespace winrt::Microsoft::UI::Xaml::Automation::Peers;
using namespace winrt::Microsoft::UI::Xaml::Automation::Provider;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    void ExpandCollapsePatternGazeTargetItem::Invoke()
    {
        auto peer = FrameworkElementAutomationPeer::FromElement(TargetElement());
        auto provider = peer.GetPattern(PatternInterface::ExpandCollapse).try_as<IExpandCollapseProvider>();
        switch (provider.ExpandCollapseState())
        {
        case ExpandCollapseState::Collapsed:
            provider.Expand();
            break;

        case ExpandCollapseState::Expanded:
            provider.Collapse();
            break;
        }
    }
}
