//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeInput.h"

#include "GazeElement.h"
#include "GazePointer.h"
#include "GazePointerProxy.h"

using namespace Platform;
using namespace Windows::UI;

BEGIN_NAMESPACE_GAZE_INPUT

static Brush^ s_enterBrush = nullptr;

Brush^ GazeInput::DwellFeedbackEnterBrush::get()
{
    return s_enterBrush;
}

void GazeInput::DwellFeedbackEnterBrush::set(Brush^ value)
{
    s_enterBrush = value;
}

static Brush^ s_progressBrush = ref new SolidColorBrush(Colors::Green);

Brush^ GazeInput::DwellFeedbackProgressBrush::get()
{
    return s_progressBrush;
}

void GazeInput::DwellFeedbackProgressBrush::set(Brush^ value)
{
    s_progressBrush = value;
}

static Brush^ s_completeBrush = ref new SolidColorBrush(Colors::Red);

Brush^ GazeInput::DwellFeedbackCompleteBrush::get()
{
    return s_completeBrush;
}

void GazeInput::DwellFeedbackCompleteBrush::set(Brush^ value)
{
    s_completeBrush = value;
}

TimeSpan GazeInput::UnsetTimeSpan = { -1 };

static void OnInteractionChanged(DependencyObject^ ob, DependencyPropertyChangedEventArgs^ args)
{
    auto element = safe_cast<FrameworkElement^>(ob);
    auto interaction = safe_cast<Interaction>(args->NewValue);
    GazePointerProxy::SetGazeInteraction(element, interaction);
}

static void OnIsCursorVisibleChanged(DependencyObject^ ob, DependencyPropertyChangedEventArgs^ args)
{
    GazePointer::Instance->IsCursorVisible = safe_cast<bool>(args->NewValue);
}

static void OnCursorRadiusChanged(DependencyObject^ ob, DependencyPropertyChangedEventArgs^ args)
{
    GazePointer::Instance->CursorRadius = safe_cast<int>(args->NewValue);
}

static DependencyProperty^ s_interactionProperty = DependencyProperty::RegisterAttached("Interaction", Interaction::typeid, GazeInput::typeid,
    ref new PropertyMetadata(Interaction::Inherited, ref new PropertyChangedCallback(&OnInteractionChanged)));
static DependencyProperty^ s_isCursorVisibleProperty = DependencyProperty::RegisterAttached("IsCursorVisible", bool::typeid, GazeInput::typeid,
    ref new PropertyMetadata(true, ref new PropertyChangedCallback(&OnIsCursorVisibleChanged)));
static DependencyProperty^ s_cursorRadiusProperty = DependencyProperty::RegisterAttached("CursorRadius", int::typeid, GazeInput::typeid,
    ref new PropertyMetadata(6, ref new PropertyChangedCallback(&OnCursorRadiusChanged)));
static DependencyProperty^ s_gazeElementProperty = DependencyProperty::RegisterAttached("GazeElement", GazeElement::typeid, GazeInput::typeid, ref new PropertyMetadata(nullptr));
static DependencyProperty^ s_fixationDurationProperty = DependencyProperty::RegisterAttached("FixationDuration", TimeSpan::typeid, GazeInput::typeid, ref new PropertyMetadata(GazeInput::UnsetTimeSpan));
static DependencyProperty^ s_dwellDurationProperty = DependencyProperty::RegisterAttached("DwellDuration", TimeSpan::typeid, GazeInput::typeid, ref new PropertyMetadata(GazeInput::UnsetTimeSpan));
static DependencyProperty^ s_repeatDelayDurationProperty = DependencyProperty::RegisterAttached("RepeatDelayDuration", TimeSpan::typeid, GazeInput::typeid, ref new PropertyMetadata(GazeInput::UnsetTimeSpan));
static DependencyProperty^ s_dwellRepeatDurationProperty = DependencyProperty::RegisterAttached("DwellRepeatDuration", TimeSpan::typeid, GazeInput::typeid, ref new PropertyMetadata(GazeInput::UnsetTimeSpan));
static DependencyProperty^ s_thresholdDurationProperty = DependencyProperty::RegisterAttached("ThresholdDuration", TimeSpan::typeid, GazeInput::typeid, ref new PropertyMetadata(GazeInput::UnsetTimeSpan));
static DependencyProperty^ s_maxRepeatCountProperty = DependencyProperty::RegisterAttached("MaxDwellRepeatCount", int::typeid, GazeInput::typeid, ref new PropertyMetadata(safe_cast<Object^>(0)));

DependencyProperty^ GazeInput::InteractionProperty::get() { return s_interactionProperty; }
DependencyProperty^ GazeInput::IsCursorVisibleProperty::get() { return s_isCursorVisibleProperty; }
DependencyProperty^ GazeInput::CursorRadiusProperty::get() { return s_cursorRadiusProperty; }
DependencyProperty^ GazeInput::GazeElementProperty::get() { return s_gazeElementProperty; }
DependencyProperty^ GazeInput::FixationDurationProperty::get() { return s_fixationDurationProperty; }
DependencyProperty^ GazeInput::DwellDurationProperty::get() { return s_dwellDurationProperty; }
DependencyProperty^ GazeInput::RepeatDelayDurationProperty::get() { return s_repeatDelayDurationProperty; }
DependencyProperty^ GazeInput::DwellRepeatDurationProperty::get() { return s_dwellRepeatDurationProperty; }
DependencyProperty^ GazeInput::ThresholdDurationProperty::get() { return s_thresholdDurationProperty; }
DependencyProperty^ GazeInput::MaxDwellRepeatCountProperty::get() { return s_maxRepeatCountProperty; }

Interaction GazeInput::GetInteraction(UIElement^ element) { return safe_cast<Interaction>(element->GetValue(s_interactionProperty)); }
bool GazeInput::GetIsCursorVisible(UIElement^ element) { return safe_cast<bool>(element->GetValue(s_isCursorVisibleProperty)); }
int GazeInput::GetCursorRadius(UIElement^ element) { return safe_cast<int>(element->GetValue(s_cursorRadiusProperty)); }
GazeElement^ GazeInput::GetGazeElement(UIElement^ element) { return safe_cast<GazeElement^>(element->GetValue(s_gazeElementProperty)); }
TimeSpan GazeInput::GetFixationDuration(UIElement^ element) { return safe_cast<TimeSpan>(element->GetValue(s_fixationDurationProperty)); }
TimeSpan GazeInput::GetDwellDuration(UIElement^ element) { return safe_cast<TimeSpan>(element->GetValue(s_dwellDurationProperty)); }
TimeSpan GazeInput::GetRepeatDelayDuration(UIElement^ element) { return safe_cast<TimeSpan>(element->GetValue(s_repeatDelayDurationProperty)); }
TimeSpan GazeInput::GetDwellRepeatDuration(UIElement^ element) { return safe_cast<TimeSpan>(element->GetValue(s_dwellRepeatDurationProperty)); }
TimeSpan GazeInput::GetThresholdDuration(UIElement^ element) { return safe_cast<TimeSpan>(element->GetValue(s_thresholdDurationProperty)); }
int GazeInput::GetMaxDwellRepeatCount(UIElement^ element) { return safe_cast<int>(element->GetValue(s_maxRepeatCountProperty)); }

void GazeInput::SetInteraction(UIElement^ element, Interaction value) { element->SetValue(s_interactionProperty, value); }
void GazeInput::SetIsCursorVisible(UIElement^ element, bool value) { element->SetValue(s_isCursorVisibleProperty, value); }
void GazeInput::SetCursorRadius(UIElement^ element, int value) { element->SetValue(s_cursorRadiusProperty, value); }
void GazeInput::SetGazeElement(UIElement^ element, GazeElement^ value) { element->SetValue(s_gazeElementProperty, value); }
void GazeInput::SetFixationDuration(UIElement^ element, TimeSpan span) { element->SetValue(s_fixationDurationProperty, span); }
void GazeInput::SetDwellDuration(UIElement^ element, TimeSpan span) { element->SetValue(s_dwellDurationProperty, span); }
void GazeInput::SetRepeatDelayDuration(UIElement^ element, TimeSpan span) { element->SetValue(s_repeatDelayDurationProperty, span); }
void GazeInput::SetDwellRepeatDuration(UIElement^ element, TimeSpan span) { element->SetValue(s_dwellRepeatDurationProperty, span); }
void GazeInput::SetThresholdDuration(UIElement^ element, TimeSpan span) { element->SetValue(s_thresholdDurationProperty, span); }
void GazeInput::SetMaxDwellRepeatCount(UIElement^ element, int value) { element->SetValue(s_maxRepeatCountProperty, value); }

GazePointer^ GazeInput::GetGazePointer(Page^ page)
{
    return GazePointer::Instance;
}

bool GazeInput::IsDeviceAvailable::get()
{
    return GazePointer::Instance->IsDeviceAvailable; 
}

EventRegistrationToken GazeInput::IsDeviceAvailableChanged::add(EventHandler<Object^>^ handler)
{ 
    return GazePointer::Instance->IsDeviceAvailableChanged += handler; 
}

void GazeInput::IsDeviceAvailableChanged::remove(EventRegistrationToken token) 
{ 
    GazePointer::Instance->IsDeviceAvailableChanged -= token; 
}

END_NAMESPACE_GAZE_INPUT