//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeTargetItem.h"
#include "GazePointer.h"

#include "GazeElement.h"
#include "GazeFeedbackPopupFactory.h"

using namespace Windows::UI::Xaml::Automation;
using namespace Windows::UI::Xaml::Automation::Provider;
using namespace Windows::UI::Xaml::Automation::Peers;

BEGIN_NAMESPACE_GAZE_INPUT

static DependencyProperty^ GazeTargetItemProperty = DependencyProperty::RegisterAttached("_GazeTargetItem", GazeTargetItem::typeid, GazeTargetItem::typeid, ref new PropertyMetadata(nullptr));

template<PatternInterface P, typename T>
ref class PatternGazeTargetItem abstract : public GazeTargetItem
{
internal:

    PatternGazeTargetItem(UIElement^ element)
        : GazeTargetItem(element)
    {
    }

    static T^ GetPattern(AutomationPeer^ peer)
    {
        auto pattern = peer->GetPattern(P);
        return safe_cast<T^>(pattern);
    }

    static bool IsCandidate(AutomationPeer^ peer)
    {
        auto provider = GetPattern(peer);
        return provider != nullptr;
    }

    void Invoke() override sealed
    {
        auto peer = FrameworkElementAutomationPeer::FromElement(TargetElement);
        auto provider = GetPattern(peer);
        Invoke(provider);
    }

    virtual void Invoke(T^ provider) = 0;
};

ref class InvokePatternGazeTargetItem : public PatternGazeTargetItem<PatternInterface::Invoke, IInvokeProvider>
{
internal:

    InvokePatternGazeTargetItem(UIElement^ element)
        : PatternGazeTargetItem(element)
    {
    }

    void Invoke(IInvokeProvider^ provider) override sealed
    {
        provider->Invoke();
    }
};

ref class TogglePatternGazeTargetItem : public PatternGazeTargetItem<PatternInterface::Toggle, IToggleProvider>
{
internal:

    TogglePatternGazeTargetItem(UIElement^ element)
        : PatternGazeTargetItem(element)
    {
    }

    void Invoke(IToggleProvider^ provider) override
    {
        provider->Toggle();
    }
};

ref class SelectionItemPatternGazeTargetItem : public PatternGazeTargetItem<PatternInterface::SelectionItem, ISelectionItemProvider>
{
internal:

    SelectionItemPatternGazeTargetItem(UIElement^ element)
        : PatternGazeTargetItem(element)
    {
    }

    void Invoke(ISelectionItemProvider^ provider) override
    {
        provider->Select();
    }
};

ref class ExpandCollapsePatternGazeTargetItem : public PatternGazeTargetItem<PatternInterface::ExpandCollapse, IExpandCollapseProvider>
{
internal:

    ExpandCollapsePatternGazeTargetItem(UIElement^ element)
        : PatternGazeTargetItem(element)
    {
    }

    void Invoke(IExpandCollapseProvider^ provider) override
    {
        switch (provider->ExpandCollapseState)
        {
        case ExpandCollapseState::Collapsed:
            provider->Expand();
            break;

        case ExpandCollapseState::Expanded:
            provider->Collapse();
            break;
        }
    }
};

ref class ComboBoxItemGazeTargetItem sealed : GazeTargetItem
{
internal:

    ComboBoxItemGazeTargetItem(UIElement^ element)
        : GazeTargetItem(element)
    {
    }

    void Invoke() override
    {
        auto peer = FrameworkElementAutomationPeer::FromElement(TargetElement);
        auto comboBoxItemAutomationPeer = dynamic_cast<ComboBoxItemAutomationPeer^>(peer);
        auto comboBoxItem = safe_cast<ComboBoxItem^>(comboBoxItemAutomationPeer->Owner);

        AutomationPeer^ ancestor = comboBoxItemAutomationPeer;
        auto comboBoxAutomationPeer = dynamic_cast<ComboBoxAutomationPeer^>(ancestor);
        while (comboBoxAutomationPeer == nullptr)
        {
            ancestor = safe_cast<AutomationPeer^>(ancestor->Navigate(AutomationNavigationDirection::Parent));
            comboBoxAutomationPeer = dynamic_cast<ComboBoxAutomationPeer^>(ancestor);
        }

        comboBoxItem->IsSelected = true;
        comboBoxAutomationPeer->Collapse();
    }
};

ref class PivotItemGazeTargetItem sealed : GazeTargetItem
{
internal:

    PivotItemGazeTargetItem(UIElement^ element)
        : GazeTargetItem(element)
    {
    }

    void Invoke() override
    {
        auto headerItem = safe_cast<PivotHeaderItem^>(TargetElement);
        auto headerPanel = safe_cast<PivotHeaderPanel^>(VisualTreeHelper::GetParent(headerItem));
        unsigned index;
        headerPanel->Children->IndexOf(headerItem, &index);

        DependencyObject^ walker = headerPanel;
        Pivot^ pivot;
        do
        {
            walker = VisualTreeHelper::GetParent(walker);
            pivot = dynamic_cast<Pivot^>(walker);
        } while (pivot == nullptr);

        pivot->SelectedIndex = index;
    }
};

GazeTargetItem^ GazeTargetItem::GetOrCreate(UIElement^ element)
{
    GazeTargetItem^ item;

    auto value = element->ReadLocalValue(GazeTargetItemProperty);

    if (value != DependencyProperty::UnsetValue)
    {
        item = safe_cast<GazeTargetItem^>(value);
    }
    else
    {
        auto peer = FrameworkElementAutomationPeer::FromElement(element);

        if (peer == nullptr)
        {
            if (dynamic_cast<PivotHeaderItem^>(element) != nullptr)
            {
                item = ref new PivotItemGazeTargetItem(element);
            }
            else
            {
                item = GazePointer::Instance->_nonInvokeGazeTargetItem;
            }
        }
        else if (InvokePatternGazeTargetItem::IsCandidate(peer))
        {
            item = ref new InvokePatternGazeTargetItem(element);
        }
        else if (TogglePatternGazeTargetItem::IsCandidate(peer))
        {
            item = ref new TogglePatternGazeTargetItem(element);
        }
        else if (SelectionItemPatternGazeTargetItem::IsCandidate(peer))
        {
            item = ref new SelectionItemPatternGazeTargetItem(element);
        }
        else if (ExpandCollapsePatternGazeTargetItem::IsCandidate(peer))
        {
            item = ref new ExpandCollapsePatternGazeTargetItem(element);
        }
        else if (dynamic_cast<ComboBoxItemAutomationPeer^>(peer) != nullptr)
        {
            item = ref new ComboBoxItemGazeTargetItem(element);
        }
        else
        {
            item = GazePointer::Instance->_nonInvokeGazeTargetItem;
        }

        element->SetValue(GazeTargetItemProperty, item);
    }

    return item;
}

void GazeTargetItem::RaiseProgressEvent(DwellProgressState state)
{
    // TODO: We should eliminate non-invokable controls before we arrive here!
    if (dynamic_cast<Page^>(TargetElement) != nullptr)
    {
        return;
    }

    if (_notifiedProgressState != state || state == DwellProgressState::Progressing)
    {
        auto handled = false;

        auto gazeElement = GazeInput::GetGazeElement(TargetElement);
        if (gazeElement != nullptr)
        {
            handled = gazeElement->RaiseProgressFeedback(TargetElement, state, ElapsedTime - _prevStateTime, _nextStateTime - _prevStateTime);
        }

        if (!handled && state != DwellProgressState::Idle)
        {
            if (_feedbackPopup == nullptr)
            {
                _feedbackPopup = GazePointer::Instance->_gazeFeedbackPopupFactory->Get();
            }

            auto control = safe_cast<FrameworkElement^>(TargetElement);

            auto transform = control->TransformToVisual(_feedbackPopup);
            auto bounds = transform->TransformBounds(*ref new Rect(*ref new Point(0, 0),
                *ref new Size(safe_cast<float>(control->ActualWidth), safe_cast<float>(control->ActualHeight))));
            auto rectangle = safe_cast<Rectangle^>(_feedbackPopup->Child);

            if (state == DwellProgressState::Progressing)
            {
                auto progress = ((double)(ElapsedTime - _prevStateTime).Duration) / (_nextStateTime - _prevStateTime).Duration;

                if (0 <= progress && progress < 1)
                {
                    rectangle->Stroke = GazeInput::DwellFeedbackProgressBrush;
                    rectangle->Width = (1 - progress) * bounds.Width;
                    rectangle->Height = (1 - progress) * bounds.Height;

                    _feedbackPopup->HorizontalOffset = bounds.Left + progress * bounds.Width / 2;
                    _feedbackPopup->VerticalOffset = bounds.Top + progress * bounds.Height / 2;
                }
            }
            else
            {
                rectangle->Stroke = state == DwellProgressState::Fixating ?
                    GazeInput::DwellFeedbackEnterBrush : GazeInput::DwellFeedbackCompleteBrush;
                rectangle->Width = bounds.Width;
                rectangle->Height = bounds.Height;

                _feedbackPopup->HorizontalOffset = bounds.Left;
                _feedbackPopup->VerticalOffset = bounds.Top;
            }

            _feedbackPopup->IsOpen = true;
        }
        else
        {
            if (_feedbackPopup != nullptr)
            {
                GazePointer::Instance->_gazeFeedbackPopupFactory->Return(_feedbackPopup);
                _feedbackPopup = nullptr;
            }
        }
    }

    _notifiedProgressState = state;
}

END_NAMESPACE_GAZE_INPUT