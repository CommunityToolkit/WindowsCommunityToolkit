//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "OneEuroFilter.h"
#include "GazePointer.h"
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

BEGIN_NAMESPACE_GAZE_INPUT

GazePointer::GazePointer(UIElement^ root)
{
    _gazeSettings = GazeSettings::Instance;

    _rootElement = root;
    _coreDispatcher = CoreWindow::GetForCurrentThread()->Dispatcher;

    InputEventForwardingEnabled = true;
    // Default to not filtering sample data
    Filter = ref new NullFilter();

    _gazeCursor = GazeCursor::Instance;

    // timer that gets called back if there gaze samples haven't been received in a while
    _eyesOffTimer = ref new DispatcherTimer();
    _eyesOffTimer->Tick += ref new EventHandler<Object^>(this, &GazePointer::OnEyesOff);

    // provide a default of GAZE_IDLE_TIME microseconds to fire eyes off 
    EyesOffDelay = _gazeSettings->GazePointer_Gaze_Idle_Time;

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

void GazePointer::InitializeHistogram()
{
    _defaultInvokeParams = ref new GazeInvokeParams();
    _defaultInvokeParams->Insert(GazePointerState::Fixation, _gazeSettings->GazePointer_Fixation_Delay);
    _defaultInvokeParams->Insert(GazePointerState::Dwell, _gazeSettings->GazePointer_Dwell_Delay);
    _defaultInvokeParams->Insert(GazePointerState::DwellRepeat, _gazeSettings->GazePointer_Repeat_Delay);
    _defaultInvokeParams->Insert(GazePointerState::Enter, _gazeSettings->GazePointer_Enter_Exit_Delay);
    _defaultInvokeParams->Insert(GazePointerState::Exit, _gazeSettings->GazePointer_Enter_Exit_Delay);

    _hitTargetTimes = ref new Map<int, GazeTargetItem^>();

    _offScreenElement = ref new UserControl();
    SetElementStateDelay(_offScreenElement, GazePointerState::Fixation, _gazeSettings->GazePointer_Fixation_Delay);
    SetElementStateDelay(_offScreenElement, GazePointerState::Dwell, _gazeSettings->GazePointer_Dwell_Delay);

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

void GazePointer::SetElementStateDelay(UIElement ^element, GazePointerState relevantState, int stateDelay)
{
    int hashCode = element->GetHashCode();

    // do we need to create a new invoke params map for the user-specified element?
    if (_elementInvokeParams.find(hashCode) == _elementInvokeParams.end())
    {
        // copy all existing default delay params from existe _offscreenElementInvokeParams.
        // this copy is done because it makes it easier for the rest of the code to use. element invoke
        // params could be refactored to avoid this copying of default values but it doesn't seem worth 
        // it considering all the places and situations that invoke params are used
        auto newInvokeParams = ref new GazeInvokeParams();
        for each (auto params in _defaultInvokeParams)
        {
            newInvokeParams->Insert(params->Key, params->Value);
        }

        _elementInvokeParams[hashCode] = newInvokeParams;
    }

    auto invokeParams = _elementInvokeParams[hashCode];
    invokeParams->Insert(relevantState, stateDelay);

    // fix up _maxHistoryTime in case the new param exceeds the history length we are currently tracking
    int dwellTime = invokeParams->Lookup(GazePointerState::Dwell);
    int repeatTime = invokeParams->Lookup(GazePointerState::DwellRepeat);
    if (repeatTime != INT_MAX)
    {
        _maxHistoryTime = max(2 * repeatTime, _maxHistoryTime);
    }
    else
    {
        _maxHistoryTime = max(2 * dwellTime, _maxHistoryTime);
    }
}

int GazePointer::GetElementStateDelay(UIElement ^element, GazePointerState pointerState)
{
    int hashCode = element->GetHashCode();

    // do we need to create a new invoke params map for the user-specified element?
    auto iterator = _elementInvokeParams.find(hashCode);
    if (iterator == _elementInvokeParams.end())
    {
        return _defaultInvokeParams->Lookup(pointerState);
    }

    return iterator->second->Lookup(pointerState);;
}

void GazePointer::Reset()
{
    _hitTargetTimes->Clear();
    _gazeHistory->Clear();
}

GazeInvokeParams^ GazePointer::GetGazeInvokeParams(UIElement^ target)
{
    auto hashCode = target->GetHashCode();

    // return invoke params for _offscreenElement if target IS _offscreenElement or if no invoke params
    // exist for the target requested
    if (target == _rootElement || _elementInvokeParams.find(hashCode) == _elementInvokeParams.end())
    {
        return _defaultInvokeParams;
    }

    // TODO: adjust history length if the click params indicate something else
    return _elementInvokeParams[hashCode];
}

bool GazePointer::IsInvokable(UIElement^ element)
{
    if (IsInvokableImpl != nullptr)
    {
        return IsInvokableImpl(element);
    }
    else
    {
        auto button = dynamic_cast<Button ^>(element);
        if (button != nullptr)
        {
            return true;
        }

        auto toggleButton = dynamic_cast<ToggleButton^>(element);
        if (toggleButton != nullptr)
        {
            return true;
        }

        auto toggleSwitch = dynamic_cast<ToggleSwitch^>(element);
        if (toggleSwitch != nullptr)
        {
            return true;
        }

        auto textbox = dynamic_cast<TextBox^>(element);
        if (textbox != nullptr)
        {
            return true;
        }

        auto pivot = dynamic_cast<Pivot^>(element);
        if (pivot != nullptr)
        {
            return true;
        }
    }

    return false;
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

UIElement^ GazePointer::ResolveHitTarget(Point gazePoint, long long timestamp)
{
    // create GazeHistoryItem to deal with this sample
    auto historyItem = ref new GazeHistoryItem();
    historyItem->HitTarget = GetHitTarget(gazePoint);
    historyItem->Timestamp = timestamp;
    historyItem->Duration = 0;
    int hashCode = historyItem->HitTarget->GetHashCode();
    assert(historyItem->HitTarget != nullptr);

    // create new GazeTargetItem with a (default) total elapsed time of zero if one does not exist already.
    // this ensures that there will always be an entry for target elements in the code below.
    if (!_hitTargetTimes->HasKey(hashCode))
    {
        auto invokeParams = GetGazeInvokeParams(historyItem->HitTarget);
        // calculate the time that the first DwellRepeat needs to be fired after. this will be updated every time a DwellRepeat is 
        // fired to keep track of when the next one is to be fired after that.
        int nextStateTime = invokeParams->Lookup(GazePointerState::Enter);
        int nextRepeatTime = invokeParams->Lookup(GazePointerState::DwellRepeat);

        _hitTargetTimes->Insert(hashCode, ref new GazeTargetItem(historyItem->HitTarget, timestamp, nextStateTime, nextRepeatTime));
    }


    // just append to the list and return if the list is empty
    if (_gazeHistory->Size == 0)
    {
        _gazeHistory->Append(historyItem);
        return historyItem->HitTarget;
    }

    // find elapsed time since we got the last hit target
    historyItem->Duration = (int)(timestamp - _gazeHistory->GetAt(_gazeHistory->Size - 1)->Timestamp);
    if (historyItem->Duration > _gazeSettings->GazePointer_Max_Single_Sample_Duration)
    {
        historyItem->Duration = _gazeSettings->GazePointer_Max_Single_Sample_Duration;
    }
    _gazeHistory->Append(historyItem);

    // update the time this particular hit target has accumulated
    auto target = _hitTargetTimes->Lookup(hashCode);
    target->ElapsedTime += historyItem->Duration;
    target->LastTimestamp = timestamp;


    // drop the oldest samples from the list until we have samples only 
    // within the window we are monitoring
    //
    // historyItem is the last item we just appended a few lines above. 
    while (historyItem->Timestamp - _gazeHistory->GetAt(0)->Timestamp > _maxHistoryTime)
    {
        auto evOldest = _gazeHistory->GetAt(0);
        _gazeHistory->RemoveAt(0);

        int hashOldest = evOldest->HitTarget->GetHashCode();
        assert(_hitTargetTimes->Lookup(hashOldest)->ElapsedTime - evOldest->Duration >= 0);

        // subtract the duration obtained from the oldest sample in _gazeHistory
        auto targetItem = _hitTargetTimes->Lookup(hashOldest);
        targetItem->ElapsedTime -= evOldest->Duration;

        auto invokeParams = GetGazeInvokeParams(targetItem->TargetElement);

        if ((targetItem->ElementState == GazePointerState::Dwell) &&
            (invokeParams->Lookup(GazePointerState::DwellRepeat) != MAXINT))
        {
            targetItem->NextStateTime -= evOldest->Duration;
        }
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
    if (target == _rootElement)
    {
        return;
    }

    auto control = dynamic_cast<Control^>(target);
    if ((control == nullptr) || (!control->IsEnabled))
    {
        return;
    }

    if (InvokeTargetImpl != nullptr)
    {
        InvokeTargetImpl(target);
    }
    else
    {
        auto button = dynamic_cast<Button^>(control);
        if (button != nullptr)
        {
            auto peer = ref new ButtonAutomationPeer(button);
            peer->Invoke();
            return;
        }

        auto toggleButton = dynamic_cast<ToggleButton^>(control);
        if (toggleButton != nullptr)
        {
            auto peer = ref new ToggleButtonAutomationPeer(toggleButton);
            peer->Toggle();
            return;
        }

        auto toggleSwitch = dynamic_cast<ToggleSwitch^>(control);
        if (toggleSwitch)
        {
            auto peer = ref new ToggleSwitchAutomationPeer(toggleSwitch);
            peer->Toggle();
            return;
        }

        auto textBox = dynamic_cast<TextBox^>(control);
        if (textBox != nullptr)
        {
            auto peer = ref new TextBoxAutomationPeer(textBox);
            peer->SetFocus();
            return;
        }

        auto pivot = dynamic_cast<Pivot^>(control);
        if (pivot != nullptr)
        {
            auto peer = ref new PivotAutomationPeer(pivot);
            peer->SetFocus();
            return;
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
    for each (auto target in _hitTargetTimes)
    {
        auto targetItem = target->Value;
        auto invokeParams = GetGazeInvokeParams(targetItem->TargetElement);

        long long idleDuration = curTimestamp - targetItem->LastTimestamp;
        if (targetItem->ElementState != GazePointerState::PreEnter && idleDuration > invokeParams->Lookup(GazePointerState::Exit))
        {
            GotoState(targetItem->TargetElement, GazePointerState::Exit);
            RaiseGazePointerEvent(targetItem->TargetElement, GazePointerState::Exit, targetItem->ElapsedTime);

            int targetHash = target->Key;
            _hitTargetTimes->Remove(targetHash);

            // remove all history samples referring to deleted hit target
            for (unsigned i = 0; i < _gazeHistory->Size; )
            {
                auto hitTarget = _gazeHistory->GetAt(i)->HitTarget;
                if (hitTarget->GetHashCode() == targetHash)
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
    OnGazePointerEvent(this, gpea);
}

void GazePointer::OnGazeMoved(GazeInputSourcePreview^ provider, GazeMovedPreviewEventArgs^ args)
{
    auto intermediatePoints = args->GetIntermediatePoints();
    for each(auto point in intermediatePoints)
    {
        ProcessGazePoint(point);
    }
}

void GazePointer::ProcessGazePoint(GazePointPreview^ gazePoint)
{
    if (gazePoint->EyeGazePosition == nullptr)
        return;

    auto ea = ref new GazeEventArgs(gazePoint->EyeGazePosition->Value, gazePoint->Timestamp);

    if (InputEventForwardingEnabled)
    {
        OnGazeInputEvent(this, ea);
    }

    auto fa = Filter->Update(ea);
    _gazeCursor->Position = fa->Location;

    auto hitTarget = ResolveHitTarget(fa->Location, fa->Timestamp);
    assert(hitTarget != nullptr);

    //Debug::WriteLine(L"ProcessGazePoint: %llu -> [%d, %d], %llu", hitTarget->GetHashCode(), (int)fa->Location.X, (int)fa->Location.Y, fa->Timestamp);

    // check to see if any element in _hitTargetTimes needs an exit event fired.
    // this ensures that all exit events are fired before enter event
    CheckIfExiting(fa->Timestamp);

    auto targetItem = _hitTargetTimes->Lookup(hitTarget->GetHashCode());
    auto invokeParams = GetGazeInvokeParams(targetItem->TargetElement);

    GazePointerState nextState = static_cast<GazePointerState>(static_cast<int>(targetItem->ElementState) + 1);

    //Debug::WriteLine(L"%llu -> State=%d, Elapsed=%d, NextStateTime=%d", targetItem->TargetElement, targetItem->ElementState, targetItem->ElapsedTime, targetItem->NextStateTime);

    if (targetItem->ElapsedTime > targetItem->NextStateTime)
    {
        // prevent targetItem from ever actually transitioning into the DwellRepeat state so as
        // to continuously emit the DwellRepeat event
        if (nextState != GazePointerState::DwellRepeat)
        {
            targetItem->ElementState = nextState;
            nextState = static_cast<GazePointerState>(static_cast<int>(nextState) + 1);     // nextState++
            targetItem->NextStateTime = invokeParams->Lookup(nextState);
        }
        else
        {
            // move the NextStateTime by one dwell period, while continuing to stay in Dwell state
            targetItem->NextStateTime += invokeParams->Lookup(GazePointerState::Dwell) - invokeParams->Lookup(GazePointerState::Fixation);
        }

        GotoState(targetItem->TargetElement, targetItem->ElementState);
        RaiseGazePointerEvent(targetItem->TargetElement, targetItem->ElementState, targetItem->ElapsedTime);
    }

    _eyesOffTimer->Start();
    _lastTimestamp = fa->Timestamp;
}

END_NAMESPACE_GAZE_INPUT