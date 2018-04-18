//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazePointer.h"
#include "GazeApi.h"
#include "GazeTargetItem.h"
#include "GazeHistoryItem.h"
#include "GazePointerEventArgs.h"
#include "GazeElement.h"
#include <xstddef>
#include <varargs.h>
#include <strsafe.h>

using namespace std;
using namespace Platform;
using namespace Windows::Foundation::Collections;
using namespace Windows::Graphics::Display;
using namespace Windows::UI;
using namespace Windows::UI::ViewManagement;
using namespace Windows::UI::Xaml::Automation::Peers;
using namespace Windows::UI::Xaml::Hosting;
using namespace Windows::UI::Xaml::Automation;
using namespace Windows::UI::Xaml::Automation::Provider;

BEGIN_NAMESPACE_GAZE_INPUT

void GazePointer::OnPageUnloaded(Object^ sender, RoutedEventArgs^ e)
{
    _isShuttingDown = true;
    IsCursorVisible = false;

    auto page = safe_cast<Page^>(_rootElement);
    page->Unloaded -= _unloadedToken;
    page->ClearValue(GazeApi::GazePointerProperty);

    //if (_gazeInputSource != nullptr)
    //{
    //    _gazeInputSource->GazeMoved -= _gazeMovedToken;
    //}
}

static DependencyProperty^ GazeTargetItemProperty = DependencyProperty::RegisterAttached("GazeTargetItem", GazeTargetItem::typeid, GazePointer::typeid, ref new PropertyMetadata(nullptr));


GazePointer::GazePointer(UIElement^ root)
{
    _rootElement = root;
    _coreDispatcher = CoreWindow::GetForCurrentThread()->Dispatcher;

    // Default to not filtering sample data
    Filter = ref new NullFilter();

    _gazeCursor = GazeCursor::Instance;

    // timer that gets called back if there gaze samples haven't been received in a while
    _eyesOffTimer = ref new DispatcherTimer();
    _eyesOffTimer->Tick += ref new EventHandler<Object^>(this, &GazePointer::OnEyesOff);

    // provide a default of GAZE_IDLE_TIME microseconds to fire eyes off 
    EyesOffDelay = GAZE_IDLE_TIME;

    InitializeHistogram();
    InitializeGazeInputSource();
}

GazePointer::~GazePointer()
{
    if (_gazeInputSource != nullptr)
    {
        _gazeInputSource->GazeMoved -= _gazeMovedToken;
    }
}

void GazePointer::LoadSettings(ValueSet^ settings)
{
    _gazeCursor->LoadSettings(settings);
    Filter->LoadSettings(settings);

    // TODO Add logic to protect against missing settings

    if (settings->HasKey("GazePointer.FixationDelay"))
    {
        _defaultFixation = (int)(settings->Lookup("GazePointer.FixationDelay"));
    }

    if (settings->HasKey("GazePointer.DwellDelay"))
    {
        _defaultDwell = (int)(settings->Lookup("GazePointer.DwellDelay"));
    }

    if (settings->HasKey("GazePointer.RepeatDelay"))
    {
        _defaultRepeat = (int)(settings->Lookup("GazePointer.RepeatDelay"));
    }

    if (settings->HasKey("GazePointer.EnterExitDelay"))
    {
        _defaultEnter = (int)(settings->Lookup("GazePointer.EnterExitDelay"));
    }

    if (settings->HasKey("GazePointer.EnterExitDelay"))
    {
        _defaultExit = (int)(settings->Lookup("GazePointer.EnterExitDelay"));
    }

    // TODO need to set fixation and dwell for all elements
    if (settings->HasKey("GazePointer.FixationDelay"))
    {
        SetElementStateDelay(_offScreenElement, GazePointerState::Fixation, (int)(settings->Lookup("GazePointer.FixationDelay")));
    }
    if (settings->HasKey("GazePointer.DwellDelay"))
    {
        SetElementStateDelay(_offScreenElement, GazePointerState::Dwell, (int)(settings->Lookup("GazePointer.DwellDelay")));
    }

    if (settings->HasKey("GazePointer.GazeIdleTime"))
    {
        EyesOffDelay = (int)(settings->Lookup("GazePointer.GazeIdleTime"));
    }
}

void GazePointer::InitializeHistogram()
{
    _activeHitTargetTimes = ref new Vector<GazeTargetItem^>();

    _offScreenElement = ref new UserControl();
    SetElementStateDelay(_offScreenElement, GazePointerState::Fixation, _defaultFixation);
    SetElementStateDelay(_offScreenElement, GazePointerState::Dwell, _defaultDwell);

    _maxHistoryTime = DEFAULT_MAX_HISTORY_DURATION;    // maintain about 3 seconds of history (in microseconds)
    _gazeHistory = ref new Vector<GazeHistoryItem^>();
}

void GazePointer::InitializeGazeInputSource()
{
    _gazeInputSource = GazeInputSourcePreview::GetForCurrentView();
    if (_gazeInputSource != nullptr)
    {
        _gazeMovedToken = _gazeInputSource->GazeMoved += ref new TypedEventHandler<
            GazeInputSourcePreview^, GazeMovedPreviewEventArgs^>(this, &GazePointer::OnGazeMoved);
    }
}

static DependencyProperty^ GetProperty(GazePointerState state)
{
    switch (state)
    {
    case GazePointerState::Fixation: return GazeApi::FixationProperty;
    case GazePointerState::Dwell: return GazeApi::DwellProperty;
    case GazePointerState::DwellRepeat: return GazeApi::DwellRepeatProperty;
    case GazePointerState::Enter: return GazeApi::EnterProperty;
    case GazePointerState::Exit: return GazeApi::ExitProperty;
    default: return nullptr;
    }
}

TimeSpan GazePointer::GetDefaultPropertyValue(GazePointerState state)
{
    switch (state)
    {
    case GazePointerState::Fixation: return *new TimeSpan{ 10 * _defaultFixation };
    case GazePointerState::Dwell: return *new TimeSpan{ 10 * _defaultDwell };
    case GazePointerState::DwellRepeat: return *new TimeSpan{ 10 * _defaultRepeat };
    case GazePointerState::Enter: return *new TimeSpan{ 10 * _defaultEnter };
    case GazePointerState::Exit: return *new TimeSpan{ 10 * _defaultExit };
    default: throw ref new NotImplementedException();
    }
}

void GazePointer::SetElementStateDelay(UIElement ^element, GazePointerState relevantState, int stateDelay)
{
    auto property = GetProperty(relevantState);
    Object^ delay = *new TimeSpan{ 10 * stateDelay };
    element->SetValue(property, delay);

    // fix up _maxHistoryTime in case the new param exceeds the history length we are currently tracking
    int dwellTime = GetElementStateDelay(element, GazePointerState::Dwell);
    int repeatTime = GetElementStateDelay(element, GazePointerState::DwellRepeat);
    _maxHistoryTime = 2 * max(dwellTime, repeatTime);
}

int GazePointer::GetElementStateDelay(UIElement ^element, GazePointerState pointerState)
{
    auto property = GetProperty(pointerState);

    DependencyObject^ walker = element;
    Object^ valueAtWalker = walker->GetValue(property);

    while (GazeApi::UnsetTimeSpan.Equals(valueAtWalker) && walker != nullptr)
    {
        walker = VisualTreeHelper::GetParent(walker);

        if (walker != nullptr)
        {
            valueAtWalker = walker->GetValue(property);
        }
    }

    auto delay = GazeApi::UnsetTimeSpan.Equals(valueAtWalker) ? GetDefaultPropertyValue(pointerState) : safe_cast<TimeSpan>(valueAtWalker);

    auto ticks = safe_cast<int>(delay.Duration / 10);
    switch (pointerState)
    {
    case GazePointerState::Dwell:
    case GazePointerState::DwellRepeat:
        _maxHistoryTime = max(_maxHistoryTime, 2 * ticks);
        break;
    }
    return ticks;
}

void GazePointer::Reset()
{
    _activeHitTargetTimes->Clear();
    _gazeHistory->Clear();

    _maxHistoryTime = DEFAULT_MAX_HISTORY_DURATION;
}

bool GazePointer::IsInvokable(UIElement^ element)
{
    bool isInvokable;

    auto peer = FrameworkElementAutomationPeer::FromElement(element);

    isInvokable = dynamic_cast<IInvokeProvider^>(peer) != nullptr ||
        dynamic_cast<IToggleProvider^>(peer) != nullptr ||
        dynamic_cast<ISelectionItemProvider^>(peer) != nullptr ||
        dynamic_cast<IExpandCollapseProvider^>(peer) != nullptr;

    return isInvokable;
}

UIElement^ GazePointer::GetHitTarget(Point gazePoint)
{
    auto targets = VisualTreeHelper::FindElementsInHostCoordinates(gazePoint, _rootElement, false);
    for each (auto target in targets)
    {
        if (IsInvokable(target))
        {
            return target;
        }
    }
    // TODO : Check if the location is offscreen
    return _rootElement;
}

GazeTargetItem^ GazePointer::GetOrCreateGazeTargetItem(UIElement^ element)
{
    auto target = safe_cast<GazeTargetItem^>(element->GetValue(GazeTargetItemProperty));
    if (target == nullptr)
    {
        target = ref new GazeTargetItem(element);
        element->SetValue(GazeTargetItemProperty, target);
    }

    unsigned int index;
    if (!_activeHitTargetTimes->IndexOf(target, &index))
    {
        _activeHitTargetTimes->Append(target);

        // calculate the time that the first DwellRepeat needs to be fired after. this will be updated every time a DwellRepeat is 
        // fired to keep track of when the next one is to be fired after that.
        int nextStateTime = GetElementStateDelay(element, GazePointerState::Enter);

        target->Reset(nextStateTime);
    }

    return target;
}

GazeTargetItem^ GazePointer::GetGazeTargetItem(UIElement^ element)
{
    auto target = safe_cast<GazeTargetItem^>(element->GetValue(GazeTargetItemProperty));
    return target;
}

UIElement^ GazePointer::ResolveHitTarget(Point gazePoint, long long timestamp)
{
    // TODO: The existance of a GazeTargetItem should be used to indicate that
    // the target item is invokable. The method of invokation should be stored
    // within the GazeTargetItem when it is created and not recalculated when
    // subsequently needed.

    // create GazeHistoryItem to deal with this sample
    auto historyItem = ref new GazeHistoryItem();
    historyItem->HitTarget = GetHitTarget(gazePoint);
    historyItem->Timestamp = timestamp;
    historyItem->Duration = 0;
    assert(historyItem->HitTarget != nullptr);

    // create new GazeTargetItem with a (default) total elapsed time of zero if one does not exist already.
    // this ensures that there will always be an entry for target elements in the code below.
    auto target = GetOrCreateGazeTargetItem(historyItem->HitTarget);
    target->LastTimestamp = timestamp;


    // just append to the list and return if the list is empty
    if (_gazeHistory->Size == 0)
    {
        _gazeHistory->Append(historyItem);
        return historyItem->HitTarget;
    }

    // find elapsed time since we got the last hit target
    historyItem->Duration = (int)(timestamp - _gazeHistory->GetAt(_gazeHistory->Size - 1)->Timestamp);
    if (historyItem->Duration > MAX_SINGLE_SAMPLE_DURATION)
    {
        historyItem->Duration = MAX_SINGLE_SAMPLE_DURATION;
    }
    _gazeHistory->Append(historyItem);

    // update the time this particular hit target has accumulated
    target->ElapsedTime += historyItem->Duration;


    // drop the oldest samples from the list until we have samples only 
    // within the window we are monitoring
    //
    // historyItem is the last item we just appended a few lines above. 
    while (historyItem->Timestamp - _gazeHistory->GetAt(0)->Timestamp > _maxHistoryTime)
    {
        auto evOldest = _gazeHistory->GetAt(0);
        _gazeHistory->RemoveAt(0);

        assert(GetGazeTargetItem(evOldest->HitTarget)->ElapsedTime - evOldest->Duration >= 0);

        // subtract the duration obtained from the oldest sample in _gazeHistory
        auto targetItem = GetGazeTargetItem(evOldest->HitTarget);
        targetItem->ElapsedTime -= evOldest->Duration;
    }

    // Return the most recent hit target 
    // Intuition would tell us that we should return NOT the most recent
    // hitTarget, but the one with the most accumulated time in 
    // in the maintained history. But the effect of that is that
    // the user will feel that they have clicked on the wrong thing
    // when they are looking at something else.
    // That is why we return the most recent hitTarget so that 
    // when its dwell time has elapsed, it will be invoked
    return historyItem->HitTarget;
}

void GazePointer::GotoState(UIElement^ control, GazePointerState state)
{
    Platform::String^ stateName;

    switch (state)
    {
    case GazePointerState::Enter:
        return;
    case GazePointerState::Exit:
        stateName = "Normal";
        break;
    case GazePointerState::Fixation:
        stateName = "Fixation";
        break;
    case GazePointerState::DwellRepeat:
    case GazePointerState::Dwell:
        stateName = "Dwell";
        break;
    default:
        assert(0);
        return;
    }

    // TODO: Implement proper support for visual states
    // VisualStateManager::GoToState(dynamic_cast<Control^>(control), stateName, true);
}

void GazePointer::InvokeTarget(UIElement ^target)
{
    auto control = dynamic_cast<Control^>(target);
    if (control != nullptr && control->IsEnabled)
    {
        auto peer = FrameworkElementAutomationPeer::FromElement(control);

        auto invokeProvider = dynamic_cast<IInvokeProvider^>(peer);
        if (invokeProvider != nullptr)
        {
            invokeProvider->Invoke();
        }
        else
        {
            auto toggleProvider = dynamic_cast<IToggleProvider^>(peer);
            if (toggleProvider != nullptr)
            {
                toggleProvider->Toggle();
            }
            else
            {
                auto selectionItemProvider = dynamic_cast<ISelectionItemProvider^>(peer);
                if (selectionItemProvider != nullptr)
                {
                    selectionItemProvider->Select();
                }
                else
                {
                    auto expandCollapseProvider = dynamic_cast<IExpandCollapseProvider^>(peer);
                    if (expandCollapseProvider != nullptr)
                    {
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
                }
            }
        }
    }
}

void GazePointer::OnEyesOff(Object ^sender, Object ^ea)
{
    _eyesOffTimer->Stop();

    CheckIfExiting(_lastTimestamp + EyesOffDelay);
    RaiseGazePointerEvent(nullptr, GazePointerState::Enter, (int)EyesOffDelay);
}

void GazePointer::CheckIfExiting(long long curTimestamp)
{
    for (unsigned int index = 0; index < _activeHitTargetTimes->Size; index++)
    {
        auto targetItem = _activeHitTargetTimes->GetAt(index);
        auto targetElement = targetItem->TargetElement;
        auto exitDelay = GetElementStateDelay(targetElement, GazePointerState::Exit);

        long long idleDuration = curTimestamp - targetItem->LastTimestamp;
        if (targetItem->ElementState != GazePointerState::PreEnter && idleDuration > exitDelay)
        {
            targetItem->ElementState = GazePointerState::PreEnter;
            GotoState(targetElement, GazePointerState::Exit);
            RaiseGazePointerEvent(targetElement, GazePointerState::Exit, targetItem->ElapsedTime);
            targetItem->GiveFeedback();

            _activeHitTargetTimes->RemoveAt(index);

            // remove all history samples referring to deleted hit target
            for (unsigned i = 0; i < _gazeHistory->Size; )
            {
                auto hitTarget = _gazeHistory->GetAt(i)->HitTarget;
                if (hitTarget == targetElement)
                {
                    _gazeHistory->RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            // return because only one element can be exited at a time and at this point
            // we have done everything that we can do
            return;
        }
    }
}

wchar_t *PointerStates[] = {
    L"Exit",
    L"PreEnter",
    L"Enter",
    L"Fixation",
    L"Dwell",
    L"DwellRepeat"
};

void GazePointer::RaiseGazePointerEvent(UIElement^ target, GazePointerState state, int elapsedTime)
{
    //assert(target != _rootElement);
    auto gpea = ref new GazePointerEventArgs(target, state, elapsedTime);
    //auto buttonObj = dynamic_cast<Button ^>(target);
    //if (buttonObj && buttonObj->Content)
    //{
    //    String^ buttonText = dynamic_cast<String^>(buttonObj->Content);
    //    Debug::WriteLine(L"GPE: %s -> %s, %d", buttonText, PointerStates[(int)state], elapsedTime);
    //}
    //else
    //{
    //    Debug::WriteLine(L"GPE: 0x%08x -> %s, %d", target != nullptr ? target->GetHashCode() : 0, PointerStates[(int)state], elapsedTime);
    //}

    auto control = safe_cast<Control^>(target);
    auto gazeElement = target != nullptr ? GazeApi::GetGazeElement(target) : nullptr;

    if (gazeElement != nullptr)
    {
        gazeElement->RaiseStateChanged(control, gpea);
    }

    if (state == GazePointerState::Dwell)
    {
        auto handled = false;

        if (gazeElement != nullptr)
        {
            auto args = ref new GazeInvokedRoutedEventArgs();
            gazeElement->RaiseInvoked(control, args);
            handled = args->Handled;
        }

        if (!handled)
        {
            InvokeTarget(target);
        }
    }
}

void GazePointer::OnGazeMoved(GazeInputSourcePreview^ provider, GazeMovedPreviewEventArgs^ args)
{
    if (!_isShuttingDown)
    {
        auto intermediatePoints = args->GetIntermediatePoints();
        for each(auto point in intermediatePoints)
        {
            auto position = point->EyeGazePosition;
            if (position != nullptr)
            {
                ProcessGazePoint(point->Timestamp, position->Value);
            }
        }
    }
}

void GazePointer::ProcessGazePoint(long long timestamp, Point position)
{
    auto ea = ref new GazeEventArgs(position, timestamp);

    auto fa = Filter->Update(ea);
    _gazeCursor->Position = fa->Location;

    auto hitTarget = ResolveHitTarget(fa->Location, fa->Timestamp);
    assert(hitTarget != nullptr);

    //Debug::WriteLine(L"ProcessGazePoint: %llu -> [%d, %d], %llu", hitTarget->GetHashCode(), (int)fa->Location.X, (int)fa->Location.Y, fa->Timestamp);

    // check to see if any element in _hitTargetTimes needs an exit event fired.
    // this ensures that all exit events are fired before enter event
    CheckIfExiting(fa->Timestamp);

    auto targetItem = GetGazeTargetItem(hitTarget);
    GazePointerState nextState = static_cast<GazePointerState>(static_cast<int>(targetItem->ElementState) + 1);

    Debug::WriteLine(L"%llu -> State=%d, Elapsed=%d, NextStateTime=%d", targetItem->TargetElement, targetItem->ElementState, targetItem->ElapsedTime, targetItem->NextStateTime);

    if (targetItem->ElapsedTime > targetItem->NextStateTime)
    {
        auto prevStateTime = targetItem->NextStateTime;

        // prevent targetItem from ever actually transitioning into the DwellRepeat state so as
        // to continuously emit the DwellRepeat event
        if (nextState != GazePointerState::DwellRepeat)
        {
            targetItem->ElementState = nextState;
            nextState = static_cast<GazePointerState>(static_cast<int>(nextState) + 1);     // nextState++
            targetItem->NextStateTime = GetElementStateDelay(targetItem->TargetElement, nextState);
        }
        else
        {
            // move the NextStateTime by one dwell period, while continuing to stay in Dwell state
            targetItem->NextStateTime += GetElementStateDelay(targetItem->TargetElement, GazePointerState::Dwell) -
                GetElementStateDelay(targetItem->TargetElement, GazePointerState::Fixation);
        }

        if (targetItem->ElementState == GazePointerState::Dwell)
        {
            targetItem->RepeatCount++;
            if (targetItem->MaxRepeatCount < targetItem->RepeatCount)
            {
                targetItem->NextStateTime = MAXINT;
            }
        }

        GotoState(targetItem->TargetElement, targetItem->ElementState);

        RaiseGazePointerEvent(targetItem->TargetElement, targetItem->ElementState, targetItem->ElapsedTime);
    }

    targetItem->GiveFeedback();

    _eyesOffTimer->Start();
    _lastTimestamp = fa->Timestamp;
}

END_NAMESPACE_GAZE_INPUT