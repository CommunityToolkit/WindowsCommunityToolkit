//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"

#include "GazePointer.h"

#include "GazeElement.h"
#include "GazeHistoryItem.h"
#include "GazeTargetItem.h"
#include "StateChangedEventArgs.h"

using namespace Platform;

BEGIN_NAMESPACE_GAZE_INPUT

ref class NonInvokeGazeTargetItem sealed : GazeTargetItem
{
internal:

    NonInvokeGazeTargetItem()
        : GazeTargetItem(ref new Page())
    {
    }

internal:

    virtual property bool IsInvokable { bool get() override { return false; } }

    void Invoke() override
    {
    }
};

GazePointer^ GazePointer::Instance::get()
{
    thread_local static GazePointer^ value;
    if (value == nullptr)
    {
        value = ref new GazePointer();
    }
    return value;
}

void GazePointer::AddRoot(FrameworkElement^ element)
{
    _roots->InsertAt(0, element);

    if (_roots->Size == 1)
    {
        _isShuttingDown = false;
        InitializeGazeInputSource();
    }
}

void GazePointer::RemoveRoot(FrameworkElement^ element)
{
    unsigned int index = 0;
    if (_roots->IndexOf(element, &index))
    {
        _roots->RemoveAt(index);
    }
    else
    {
        assert(false);
    }

    if (_roots->Size == 0)
    {
        _isShuttingDown = true;
        _gazeCursor->IsGazeEntered = false;
        DeinitializeGazeInputSource();
    }
}

GazePointer::GazePointer()
{
    _nonInvokeGazeTargetItem = ref new NonInvokeGazeTargetItem();

    // Default to not filtering sample data
    Filter = ref new NullFilter();

    _gazeCursor = ref new GazeCursor();

    // timer that gets called back if there gaze samples haven't been received in a while
    _eyesOffTimer = ref new DispatcherTimer();
    _eyesOffTimer->Tick += ref new EventHandler<Object^>(this, &GazePointer::OnEyesOff);

    // provide a default of GAZE_IDLE_TIME microseconds to fire eyes off 
    EyesOffDelay = GAZE_IDLE_TIME;

    InitializeHistogram();

    _watcher = GazeInputSourcePreview::CreateWatcher();
    _watcher->Added += ref new TypedEventHandler<GazeDeviceWatcherPreview^, GazeDeviceWatcherAddedPreviewEventArgs^>(this, &GazePointer::OnDeviceAdded);
    _watcher->Removed += ref new TypedEventHandler<GazeDeviceWatcherPreview^, GazeDeviceWatcherRemovedPreviewEventArgs^>(this, &GazePointer::OnDeviceRemoved);
    _watcher->Start();
}

void GazePointer::OnDeviceAdded(GazeDeviceWatcherPreview^ sender, GazeDeviceWatcherAddedPreviewEventArgs^ args)
{
    _deviceCount++;

    if (_deviceCount == 1)
    {
        IsDeviceAvailableChanged(nullptr, nullptr);

        InitializeGazeInputSource();
    }
}

void GazePointer::OnDeviceRemoved(GazeDeviceWatcherPreview^ sender, GazeDeviceWatcherRemovedPreviewEventArgs^ args)
{
    _deviceCount--;

    if (_deviceCount == 0)
    {
        IsDeviceAvailableChanged(nullptr, nullptr);
    }
}

GazePointer::~GazePointer()
{
    _watcher->Added -= _deviceAddedToken;
    _watcher->Removed -= _deviceRemovedToken;

    if (_gazeInputSource != nullptr)
    {
        _gazeInputSource->GazeEntered -= _gazeEnteredToken;
        _gazeInputSource->GazeMoved -= _gazeMovedToken;
        _gazeInputSource->GazeExited -= _gazeExitedToken;
    }
}

void GazePointer::LoadSettings(ValueSet^ settings)
{
    _gazeCursor->LoadSettings(settings);
    Filter->LoadSettings(settings);

    // TODO Add logic to protect against missing settings

    if (settings->HasKey("GazePointer.FixationDelay"))
    {
        _defaultFixation = TimeSpanFromMicroseconds((int)(settings->Lookup("GazePointer.FixationDelay")));
    }

    if (settings->HasKey("GazePointer.DwellDelay"))
    {
        _defaultDwell = TimeSpanFromMicroseconds((int)(settings->Lookup("GazePointer.DwellDelay")));
    }

    if (settings->HasKey("GazePointer.DwellDelay"))
    {
        _defaultDwellRepeatDelay = TimeSpanFromMicroseconds((int)(settings->Lookup("GazePointer.DwellRepeatDelay")));
    }

    if (settings->HasKey("GazePointer.RepeatDelay"))
    {
        _defaultRepeat = TimeSpanFromMicroseconds((int)(settings->Lookup("GazePointer.RepeatDelay")));
    }

    if (settings->HasKey("GazePointer.ThresholdDelay"))
    {
        _defaultThreshold = TimeSpanFromMicroseconds((int)(settings->Lookup("GazePointer.ThresholdDelay")));
    }

    // TODO need to set fixation and dwell for all elements
    if (settings->HasKey("GazePointer.FixationDelay"))
    {
        SetElementStateDelay(_offScreenElement, PointerState::Fixation, TimeSpanFromMicroseconds((int)(settings->Lookup("GazePointer.FixationDelay"))));
    }
    if (settings->HasKey("GazePointer.DwellDelay"))
    {
        SetElementStateDelay(_offScreenElement, PointerState::Dwell, TimeSpanFromMicroseconds((int)(settings->Lookup("GazePointer.DwellDelay"))));
    }

    if (settings->HasKey("GazePointer.GazeIdleTime"))
    {
        EyesOffDelay = TimeSpanFromMicroseconds((int)(settings->Lookup("GazePointer.GazeIdleTime")));
    }
}

void GazePointer::InitializeHistogram()
{
    _activeHitTargetTimes = ref new Vector<GazeTargetItem^>();

    _offScreenElement = ref new UserControl();
    SetElementStateDelay(_offScreenElement, PointerState::Fixation, _defaultFixation);
    SetElementStateDelay(_offScreenElement, PointerState::Dwell, _defaultDwell);

    _maxHistoryTime = DEFAULT_MAX_HISTORY_DURATION;    // maintain about 3 seconds of history (in microseconds)
    _gazeHistory = ref new Vector<GazeHistoryItem^>();
}

void GazePointer::InitializeGazeInputSource()
{
    if (_gazeInputSource == nullptr && _roots->Size != 0 && _deviceCount != 0)
    {
        _gazeInputSource = GazeInputSourcePreview::GetForCurrentView();
        if (_gazeInputSource != nullptr)
        {
            _gazeEnteredToken = _gazeInputSource->GazeEntered += ref new TypedEventHandler<
                GazeInputSourcePreview^, GazeEnteredPreviewEventArgs^>(this, &GazePointer::OnGazeEntered);
            _gazeMovedToken = _gazeInputSource->GazeMoved += ref new TypedEventHandler<
                GazeInputSourcePreview^, GazeMovedPreviewEventArgs^>(this, &GazePointer::OnGazeMoved);
            _gazeExitedToken = _gazeInputSource->GazeExited += ref new TypedEventHandler<
                GazeInputSourcePreview^, GazeExitedPreviewEventArgs^>(this, &GazePointer::OnGazeExited);
        }
    }
}

void GazePointer::DeinitializeGazeInputSource()
{
    if (_gazeInputSource != nullptr)
    {
        _gazeInputSource->GazeEntered -= _gazeEnteredToken;
        _gazeInputSource->GazeMoved -= _gazeMovedToken;
        _gazeInputSource->GazeExited -= _gazeExitedToken;

        _gazeInputSource = nullptr;
    }
}

static DependencyProperty^ GetProperty(PointerState state)
{
    switch (state)
    {
    case PointerState::Fixation: return GazeInput::FixationDurationProperty;
    case PointerState::Dwell: return GazeInput::DwellDurationProperty;
    case PointerState::DwellRepeat: return GazeInput::DwellRepeatDurationProperty;
    case PointerState::Enter: return GazeInput::ThresholdDurationProperty;
    case PointerState::Exit: return GazeInput::ThresholdDurationProperty;
    default: return nullptr;
    }
}

TimeSpan GazePointer::GetDefaultPropertyValue(PointerState state)
{
    switch (state)
    {
    case PointerState::Fixation: return _defaultFixation;
    case PointerState::Dwell: return _defaultDwell;
    case PointerState::DwellRepeat: return _defaultRepeat;
    case PointerState::Enter: return _defaultThreshold;
    case PointerState::Exit: return _defaultThreshold;
    default: throw ref new NotImplementedException();
    }
}

void GazePointer::SetElementStateDelay(UIElement ^element, PointerState relevantState, TimeSpan stateDelay)
{
    auto property = GetProperty(relevantState);
    element->SetValue(property, stateDelay);

    // fix up _maxHistoryTime in case the new param exceeds the history length we are currently tracking
    auto dwellTime = GetElementStateDelay(element, PointerState::Dwell);
    auto repeatTime = GetElementStateDelay(element, PointerState::DwellRepeat);
    _maxHistoryTime = 2 * max(dwellTime, repeatTime);
}

TimeSpan GazePointer::GetElementStateDelay(UIElement ^element, DependencyProperty^ property, TimeSpan defaultValue)
{
    DependencyObject^ walker = element;
    Object^ valueAtWalker = walker->GetValue(property);

    while (GazeInput::UnsetTimeSpan.Equals(valueAtWalker) && walker != nullptr)
    {
        walker = VisualTreeHelper::GetParent(walker);

        if (walker != nullptr)
        {
            valueAtWalker = walker->GetValue(property);
        }
    }

    auto ticks = GazeInput::UnsetTimeSpan.Equals(valueAtWalker) ? defaultValue : safe_cast<TimeSpan>(valueAtWalker);

    return ticks;
}

TimeSpan GazePointer::GetElementStateDelay(UIElement ^element, PointerState pointerState)
{
    auto property = GetProperty(pointerState);
    auto defaultValue = GetDefaultPropertyValue(pointerState);
    auto ticks = GetElementStateDelay(element, property, defaultValue);

    switch (pointerState)
    {
    case PointerState::Dwell:
    case PointerState::DwellRepeat:
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

GazeTargetItem^ GazePointer::GetHitTarget(Point gazePoint)
{
    GazeTargetItem^ invokable;

    switch (Window::Current->CoreWindow->ActivationMode)
    {
    default:
        invokable = _nonInvokeGazeTargetItem;
        break;

    case CoreWindowActivationMode::ActivatedInForeground:
    case CoreWindowActivationMode::ActivatedNotForeground:
        auto elements = VisualTreeHelper::FindElementsInHostCoordinates(gazePoint, nullptr, false);
        auto first = elements->First();
        auto element = first->HasCurrent ? first->Current : nullptr;

        invokable = nullptr;

        if (element != nullptr)
        {
            invokable = GazeTargetItem::GetOrCreate(element);

            while (element != nullptr && !invokable->IsInvokable)
            {
                element = dynamic_cast<UIElement^>(VisualTreeHelper::GetParent(element));

                if (element != nullptr)
                {
                    invokable = GazeTargetItem::GetOrCreate(element);
                }
            }
        }

        if (element == nullptr || !invokable->IsInvokable)
        {
            invokable = _nonInvokeGazeTargetItem;
        }
        else
        {
            Interaction interaction;
            do
            {
                interaction = GazeInput::GetInteraction(element);
                element = dynamic_cast<UIElement^>(VisualTreeHelper::GetParent(element));
            } while (interaction == Interaction::Inherited && element != nullptr);

            if (interaction == Interaction::Inherited)
            {
                interaction = GazeInput::Interaction;
            }

            if (interaction != Interaction::Enabled)
            {
                invokable = _nonInvokeGazeTargetItem;
            }
        }
        break;
    }

    return invokable;
}

void GazePointer::ActivateGazeTargetItem(GazeTargetItem^ target)
{
    unsigned int index;
    if (!_activeHitTargetTimes->IndexOf(target, &index))
    {
        _activeHitTargetTimes->Append(target);

        // calculate the time that the first DwellRepeat needs to be fired after. this will be updated every time a DwellRepeat is 
        // fired to keep track of when the next one is to be fired after that.
        auto nextStateTime = GetElementStateDelay(target->TargetElement, PointerState::Enter);

        target->Reset(nextStateTime);
    }
}

GazeTargetItem^ GazePointer::ResolveHitTarget(Point gazePoint, TimeSpan timestamp)
{
    // TODO: The existance of a GazeTargetItem should be used to indicate that
    // the target item is invokable. The method of invokation should be stored
    // within the GazeTargetItem when it is created and not recalculated when
    // subsequently needed.

    // create GazeHistoryItem to deal with this sample
    auto target = GetHitTarget(gazePoint);
    auto historyItem = ref new GazeHistoryItem();
    historyItem->HitTarget = target;
    historyItem->Timestamp = timestamp;
    historyItem->Duration = TimeSpanZero;
    assert(historyItem->HitTarget != nullptr);

    // create new GazeTargetItem with a (default) total elapsed time of zero if one does not exist already.
    // this ensures that there will always be an entry for target elements in the code below.
    ActivateGazeTargetItem(target);
    target->LastTimestamp = timestamp;

    // find elapsed time since we got the last hit target
    historyItem->Duration = timestamp - _lastTimestamp;
    if (historyItem->Duration > MAX_SINGLE_SAMPLE_DURATION)
    {
        historyItem->Duration = MAX_SINGLE_SAMPLE_DURATION;
    }
    _gazeHistory->Append(historyItem);

    // update the time this particular hit target has accumulated
    target->DetailedTime += historyItem->Duration;

    // drop the oldest samples from the list until we have samples only 
    // within the window we are monitoring
    //
    // historyItem is the last item we just appended a few lines above. 
    for (auto evOldest = _gazeHistory->GetAt(0);
        historyItem->Timestamp - evOldest->Timestamp > _maxHistoryTime;
        evOldest = _gazeHistory->GetAt(0))
    {
        _gazeHistory->RemoveAt(0);

        // subtract the duration obtained from the oldest sample in _gazeHistory
        auto targetItem = evOldest->HitTarget;
        assert(targetItem->DetailedTime - evOldest->Duration >= TimeSpanZero);
        targetItem->DetailedTime -= evOldest->Duration;
        if (targetItem->ElementState != PointerState::PreEnter)
        {
            targetItem->OverflowTime += evOldest->Duration;
        }
    }

    _lastTimestamp = timestamp;

    // Return the most recent hit target 
    // Intuition would tell us that we should return NOT the most recent
    // hitTarget, but the one with the most accumulated time in 
    // in the maintained history. But the effect of that is that
    // the user will feel that they have clicked on the wrong thing
    // when they are looking at something else.
    // That is why we return the most recent hitTarget so that 
    // when its dwell time has elapsed, it will be invoked
    return target;
}

void GazePointer::GotoState(UIElement^ control, PointerState state)
{
    Platform::String^ stateName;

    switch (state)
    {
    case PointerState::Enter:
        return;
    case PointerState::Exit:
        stateName = "Normal";
        break;
    case PointerState::Fixation:
        stateName = "Fixation";
        break;
    case PointerState::DwellRepeat:
    case PointerState::Dwell:
        stateName = "Dwell";
        break;
    default:
        //assert(0);
        return;
    }

    // TODO: Implement proper support for visual states
    // VisualStateManager::GoToState(dynamic_cast<Control^>(control), stateName, true);
}

void GazePointer::OnEyesOff(Object ^sender, Object ^ea)
{
    _eyesOffTimer->Stop();

    CheckIfExiting(_lastTimestamp + EyesOffDelay);
    RaiseGazePointerEvent(nullptr, PointerState::Enter, EyesOffDelay);
}

void GazePointer::CheckIfExiting(TimeSpan curTimestamp)
{
    for (unsigned int index = 0; index < _activeHitTargetTimes->Size; index++)
    {
        auto targetItem = _activeHitTargetTimes->GetAt(index);
        auto targetElement = targetItem->TargetElement;
        auto exitDelay = GetElementStateDelay(targetElement, PointerState::Exit);

        auto idleDuration = curTimestamp - targetItem->LastTimestamp;
        if (targetItem->ElementState != PointerState::PreEnter && idleDuration > exitDelay)
        {
            targetItem->ElementState = PointerState::PreEnter;
            GotoState(targetElement, PointerState::Exit);
            RaiseGazePointerEvent(targetItem, PointerState::Exit, targetItem->ElapsedTime);
            targetItem->GiveFeedback();

            _activeHitTargetTimes->RemoveAt(index);

            // remove all history samples referring to deleted hit target
            for (unsigned i = 0; i < _gazeHistory->Size; )
            {
                auto hitTarget = _gazeHistory->GetAt(i)->HitTarget;
                if (hitTarget->TargetElement == targetElement)
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

void GazePointer::RaiseGazePointerEvent(GazeTargetItem^ target, PointerState state, TimeSpan elapsedTime)
{
    auto control = target != nullptr ? target->TargetElement : nullptr;
    //assert(target != _rootElement);
    auto gpea = ref new StateChangedEventArgs(control, state, elapsedTime);
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

    auto gazeElement = target != nullptr ? GazeInput::GetGazeElement(control) : nullptr;

    if (gazeElement != nullptr)
    {
        gazeElement->RaiseStateChanged(control, gpea);
    }

    if (state == PointerState::Dwell)
    {
        auto handled = false;

        if (gazeElement != nullptr)
        {
            auto args = ref new DwellInvokedRoutedEventArgs();
            gazeElement->RaiseInvoked(control, args);
            handled = args->Handled;
        }

        if (!handled)
        {
            target->Invoke();
        }
    }
}

void GazePointer::OnGazeEntered(GazeInputSourcePreview^ provider, GazeEnteredPreviewEventArgs^ args)
{
    //Debug::WriteLine(L"Entered at %ld", args->CurrentPoint->Timestamp);
    _gazeCursor->IsGazeEntered = true;
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
                _gazeCursor->IsGazeEntered = true;
                ProcessGazePoint(TimeSpanFromMicroseconds(point->Timestamp), position->Value);
            }
            else
            {
                //Debug::WriteLine(L"Null position eaten at %ld", point->Timestamp);
            }
        }
    }
}

void GazePointer::OnGazeExited(GazeInputSourcePreview^ provider, GazeExitedPreviewEventArgs^ args)
{
    //Debug::WriteLine(L"Exited at %ld", args->CurrentPoint->Timestamp);
    _gazeCursor->IsGazeEntered = false;
}

void GazePointer::ProcessGazePoint(TimeSpan timestamp, Point position)
{
    auto ea = ref new GazeFilterArgs(position, timestamp);

    auto fa = Filter->Update(ea);
    _gazeCursor->Position = fa->Location;

    auto targetItem = ResolveHitTarget(fa->Location, fa->Timestamp);
    assert(targetItem != nullptr);

    //Debug::WriteLine(L"ProcessGazePoint: %llu -> [%d, %d], %llu", hitTarget->GetHashCode(), (int)fa->Location.X, (int)fa->Location.Y, fa->Timestamp);

    // check to see if any element in _hitTargetTimes needs an exit event fired.
    // this ensures that all exit events are fired before enter event
    CheckIfExiting(fa->Timestamp);

    PointerState nextState = static_cast<PointerState>(static_cast<int>(targetItem->ElementState) + 1);

    //Debug::WriteLine(L"%llu -> State=%d, Elapsed=%d, NextStateTime=%d", targetItem->TargetElement, targetItem->ElementState, targetItem->ElapsedTime, targetItem->NextStateTime);

    if (targetItem->ElapsedTime > targetItem->NextStateTime)
    {
        auto prevStateTime = targetItem->NextStateTime;

        // prevent targetItem from ever actually transitioning into the DwellRepeat state so as
        // to continuously emit the DwellRepeat event
        if (nextState != PointerState::DwellRepeat)
        {
            targetItem->ElementState = nextState;
            nextState = static_cast<PointerState>(static_cast<int>(nextState) + 1);     // nextState++
            targetItem->NextStateTime += GetElementStateDelay(targetItem->TargetElement, nextState);

            if (targetItem->ElementState == PointerState::Dwell)
            {
                targetItem->NextStateTime += GetElementStateDelay(targetItem->TargetElement, GazeInput::RepeatDelayDurationProperty, _defaultDwellRepeatDelay);
            }
        }
        else
        {
            // move the NextStateTime by one dwell period, while continuing to stay in Dwell state
            targetItem->NextStateTime += GetElementStateDelay(targetItem->TargetElement, PointerState::DwellRepeat);
        }

        if (targetItem->ElementState == PointerState::Dwell)
        {
            targetItem->RepeatCount++;
            if (targetItem->MaxDwellRepeatCount < targetItem->RepeatCount)
            {
                targetItem->NextStateTime = TimeSpan{ MAXINT64 };
            }
        }

        GotoState(targetItem->TargetElement, targetItem->ElementState);

        RaiseGazePointerEvent(targetItem, targetItem->ElementState, targetItem->ElapsedTime);
    }

    targetItem->GiveFeedback();

    _eyesOffTimer->Start();
    _lastTimestamp = fa->Timestamp;
}

END_NAMESPACE_GAZE_INPUT