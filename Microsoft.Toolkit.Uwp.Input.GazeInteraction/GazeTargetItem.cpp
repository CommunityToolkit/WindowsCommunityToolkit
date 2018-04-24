//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeTargetItem.h"
#include "GazeFeedbackPopupFactory.h"
#include "GazeElement.h"

using namespace std;
using namespace Platform;
using namespace Windows::Foundation::Collections;
using namespace Windows::Graphics::Display;
using namespace Windows::UI;
using namespace Windows::UI::ViewManagement;
using namespace Windows::UI::Xaml::Automation::Peers;
using namespace Windows::UI::Xaml::Hosting;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Automation;
using namespace Windows::UI::Xaml::Automation::Provider;
using namespace Windows::UI::Xaml::Automation::Peers;

BEGIN_NAMESPACE_GAZE_INPUT

static DependencyProperty^ GazeTargetItemProperty = DependencyProperty::RegisterAttached("_GazeTargetItem", GazeTargetItem::typeid, GazeTargetItem::typeid, ref new PropertyMetadata(nullptr));

ref class InvokeGazeTargetItem sealed : public GazeTargetItem
{
private:

    IInvokeProvider ^ _invokeProvider;

internal:

    InvokeGazeTargetItem(UIElement^ element, IInvokeProvider^ invokeProvider)
        : GazeTargetItem(element)
    {
        _invokeProvider = invokeProvider;
    }

    void Invoke() override
    {
        _invokeProvider->Invoke();
    }
};

ref class ToggleGazeTargetItem sealed : public GazeTargetItem
{
private:

    IToggleProvider ^ _toggleProvider;

internal:

    ToggleGazeTargetItem(UIElement^ element, IToggleProvider^ toggleProvider)
        : GazeTargetItem(element)
    {
        _toggleProvider = toggleProvider;
    }

    void Invoke() override
    {
        _toggleProvider->Toggle();
    }
};

ref class SelectionGazeTargetItem sealed : public GazeTargetItem
{
private:

    ISelectionItemProvider ^ _selectionItemProvider;

internal:

    SelectionGazeTargetItem(UIElement^ element, ISelectionItemProvider^ selectionItemProvider)
        : GazeTargetItem(element)
    {
        _selectionItemProvider = selectionItemProvider;
    }

    void Invoke() override
    {
        _selectionItemProvider->Select();
    }
};

ref class NonInvokeGazeTargetItem sealed : GazeTargetItem
{
private:

    NonInvokeGazeTargetItem()
        : GazeTargetItem(ref new Page())
    {
    }

internal:

    static property GazeTargetItem^ Instance
    {
        GazeTargetItem^ get()
        {
            static GazeTargetItem^ s_gazeTargetItem = ref new NonInvokeGazeTargetItem();
            return s_gazeTargetItem;
        }
    }

    virtual property bool IsInvokable { bool get() override { return false; } }

    void Invoke() override
    {
    }
};

ref class ExpandCollapseGazeTargetItem sealed : GazeTargetItem
{
private:

    IExpandCollapseProvider ^ _expandCollapseProvider;

internal:

    ExpandCollapseGazeTargetItem(UIElement^ element, IExpandCollapseProvider^ expandCollapseProvider)
        : GazeTargetItem(element)
    {
        _expandCollapseProvider = expandCollapseProvider;
    }

    void Invoke() override
    {
        switch (_expandCollapseProvider->ExpandCollapseState)
        {
        case ExpandCollapseState::Collapsed:
            _expandCollapseProvider->Expand();
            break;

        case ExpandCollapseState::Expanded:
            _expandCollapseProvider->Collapse();
            break;
        }
    }
};

GazeTargetItem^ GazeTargetItem::NonInvokable::get() { return NonInvokeGazeTargetItem::Instance; }

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
            item = ref new InvokeGazeTargetItem(element, invokeProvider);
        }
        else
        {
            auto toggleProvider = dynamic_cast<IToggleProvider^>(peer);
            if (toggleProvider != nullptr)
            {
                item = ref new ToggleGazeTargetItem(element, toggleProvider);
            }
            else
            {
                auto selectionItemProvider = dynamic_cast<ISelectionItemProvider^>(peer);
                if (selectionItemProvider != nullptr)
                {
                    item = ref new SelectionGazeTargetItem(element, selectionItemProvider);
                }
                else
                {
                    auto expandCollapseProvider = dynamic_cast<IExpandCollapseProvider^>(peer);
                    if (expandCollapseProvider != nullptr)
                    {
                        item = ref new ExpandCollapseGazeTargetItem(element, expandCollapseProvider);
                    }
                    else
                    {
                        item = NonInvokeGazeTargetItem::Instance;
                    }
                }
            }
        }
        element->SetValue(GazeTargetItemProperty, item);
    }

    return item;
}

void GazeTargetItem::RaiseProgressEvent(GazeProgressState state)
{
    // TODO: We should eliminate non-invokable controls before we arrive here!
    if (dynamic_cast<Page^>(TargetElement) != nullptr)
    {
        return;
    }

    if (_notifiedProgressState != state || state == GazeProgressState::Progressing)
    {
        auto handled = false;

        auto gazeElement = GazeInput::GetGazeElement(TargetElement);
        if (gazeElement != nullptr)
        {
            handled = gazeElement->RaiseProgressFeedback(TargetElement, state, ElapsedTime - _prevStateTime, _nextStateTime - _prevStateTime);
        }

        if (!handled && state != GazeProgressState::Idle)
        {
            if (_feedbackPopup == nullptr)
            {
                _feedbackPopup = GazeFeedbackPopupFactory::Get();
            }

            auto control = safe_cast<Control^>(TargetElement);

            auto transform = control->TransformToVisual(_feedbackPopup);
            auto bounds = transform->TransformBounds(*ref new Rect(*ref new Point(0, 0),
                *ref new Size(safe_cast<float>(control->ActualWidth), safe_cast<float>(control->ActualHeight))));
            auto rectangle = safe_cast<Rectangle^>(_feedbackPopup->Child);

            if (state == GazeProgressState::Progressing)
            {
                auto progress = ((double)(ElapsedTime - _prevStateTime).Duration) / (_nextStateTime - _prevStateTime).Duration;

                if (0 <= progress && progress < 1)
                {
                    rectangle->Stroke = GazeInput::GazeFeedbackProgressBrush;
                    rectangle->Width = (1 - progress) * bounds.Width;
                    rectangle->Height = (1 - progress) * bounds.Height;

                    _feedbackPopup->HorizontalOffset = bounds.Left + progress * bounds.Width / 2;
                    _feedbackPopup->VerticalOffset = bounds.Top + progress * bounds.Height / 2;
                }
            }
            else
            {
                rectangle->Stroke = GazeInput::GazeFeedbackCompleteBrush;
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
                GazeFeedbackPopupFactory::Return(_feedbackPopup);
                _feedbackPopup = nullptr;
            }
        }
    }

    _notifiedProgressState = state;
}

END_NAMESPACE_GAZE_INPUT