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

ref class InvokeGazeTargetItem sealed : public GazeTargetItem
{
internal:

    InvokeGazeTargetItem(UIElement^ element)
        : GazeTargetItem(element)
    {
    }

    void Invoke() override
    {
        auto peer = FrameworkElementAutomationPeer::FromElement(TargetElement);
        auto invokeProvider = safe_cast<IInvokeProvider^>(peer);
        invokeProvider->Invoke();
    }
};

ref class ToggleGazeTargetItem sealed : public GazeTargetItem
{
internal:

    ToggleGazeTargetItem(UIElement^ element)
        : GazeTargetItem(element)
    {
    }

    void Invoke() override
    {
        auto peer = FrameworkElementAutomationPeer::FromElement(TargetElement);
        auto toggleProvider = safe_cast<IToggleProvider ^>(peer);
        toggleProvider->Toggle();
    }
};

ref class SelectionGazeTargetItem sealed : public GazeTargetItem
{
internal:

    SelectionGazeTargetItem(UIElement^ element)
        : GazeTargetItem(element)
    {
    }

    void Invoke() override
    {
        auto peer = FrameworkElementAutomationPeer::FromElement(TargetElement);
        auto selectionItemProvider = safe_cast<ISelectionItemProvider ^>(peer);
        selectionItemProvider->Select();
    }
};

ref class ExpandCollapseGazeTargetItem sealed : GazeTargetItem
{
internal:

    ExpandCollapseGazeTargetItem(UIElement^ element)
        : GazeTargetItem(element)
    {
    }

    void Invoke() override
    {
        auto peer = FrameworkElementAutomationPeer::FromElement(TargetElement);
        auto expandCollapseProvider = safe_cast<IExpandCollapseProvider ^>(peer);
        switch (expandCollapseProvider->ExpandCollapseState)
        {
        case ExpandCollapseState::Collapsed:
            expandCollapseProvider->Expand();
            break;

        case ExpandCollapseState::Expanded:
            expandCollapseProvider->Collapse();
            break;
        }
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

        auto invokeProvider = dynamic_cast<IInvokeProvider^>(peer);
        if (invokeProvider != nullptr)
        {
            item = ref new InvokeGazeTargetItem(element);
        }
        else
        {
            auto toggleProvider = dynamic_cast<IToggleProvider^>(peer);
            if (toggleProvider != nullptr)
            {
                item = ref new ToggleGazeTargetItem(element);
            }
            else
            {
                auto selectionItemProvider = dynamic_cast<ISelectionItemProvider^>(peer);
                if (selectionItemProvider != nullptr)
                {
                    item = ref new SelectionGazeTargetItem(element);
                }
                else
                {
                    auto expandCollapseProvider = dynamic_cast<IExpandCollapseProvider^>(peer);
                    if (expandCollapseProvider != nullptr)
                    {
                        item = ref new ExpandCollapseGazeTargetItem(element);
                    }
                    else
                    {
                        item = GazePointer::Instance->_nonInvokeGazeTargetItem;
                    }
                }
            }
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