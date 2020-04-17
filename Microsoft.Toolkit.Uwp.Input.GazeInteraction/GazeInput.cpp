//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include <winrt/Microsoft.UI.Xaml.Interop.h>
#include "GazeInput.h"
#include "GazeInput.g.cpp"
#include "GazeElement.h"
#include "GazePointer.h"
#include "GazePointerProxy.h"
#include "GazeTargetItem.h"

using namespace winrt::Windows::Foundation::Collections;
using namespace winrt::Microsoft::UI;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    bool GazeInput::IsDeviceAvailable()
    {
        return GazePointer::Instance().IsDeviceAvailable();
    }
    Microsoft::Toolkit::Uwp::Input::GazeInteraction::Interaction GazeInput::Interaction()
    {
        auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
        return impl->_interaction;
    }
    void GazeInput::Interaction(Microsoft::Toolkit::Uwp::Input::GazeInteraction::Interaction const& value)
    {
        auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());

        if (impl->_interaction != value)
        {
            if (value == GazeInteraction::Interaction::Enabled)
            {
                GazePointer::Instance().AddRoot(0);
            }
            else if (impl->_interaction == GazeInteraction::Interaction::Enabled)
            {
                GazePointer::Instance().RemoveRoot(0);
            }

            impl->_interaction = value;
        }
    }

    Brush GazeInput::DwellFeedbackEnterBrush()
    {
        auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
        return impl->_enterBrush;
    }

    void GazeInput::DwellFeedbackEnterBrush(Brush const& value)
    {
        auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
        impl->_enterBrush = value;
    }

    Brush GazeInput::DwellFeedbackProgressBrush()
    {
        auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
        return impl->_progressBrush;
    }

    void GazeInput::DwellFeedbackProgressBrush(Brush const& value)
    {
        auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
        impl->_progressBrush = value;
    }

    Brush GazeInput::DwellFeedbackCompleteBrush()
    {
        auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
        return impl->_completeBrush;
    }

    void GazeInput::DwellFeedbackCompleteBrush(Brush const& value)
    {
        auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
        impl->_completeBrush = value;
    }

    double GazeInput::DwellStrokeThickness()
    {
        auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
        return impl->m_dwellStrokeThickness;
    }

    void GazeInput::DwellStrokeThickness(double value)
    {
        auto impl = winrt::get_self<GazePointer>(GazePointer::Instance());
        impl->m_dwellStrokeThickness = value;
    }

    static DependencyProperty s_interactionProperty = DependencyProperty::RegisterAttached(L"Interaction", xaml_typename<Interaction>(), xaml_typename<GazeInput>(),
        PropertyMetadata(winrt::box_value(Interaction::Inherited), PropertyChangedCallback{ &GazeInput::OnInteractionChanged }));
    static DependencyProperty s_isCursorVisibleProperty = DependencyProperty::RegisterAttached(L"IsCursorVisible", xaml_typename<bool>(), xaml_typename<GazeInput>(),
        PropertyMetadata(winrt::box_value(true), PropertyChangedCallback{ &GazeInput::OnIsCursorVisibleChanged }));
    static DependencyProperty s_cursorRadiusProperty = DependencyProperty::RegisterAttached(L"CursorRadius", xaml_typename<int>(), xaml_typename<GazeInput>(),
        PropertyMetadata(winrt::box_value(6), PropertyChangedCallback{ &GazeInput::OnCursorRadiusChanged }));
    static DependencyProperty s_gazeElementProperty = DependencyProperty::RegisterAttached(L"GazeElement", xaml_typename<GazeElement>(), xaml_typename<GazeInput>(), PropertyMetadata(nullptr));
    static DependencyProperty s_fixationDurationProperty = DependencyProperty::RegisterAttached(L"FixationDuration", xaml_typename<TimeSpan>(), xaml_typename<GazeInput>(), PropertyMetadata(winrt::box_value(GazeInput::UnsetTimeSpan)));
    static DependencyProperty s_dwellDurationProperty = DependencyProperty::RegisterAttached(L"DwellDuration", xaml_typename<TimeSpan>(), xaml_typename<GazeInput>(), PropertyMetadata(winrt::box_value(GazeInput::UnsetTimeSpan)));
    static DependencyProperty s_repeatDelayDurationProperty = DependencyProperty::RegisterAttached(L"RepeatDelayDuration", xaml_typename<TimeSpan>(), xaml_typename<GazeInput>(), PropertyMetadata(winrt::box_value(GazeInput::UnsetTimeSpan)));
    static DependencyProperty s_dwellRepeatDurationProperty = DependencyProperty::RegisterAttached(L"DwellRepeatDuration", xaml_typename<TimeSpan>(), xaml_typename<GazeInput>(), PropertyMetadata(winrt::box_value(GazeInput::UnsetTimeSpan)));
    static DependencyProperty s_thresholdDurationProperty = DependencyProperty::RegisterAttached(L"ThresholdDuration", xaml_typename<TimeSpan>(), xaml_typename<GazeInput>(), PropertyMetadata(winrt::box_value(GazeInput::UnsetTimeSpan)));
    static DependencyProperty s_maxRepeatCountProperty = DependencyProperty::RegisterAttached(L"MaxDwellRepeatCount", xaml_typename<int>(), xaml_typename<GazeInput>(), PropertyMetadata(winrt::box_value(0)));
    static DependencyProperty s_isSwitchEnabledProperty = DependencyProperty::RegisterAttached(L"IsSwitchEnabled", xaml_typename<bool>(), xaml_typename<GazeInput>(),
        PropertyMetadata(false, PropertyChangedCallback{ &GazeInput::OnIsSwitchEnabledChanged }));

    DependencyProperty GazeInput::InteractionProperty() { return s_interactionProperty; }
    DependencyProperty GazeInput::IsCursorVisibleProperty() { return s_isCursorVisibleProperty; }
    DependencyProperty GazeInput::CursorRadiusProperty() { return s_cursorRadiusProperty; }
    DependencyProperty GazeInput::GazeElementProperty() { return s_gazeElementProperty; }
    DependencyProperty GazeInput::FixationDurationProperty() { return s_fixationDurationProperty; }
    DependencyProperty GazeInput::DwellDurationProperty() { return s_dwellDurationProperty; }
    DependencyProperty GazeInput::RepeatDelayDurationProperty() { return s_repeatDelayDurationProperty; }
    DependencyProperty GazeInput::DwellRepeatDurationProperty() { return s_dwellRepeatDurationProperty; }
    DependencyProperty GazeInput::ThresholdDurationProperty() { return s_thresholdDurationProperty; }
    DependencyProperty GazeInput::MaxDwellRepeatCountProperty() { return s_maxRepeatCountProperty; }
    DependencyProperty GazeInput::IsSwitchEnabledProperty() { return s_isSwitchEnabledProperty; }
    
    Interaction GazeInput::GetInteraction(UIElement const& element) { return winrt::unbox_value<GazeInteraction::Interaction>(element.GetValue(s_interactionProperty)); }
    bool GazeInput::GetIsCursorVisible(UIElement const& element) { return winrt::unbox_value<bool>(element.GetValue(s_isCursorVisibleProperty)); }
    int GazeInput::GetCursorRadius(UIElement const& element) { return winrt::unbox_value<int>(element.GetValue(s_cursorRadiusProperty)); }
    Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeElement GazeInput::GetGazeElement(UIElement const& element) { return element.GetValue(s_gazeElementProperty).try_as<Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeElement>(); }
    TimeSpan GazeInput::GetFixationDuration(UIElement const& element) { return winrt::unbox_value<TimeSpan>(element.GetValue(s_fixationDurationProperty)); }
    TimeSpan GazeInput::GetDwellDuration(UIElement const& element) { return winrt::unbox_value<TimeSpan>(element.GetValue(s_dwellDurationProperty)); }
    TimeSpan GazeInput::GetRepeatDelayDuration(UIElement const& element) { return winrt::unbox_value<TimeSpan>(element.GetValue(s_repeatDelayDurationProperty)); }
    TimeSpan GazeInput::GetDwellRepeatDuration(UIElement const& element) { return winrt::unbox_value<TimeSpan>(element.GetValue(s_dwellRepeatDurationProperty)); }
    TimeSpan GazeInput::GetThresholdDuration(UIElement const& element) { return winrt::unbox_value<TimeSpan>(element.GetValue(s_thresholdDurationProperty)); }
    int GazeInput::GetMaxDwellRepeatCount(UIElement const& element) { return winrt::unbox_value<int>(element.GetValue(s_maxRepeatCountProperty)); }
    bool GazeInput::GetIsSwitchEnabled(UIElement const& element) { return winrt::unbox_value<bool>(element.GetValue(s_isSwitchEnabledProperty)); }

    void GazeInput::SetInteraction(UIElement const& element, GazeInteraction::Interaction const& value) { element.SetValue(s_interactionProperty, winrt::box_value(value)); }
    void GazeInput::SetIsCursorVisible(UIElement const& element, bool value) { element.SetValue(s_isCursorVisibleProperty, winrt::box_value(value)); }
    void GazeInput::SetCursorRadius(UIElement const& element, int value) { element.SetValue(s_cursorRadiusProperty, winrt::box_value(value)); }
    void GazeInput::SetGazeElement(UIElement const& element, Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeElement const& value) { element.SetValue(s_gazeElementProperty, value); }
    void GazeInput::SetFixationDuration(UIElement const& element, TimeSpan const& span) { element.SetValue(s_fixationDurationProperty, winrt::box_value(span)); }
    void GazeInput::SetDwellDuration(UIElement const& element, TimeSpan const& span) { element.SetValue(s_dwellDurationProperty, winrt::box_value(span)); }
    void GazeInput::SetRepeatDelayDuration(UIElement const& element, TimeSpan const& span) { element.SetValue(s_repeatDelayDurationProperty, winrt::box_value(span)); }
    void GazeInput::SetDwellRepeatDuration(UIElement const& element, TimeSpan const& span) { element.SetValue(s_dwellRepeatDurationProperty, winrt::box_value(span)); }
    void GazeInput::SetThresholdDuration(UIElement const& element, TimeSpan const& span) { element.SetValue(s_thresholdDurationProperty, winrt::box_value(span)); }
    void GazeInput::SetMaxDwellRepeatCount(UIElement const& element, int value) { element.SetValue(s_maxRepeatCountProperty, winrt::box_value(value)); }
    void GazeInput::SetSettIsSwitchEnabled(UIElement const& element, bool value) { element.SetValue(s_isSwitchEnabledProperty, winrt::box_value(value)); }

    void GazeInput::Invoke(UIElement element)
    {
        auto item = GazeTargetItem::GetOrCreate(element);
        item.Invoke();
    }

    void GazeInput::LoadSettings(ValueSet settings)
    {
        GazePointer::Instance().LoadSettings(settings);
    }

    TimeSpan GazeInput::UnsetTimeSpan = TimeSpan(-1);

    winrt::event_token GazeInput::IsDeviceAvailableChanged(winrt::Windows::Foundation::EventHandler<IInspectable> const& handler)
    {
        return GazePointer::Instance().IsDeviceAvailableChanged(handler);
    }

    void GazeInput::IsDeviceAvailableChanged(winrt::event_token const& token) noexcept
    {
        GazePointer::Instance().IsDeviceAvailableChanged(token);
    }

    Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazePointer GazeInput::GetGazePointer(Microsoft::UI::Xaml::Controls::Page const& page)
    {
        return GazePointer::Instance();
    }
}
