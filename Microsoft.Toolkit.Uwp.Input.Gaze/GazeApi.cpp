//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazeApi.h"
#include "GazePointer.h"
#include "GazeElement.h"

using namespace std;
using namespace Platform;
using namespace Windows::Foundation::Collections;
using namespace Windows::Graphics::Display;
using namespace Windows::UI;
using namespace Windows::UI::ViewManagement;
using namespace Windows::UI::Xaml::Automation::Peers;
using namespace Windows::UI::Xaml::Hosting;

BEGIN_NAMESPACE_GAZE_INPUT

TimeSpan GazeApi::UnsetTimeSpan = { -1 };

static DependencyProperty^ s_gazePointerProperty = DependencyProperty::RegisterAttached("_GazePointer", GazePointer::typeid, GazeApi::typeid, ref new PropertyMetadata(nullptr));

DependencyProperty^ GazeApi::GazePointerProperty::get() { return s_gazePointerProperty; }

static void OnIsGazeEnabledChanged(DependencyObject^ ob, DependencyPropertyChangedEventArgs^ args)
{
    auto isGazeEnabled = safe_cast<bool>(args->NewValue);
    if (isGazeEnabled)
    {
        auto page = safe_cast<Page^>(ob);

        auto gazePointer = GazeApi::GetGazePointer(page);
    }
    else
    {
        // TODO: Turn off GazePointer
    }
}

static void OnIsGazeCursorVisibleChanged(DependencyObject^ ob, DependencyPropertyChangedEventArgs^ args)
{
    auto gazePointer = safe_cast<GazePointer^>(ob->GetValue(GazeApi::GazePointerProperty));
    if (gazePointer != nullptr)
    {
        gazePointer->IsCursorVisible = safe_cast<bool>(args->NewValue);
    }
}

static void OnGazeCursorRadiusChanged(DependencyObject^ ob, DependencyPropertyChangedEventArgs^ args)
{
    auto gazePointer = safe_cast<GazePointer^>(ob->GetValue(GazeApi::GazePointerProperty));
    if (gazePointer != nullptr)
    {
        gazePointer->CursorRadius = safe_cast<int>(args->NewValue);
    }
}

static DependencyProperty^ s_isGazeEnabledProperty = DependencyProperty::RegisterAttached("IsGazeEnabled", bool::typeid, GazeApi::typeid,
    ref new PropertyMetadata(false, ref new PropertyChangedCallback(&OnIsGazeEnabledChanged)));
static DependencyProperty^ s_isGazeCursorVisibleProperty = DependencyProperty::RegisterAttached("IsGazeCursorVisible", bool::typeid, GazeApi::typeid,
    ref new PropertyMetadata(true, ref new PropertyChangedCallback(&OnIsGazeCursorVisibleChanged)));
static DependencyProperty^ s_gazeCursorRadiusProperty = DependencyProperty::RegisterAttached("GazeCursorRadius", int::typeid, GazeApi::typeid,
    ref new PropertyMetadata(6, ref new PropertyChangedCallback(&OnGazeCursorRadiusChanged)));
static DependencyProperty^ s_gazeElementProperty = DependencyProperty::RegisterAttached("GazeElement", GazeElement::typeid, GazeApi::typeid, ref new PropertyMetadata(nullptr));
static DependencyProperty^ s_fixationProperty = DependencyProperty::RegisterAttached("Fixation", TimeSpan::typeid, GazeApi::typeid, ref new PropertyMetadata(GazeApi::UnsetTimeSpan));
static DependencyProperty^ s_dwellProperty = DependencyProperty::RegisterAttached("Dwell", TimeSpan::typeid, GazeApi::typeid, ref new PropertyMetadata(GazeApi::UnsetTimeSpan));
static DependencyProperty^ s_dwellRepeatProperty = DependencyProperty::RegisterAttached("DwellRepeat", TimeSpan::typeid, GazeApi::typeid, ref new PropertyMetadata(GazeApi::UnsetTimeSpan));
static DependencyProperty^ s_enterProperty = DependencyProperty::RegisterAttached("Enter", TimeSpan::typeid, GazeApi::typeid, ref new PropertyMetadata(GazeApi::UnsetTimeSpan));
static DependencyProperty^ s_exitProperty = DependencyProperty::RegisterAttached("Exit", TimeSpan::typeid, GazeApi::typeid, ref new PropertyMetadata(GazeApi::UnsetTimeSpan));
static DependencyProperty^ s_maxRepeatCountProperty = DependencyProperty::RegisterAttached("MaxRepeatCount", int::typeid, GazeApi::typeid, ref new PropertyMetadata(safe_cast<Object^>(0)));

DependencyProperty^ GazeApi::IsGazeEnabledProperty::get() { return s_isGazeEnabledProperty; }
DependencyProperty^ GazeApi::IsGazeCursorVisibleProperty::get() { return s_isGazeCursorVisibleProperty; }
DependencyProperty^ GazeApi::GazeCursorRadiusProperty::get() { return s_gazeCursorRadiusProperty; }
DependencyProperty^ GazeApi::GazeElementProperty::get() { return s_gazeElementProperty; }
DependencyProperty^ GazeApi::FixationProperty::get() { return s_fixationProperty; }
DependencyProperty^ GazeApi::DwellProperty::get() { return s_dwellProperty; }
DependencyProperty^ GazeApi::DwellRepeatProperty::get() { return s_dwellRepeatProperty; }
DependencyProperty^ GazeApi::EnterProperty::get() { return s_enterProperty; }
DependencyProperty^ GazeApi::ExitProperty::get() { return s_exitProperty; }
DependencyProperty^ GazeApi::MaxRepeatCountProperty::get() { return s_maxRepeatCountProperty; }

bool GazeApi::GetIsGazeEnabled(Page^ page) { return safe_cast<bool>(page->GetValue(s_isGazeEnabledProperty)); }
bool GazeApi::GetIsGazeCursorVisible(Page^ page) { return safe_cast<bool>(page->GetValue(s_isGazeCursorVisibleProperty)); }
int GazeApi::GetGazeCursorRadius(Page^ page) { return safe_cast<int>(page->GetValue(s_gazeCursorRadiusProperty)); }
GazeElement^ GazeApi::GetGazeElement(UIElement^ element) { return safe_cast<GazeElement^>(element->GetValue(s_gazeElementProperty)); }
TimeSpan GazeApi::GetFixation(UIElement^ element) { return safe_cast<TimeSpan>(element->GetValue(s_fixationProperty)); }
TimeSpan GazeApi::GetDwell(UIElement^ element) { return safe_cast<TimeSpan>(element->GetValue(s_dwellProperty)); }
TimeSpan GazeApi::GetDwellRepeat(UIElement^ element) { return safe_cast<TimeSpan>(element->GetValue(s_dwellRepeatProperty)); }
TimeSpan GazeApi::GetEnter(UIElement^ element) { return safe_cast<TimeSpan>(element->GetValue(s_enterProperty)); }
TimeSpan GazeApi::GetExit(UIElement^ element) { return safe_cast<TimeSpan>(element->GetValue(s_exitProperty)); }
int GazeApi::GetMaxRepeatCount(UIElement^ element) { return safe_cast<int>(element->GetValue(s_maxRepeatCountProperty)); }

void GazeApi::SetIsGazeEnabled(Page^ page, bool value) { page->SetValue(s_isGazeEnabledProperty, value); }
void GazeApi::SetIsGazeCursorVisible(Page^ page, bool value) { page->SetValue(s_isGazeCursorVisibleProperty, value); }
void GazeApi::SetGazeCursorRadius(Page^ page, int value) { page->SetValue(s_gazeCursorRadiusProperty, value); }
void GazeApi::SetGazeElement(UIElement^ element, GazeElement^ value) { element->SetValue(s_gazeElementProperty, value); }
void GazeApi::SetFixation(UIElement^ element, TimeSpan span) { element->SetValue(s_fixationProperty, span); }
void GazeApi::SetDwell(UIElement^ element, TimeSpan span) { element->SetValue(s_dwellProperty, span); }
void GazeApi::SetDwellRepeat(UIElement^ element, TimeSpan span) { element->SetValue(s_dwellRepeatProperty, span); }
void GazeApi::SetEnter(UIElement^ element, TimeSpan span) { element->SetValue(s_enterProperty, span); }
void GazeApi::SetExit(UIElement^ element, TimeSpan span) { element->SetValue(s_exitProperty, span); }
void GazeApi::SetMaxRepeatCount(UIElement^ element, int value) { element->SetValue(s_maxRepeatCountProperty, value); }


GazePointer^ GazeApi::GetGazePointer(Page^ page)
{
    auto gazePointer = safe_cast<GazePointer^>(page->GetValue(GazePointerProperty));

    if (gazePointer == nullptr)
    {
        gazePointer = ref new GazePointer(page);
        page->SetValue(GazePointerProperty, gazePointer);

        gazePointer->_unloadedToken = page->Unloaded += ref new RoutedEventHandler(gazePointer, &GazePointer::OnPageUnloaded);

        gazePointer->IsCursorVisible = safe_cast<bool>(page->GetValue(GazeApi::IsGazeCursorVisibleProperty));
        gazePointer->CursorRadius = GazeApi::GetGazeCursorRadius(page);
    }

    return gazePointer;
}

END_NAMESPACE_GAZE_INPUT