#include "pch.h"
#include "ComboBoxItemGazeTargetItem.h"
#include "ComboBoxItemGazeTargetItem.g.cpp"
#include "winrt/Microsoft.UI.Xaml.Automation.Peers.h"
#include "winrt/Microsoft.UI.Xaml.Automation.Provider.h"

using namespace winrt;
using namespace winrt::Microsoft::UI::Xaml::Automation::Peers;
using namespace winrt::Microsoft::UI::Xaml::Automation::Provider;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    void ComboBoxItemGazeTargetItem::Invoke()
    {
        auto peer = FrameworkElementAutomationPeer::FromElement(TargetElement());
        ComboBoxItemAutomationPeer comboBoxItemAutomationPeer{ peer.try_as<ComboBoxItemAutomationPeer>() };
        ComboBoxItem comboBoxItem{ comboBoxItemAutomationPeer.Owner().try_as<ComboBoxItem>() };

        AutomationPeer ancestor = comboBoxItemAutomationPeer;
        ComboBoxAutomationPeer comboBoxAutomationPeer{ ancestor.try_as<ComboBoxAutomationPeer>() };
        while (comboBoxAutomationPeer == nullptr)
        {
            ancestor = ancestor.Navigate(AutomationNavigationDirection::Parent).try_as<AutomationPeer>();
            comboBoxAutomationPeer = ancestor.try_as<ComboBoxAutomationPeer>();
        }

        comboBoxItem.IsSelected(true);
        comboBoxAutomationPeer.Collapse();
    }
}
