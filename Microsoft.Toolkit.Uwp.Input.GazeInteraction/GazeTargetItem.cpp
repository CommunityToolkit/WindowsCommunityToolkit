//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include <winrt/Microsoft.UI.Xaml.Interop.h>
#include <winrt/Microsoft.UI.Xaml.Automation.h>
#include <winrt/Microsoft.UI.Xaml.Automation.Provider.h>
#include <winrt/Windows.Foundation.Collections.h>
#include "GazeTargetItem.h"
#include "GazeTargetItem.g.cpp"
#include "GazePointer.h"
#include "GazeElement.h"
#include "GazeFeedbackPopupFactory.h"
#include "TogglePatternGazeTargetItem.h"
#include "InvokePatternGazeTargetItem.h"
#include "SelectionItemPatternGazeTargetItem.h"
#include "ExpandCollapsePatternGazeTargetItem.h"
#include "ComboBoxItemGazeTargetItem.h"
#include "PivotItemGazeTargetItem.h"

using namespace winrt::Microsoft::UI::Xaml::Automation;
using namespace winrt::Microsoft::UI::Xaml::Automation::Provider;
using namespace winrt::Microsoft::UI::Xaml::Automation::Peers;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    GazeTargetItem::GazeTargetItem(Microsoft::UI::Xaml::UIElement const& target) :
        _targetElement(target)
    {
    }

    Windows::Foundation::TimeSpan GazeTargetItem::DetailedTime()
    {
        return _detailedTime;
    }

    void GazeTargetItem::DetailedTime(Windows::Foundation::TimeSpan const& value)
    {
        _detailedTime = value;
    }

    Windows::Foundation::TimeSpan GazeTargetItem::OverflowTime()
    {
        return _overflowTime;
    }

    void GazeTargetItem::OverflowTime(Windows::Foundation::TimeSpan const& value)
    {
        _overflowTime = value;
    }

    Windows::Foundation::TimeSpan GazeTargetItem::ElapsedTime()
    {
        return DetailedTime() + OverflowTime();
    }

    Windows::Foundation::TimeSpan GazeTargetItem::NextStateTime()
    {
        return _nextStateTime;
    }

    void GazeTargetItem::NextStateTime(Windows::Foundation::TimeSpan const& value)
    {
        _nextStateTime = value;
    }

    Windows::Foundation::TimeSpan GazeTargetItem::LastTimestamp()
    {
        return _lastTimestamp;
    }

    void GazeTargetItem::LastTimestamp(Windows::Foundation::TimeSpan const& value)
    {
        _lastTimestamp = value;
    }

    Microsoft::Toolkit::Uwp::Input::GazeInteraction::PointerState GazeTargetItem::ElementState()
    {
        return _elementState;
    }

    void GazeTargetItem::ElementState(Microsoft::Toolkit::Uwp::Input::GazeInteraction::PointerState const& value)
    {
        _elementState = value;
    }

    Microsoft::UI::Xaml::UIElement GazeTargetItem::TargetElement()
    {
        return _targetElement;
    }

    void GazeTargetItem::TargetElement(Microsoft::UI::Xaml::UIElement const& value)
    {
        _targetElement = value;
    }

    int32_t GazeTargetItem::RepeatCount()
    {
        return _repeatCount;
    }

    void GazeTargetItem::RepeatCount(int32_t value)
    {
        _repeatCount = value;
    }

    int32_t GazeTargetItem::MaxDwellRepeatCount()
    {
        return _maxDwellRepeatCount;
    }

    void GazeTargetItem::MaxDwellRepeatCount(int32_t value)
    {
        _maxDwellRepeatCount = value;
    }

    void GazeTargetItem::Invoke()
    {
        // This should be pure virtual
    }

    bool GazeTargetItem::IsInvokable()
    {
        return true;
    }

    void GazeTargetItem::Reset(Windows::Foundation::TimeSpan const& nextStateTime)
    {
        ElementState(PointerState::PreEnter);
        DetailedTime(TimeSpanZero);
        OverflowTime(TimeSpanZero);
        NextStateTime(nextStateTime);
        RepeatCount(0);
        MaxDwellRepeatCount(GazeInput::GetMaxDwellRepeatCount(TargetElement()));
    }

    void GazeTargetItem::GiveFeedback()
    {
        if (_nextStateTime != NextStateTime())
        {
            _prevStateTime = _nextStateTime;
            _nextStateTime = NextStateTime();
        }

        if (ElementState() != _notifiedPointerState)
        {
            switch (ElementState())
            {
            case PointerState::Enter:
                RaiseProgressEvent(DwellProgressState::Fixating);
                break;

            case PointerState::Dwell:
            case PointerState::Fixation:
                RaiseProgressEvent(DwellProgressState::Progressing);
                break;

            case PointerState::Exit:
            case PointerState::PreEnter:
                RaiseProgressEvent(DwellProgressState::Idle);
                break;
            }

            _notifiedPointerState = ElementState();
        }
        else if (ElementState() == PointerState::Dwell || ElementState() == PointerState::Fixation)
        {
            if (RepeatCount() <= MaxDwellRepeatCount())
            {
                RaiseProgressEvent(DwellProgressState::Progressing);
            }
            else
            {
                RaiseProgressEvent(DwellProgressState::Complete);
            }
        }
    }

    DependencyProperty GazeTargetItem::m_gazeTargetItemProperty = DependencyProperty::RegisterAttached(L"_GazeTargetItem", xaml_typename<GazeTargetItem>(), xaml_typename<GazeTargetItem>(), PropertyMetadata(nullptr));

    Microsoft::UI::Xaml::DependencyProperty GazeTargetItem::GazeTargetItemProperty()
    {
        return m_gazeTargetItemProperty;
    }

    Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem GazeTargetItem::GetOrCreate(Microsoft::UI::Xaml::UIElement const& element)
    {
        winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem item{ nullptr };

        auto value = element.ReadLocalValue(GazeTargetItemProperty());

        if (value != DependencyProperty::UnsetValue())
        {
            value.try_as<winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem>(item);
        }
        else
        {
            auto peer = FrameworkElementAutomationPeer::FromElement(element);

            if (peer == nullptr)
            {
                if (element.try_as<PivotHeaderItem>())
                {
                    item = winrt::make<PivotItemGazeTargetItem>(element);
                }
                else
                {
                    auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
                    item = impl->_nonInvokeGazeTargetItem;
                }
            }
            else if (peer.GetPattern(PatternInterface::Invoke).try_as<IInvokeProvider>())
            {
                item = winrt::make<InvokePatternGazeTargetItem>(element);
            }
            else if (peer.GetPattern(PatternInterface::Toggle).try_as<IToggleProvider>())
            {
                item = winrt::make<TogglePatternGazeTargetItem>(element);
            }
            else if (peer.GetPattern(PatternInterface::SelectionItem).try_as<ISelectionItemProvider>())
            {
                item = winrt::make<SelectionItemPatternGazeTargetItem>(element);
            }
            else if (peer.GetPattern(PatternInterface::ExpandCollapse).try_as<IExpandCollapseProvider>())
            {
                item = winrt::make<ExpandCollapsePatternGazeTargetItem>(element);
            }
            else if (peer.try_as<ComboBoxItemAutomationPeer>())
            {
                item = winrt::make<ComboBoxItemGazeTargetItem>(element);
            }
            else
            {
                auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
                item = impl->_nonInvokeGazeTargetItem;
            }

            element.SetValue(GazeTargetItemProperty(), winrt::box_value(item));
        }

        return item;
    }

    void GazeTargetItem::RaiseProgressEvent(DwellProgressState state)
    {
        // TODO: We should eliminate non-invokable controls before we arrive here!
        if (TargetElement().try_as<Page>())
        {
            return;
        }

        if (_notifiedProgressState != state || state == DwellProgressState::Progressing)
        {
            auto handled = false;

            auto gazeElement = GazeInput::GetGazeElement(TargetElement());
            if (gazeElement != nullptr)
            {
                handled = gazeElement.RaiseProgressFeedback(TargetElement(), state, ElapsedTime() - _prevStateTime, _nextStateTime - _prevStateTime);
            }

            if (!handled && state != DwellProgressState::Idle)
            {
                if (_feedbackPopup == nullptr)
                {
                    auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
                    _feedbackPopup = impl->_gazeFeedbackPopupFactory.Get();
                }

                FrameworkElement control{ TargetElement().try_as<FrameworkElement>() };

                auto transform = control.TransformToVisual(_feedbackPopup);
                auto bounds = transform.TransformBounds(Rect(0, 0, (float)control.ActualWidth(), (float)control.ActualHeight()));
                winrt::Microsoft::UI::Xaml::Shapes::Rectangle rectangle{ _feedbackPopup.Child().try_as<winrt::Microsoft::UI::Xaml::Shapes::Rectangle>() };

                if (state == DwellProgressState::Progressing)
                {
                    auto progress = ((double)(ElapsedTime() - _prevStateTime).count()) / (_nextStateTime - _prevStateTime).count();

                    if (0 <= progress && progress < 1)
                    {
                        rectangle.Stroke(GazeInput::DwellFeedbackProgressBrush());
                        rectangle.Width((1 - progress) * bounds.Width);
                        rectangle.Height((1 - progress) * bounds.Height);

                        _feedbackPopup.HorizontalOffset(bounds.X + progress * bounds.Width / 2);
                        _feedbackPopup.VerticalOffset(bounds.Y + progress * bounds.Height / 2);
                    }
                }
                else
                {
                    rectangle.Stroke(state == DwellProgressState::Fixating ?
                        GazeInput::DwellFeedbackEnterBrush() : GazeInput::DwellFeedbackCompleteBrush());
                    rectangle.Width(bounds.Width);
                    rectangle.Height(bounds.Height);

                    _feedbackPopup.HorizontalOffset(bounds.X);
                    _feedbackPopup.VerticalOffset(bounds.Y);
                }

                _feedbackPopup.IsOpen(true);
            }
            else
            {
                if (_feedbackPopup != nullptr)
                {
                    auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
                    impl->_gazeFeedbackPopupFactory.Return(_feedbackPopup);
                    _feedbackPopup = nullptr;
                }
            }
        }

        _notifiedProgressState = state;
    }
}