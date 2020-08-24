//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "GazePointer.h"
#include "GazePointer.g.cpp"
#include "GazeElement.h"
#include "NonInvokeGazeTargetItem.h"
#include "GazeHistoryItem.h"
#include "GazeTargetItem.h"
#include "StateChangedEventArgs.h"
#include "GazeEventArgs.h"
#include "DwellInvokedRoutedEventArgs.h"
#include "winrt/Windows.UI.Core.h"

using namespace winrt;
using namespace winrt::Windows::Foundation;
using namespace winrt::Microsoft::UI::Xaml::Automation::Peers;
using namespace winrt::Windows::UI::Core;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    void GazePointer::AddRoot(int32_t proxyId)
    {
        _roots.insert(_roots.begin(), proxyId);

        if (_roots.size() == 1)
        {
            _isShuttingDown = false;
            InitializeGazeInputSource();
        }
    }

    void GazePointer::RemoveRoot(int32_t proxyId)
    {
        std::vector<int>::iterator index = std::find(_roots.begin(), _roots.end(), proxyId);
        if (index != _roots.end())
        {
            _roots.erase(index);
        }
        else
        {
            assert(false);
        }

        if (_roots.size() == 0)
        {
            _isShuttingDown = true;
            _gazeCursor.IsGazeEntered(false);
            DeinitializeGazeInputSource();
        }
    }

    bool GazePointer::IsDeviceAvailable()
    {
        return _devices.size() != 0;
    }

    winrt::event_token GazePointer::IsDeviceAvailableChanged(Windows::Foundation::EventHandler<Windows::Foundation::IInspectable> const& handler)
    {
        return m_isDeviceAvailableChanged.add(handler);
    }

    void GazePointer::IsDeviceAvailableChanged(winrt::event_token const& token) noexcept
    {
        m_isDeviceAvailableChanged.remove(token);
    }

    double GazePointer::_dwellStrokeThickness()
    {
        return m_dwellStrokeThickness;
    }

    void GazePointer::_dwellStrokeThickness(double const& value)
    {
        m_dwellStrokeThickness = value;
    }

    GazePointer::GazePointer() :
        _gazeInputSource{ nullptr },
        _watcher{ nullptr }
    {
        _nonInvokeGazeTargetItem = winrt::make<NonInvokeGazeTargetItem>();

        // Default to not filtering sample data
        Filter = std::make_unique<NullFilter>();

        // timer that gets called back if there gaze samples haven't been received in a while
        _eyesOffTimer = DispatcherTimer();
        _eyesOffTimer.Tick({ this, &GazePointer::OnEyesOff });

        // provide a default of GAZE_IDLE_TIME microseconds to fire eyes off 
        EyesOffDelay(GAZE_IDLE_TIME);

        InitializeHistogram();

        _devices.clear();
        _watcher = GazeInputSourcePreview::CreateWatcher();
        _watcher.Added({ this, &GazePointer::OnDeviceAdded });
        _watcher.Removed({ this, &GazePointer::OnDeviceRemoved });
        _watcher.Start();
    }

    winrt::event_token GazePointer::GazeEvent(Windows::Foundation::EventHandler<Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeEventArgs> const& handler)
    {
        _gazeEventCount++;
        return m_gazeEvent.add(handler);
    }

    void GazePointer::GazeEvent(winrt::event_token const& token) noexcept
    {
        _gazeEventCount--;
        m_gazeEvent.remove(token);
    }

    Microsoft::UI::Xaml::UIElement GazePointer::CursorElement()
    {
        return _gazeCursor.PopupChild();
    }

    void GazePointer::OnDeviceAdded(GazeDeviceWatcherPreview sender, GazeDeviceWatcherAddedPreviewEventArgs args)
    {
        _devices.push_back(args.Device());

        if (_devices.size() == 1)
        {
            m_isDeviceAvailableChanged(nullptr, nullptr);

            InitializeGazeInputSource();
        }
    }

    void GazePointer::OnDeviceRemoved(GazeDeviceWatcherPreview sender, GazeDeviceWatcherRemovedPreviewEventArgs args)
    {
        auto index = std::find(_devices.begin(), _devices.end(), args.Device());

        if (index != _devices.end())
        {
            _devices.erase(index);
        }
        else
        {
            _devices.erase(_devices.begin());
        }

        if (_devices.size() == 0)
        {
            m_isDeviceAvailableChanged(nullptr, nullptr);
        }
    }

    GazePointer::~GazePointer()
    {
        _watcher.Added(_deviceAddedToken);
        _watcher.Removed(_deviceRemovedToken);

        if (_gazeInputSource != nullptr)
        {
            _gazeInputSource.GazeEntered(_gazeEnteredToken);
            _gazeInputSource.GazeMoved(_gazeMovedToken);
            _gazeInputSource.GazeExited(_gazeExitedToken);
        }
    }

    bool GazePointer::IsAlwaysActivated()
    {
        return _isAlwaysActivated;
    }

    void GazePointer::IsAlwaysActivated(bool value)
    {
        _isAlwaysActivated = value;
    }

    Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazePointer GazePointer::m_instance{ nullptr };

    Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazePointer GazePointer::Instance()
    {
        if (m_instance == nullptr)
        {
            m_instance = Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazePointer();
        }
        return GazePointer::m_instance;
    }

    void GazePointer::LoadSettings(Windows::Foundation::Collections::ValueSet const& settings)
    {
        _gazeCursor.LoadSettings(settings);
        Filter->LoadSettings(settings);

        // TODO Add logic to protect against missing settings

        if (settings.HasKey(L"GazePointer.FixationDelay"))
        {
            _defaultFixation = TimeSpanFromMicroseconds(winrt::unbox_value<int>(settings.Lookup(L"GazePointer.FixationDelay")));
        }

        if (settings.HasKey(L"GazePointer.DwellDelay"))
        {
            _defaultDwell = TimeSpanFromMicroseconds(winrt::unbox_value<int>(settings.Lookup(L"GazePointer.DwellDelay")));
        }

        if (settings.HasKey(L"GazePointer.DwellRepeatDelay"))
        {
            _defaultDwellRepeatDelay = TimeSpanFromMicroseconds(winrt::unbox_value<int>(settings.Lookup(L"GazePointer.DwellRepeatDelay")));
        }

        if (settings.HasKey(L"GazePointer.RepeatDelay"))
        {
            _defaultRepeatDelay = TimeSpanFromMicroseconds(winrt::unbox_value<int>(settings.Lookup(L"GazePointer.RepeatDelay")));
        }

        if (settings.HasKey(L"GazePointer.ThresholdDelay"))
        {
            _defaultThreshold = TimeSpanFromMicroseconds(winrt::unbox_value<int>(settings.Lookup(L"GazePointer.ThresholdDelay")));
        }

        // TODO need to set fixation and dwell for all elements
        if (settings.HasKey(L"GazePointer.FixationDelay"))
        {
            SetElementStateDelay(_offScreenElement, PointerState::Fixation, TimeSpanFromMicroseconds(winrt::unbox_value<int>(settings.Lookup(L"GazePointer.FixationDelay"))));
        }
        if (settings.HasKey(L"GazePointer.DwellDelay"))
        {
            SetElementStateDelay(_offScreenElement, PointerState::Dwell, TimeSpanFromMicroseconds(winrt::unbox_value<int>(settings.Lookup(L"GazePointer.DwellDelay"))));
        }

        if (settings.HasKey(L"GazePointer.GazeIdleTime"))
        {
            EyesOffDelay(TimeSpanFromMicroseconds(winrt::unbox_value<int>(settings.Lookup(L"GazePointer.GazeIdleTime"))));
        }

        if (settings.HasKey(L"GazePointer.IsSwitchEnabled"))
        {
            IsSwitchEnabled(unbox_value<bool>(settings.Lookup(L"GazePointer.IsSwitchEnabled")));
        }
    }

    void GazePointer::InitializeHistogram()
    {
        _activeHitTargetTimes.clear();

        _offScreenElement = UserControl();
        SetElementStateDelay(_offScreenElement, PointerState::Fixation, _defaultFixation);
        SetElementStateDelay(_offScreenElement, PointerState::Dwell, _defaultDwell);

        _maxHistoryTime = DEFAULT_MAX_HISTORY_DURATION;    // maintain about 3 seconds of history (in microseconds)
        _gazeHistory.clear();
    }

    void GazePointer::InitializeGazeInputSource()
    {
        if (!_initialized)
        {
            if (_roots.size() != 0 && _devices.size() != 0)
            {
                if (_gazeInputSource == nullptr)
                {
                    _gazeInputSource = GazeInputSourcePreview::GetForCurrentView();
                }

                if (_gazeInputSource != nullptr)
                {
                    _gazeEnteredToken = _gazeInputSource.GazeEntered({ this, &GazePointer::OnGazeEntered });
                    _gazeMovedToken = _gazeInputSource.GazeMoved({ this, &GazePointer::OnGazeMoved });
                    _gazeExitedToken = _gazeInputSource.GazeExited({ this, &GazePointer::OnGazeExited });

                    _initialized = true;
                }
            }
        }
    }

    void GazePointer::DeinitializeGazeInputSource()
    {
        if (_initialized)
        {
            _initialized = false;

            _gazeInputSource.GazeEntered(_gazeEnteredToken);
            _gazeInputSource.GazeMoved(_gazeMovedToken);
            _gazeInputSource.GazeExited(_gazeExitedToken);
        }
    }

    static DependencyProperty GetProperty(PointerState state)
    {
        switch (state)
        {
        case PointerState::Fixation: return GazeInput::FixationDurationProperty();
        case PointerState::Dwell: return GazeInput::DwellDurationProperty();
        case PointerState::DwellRepeat: return GazeInput::DwellRepeatDurationProperty();
        case PointerState::Enter: return GazeInput::ThresholdDurationProperty();
        case PointerState::Exit: return GazeInput::ThresholdDurationProperty();
        default: return nullptr;
        }
    }

    TimeSpan GazePointer::GetDefaultPropertyValue(PointerState state)
    {
        switch (state)
        {
        case PointerState::Fixation: return _defaultFixation;
        case PointerState::Dwell: return _defaultDwell;
        case PointerState::DwellRepeat: return _defaultRepeatDelay;
        case PointerState::Enter: return _defaultThreshold;
        case PointerState::Exit: return _defaultThreshold;
        default: throw winrt::hresult_not_implemented();
        }
    }

    void GazePointer::SetElementStateDelay(UIElement element, PointerState relevantState, TimeSpan stateDelay)
    {
        auto property = GetProperty(relevantState);
        element.SetValue(property, winrt::box_value(stateDelay));

        // fix up _maxHistoryTime in case the new param exceeds the history length we are currently tracking
        auto dwellTime = GetElementStateDelay(element, PointerState::Dwell);
        auto repeatTime = GetElementStateDelay(element, PointerState::DwellRepeat);
        _maxHistoryTime = 2 * max(dwellTime, repeatTime);
    }

    /// <summary>
    /// Find the parent to inherit properties from.
    /// </summary>
    static UIElement GetInheritenceParent(UIElement child)
    {
        // The result value.
        UIElement parent{ nullptr };

        // Get the automation peer...
        auto peer = FrameworkElementAutomationPeer::FromElement(child);
        if (peer != nullptr)
        {
            // ...if it exists, get the peer's parent...
            FrameworkElementAutomationPeer peerParent{ peer.Navigate(AutomationNavigationDirection::Parent).try_as<FrameworkElementAutomationPeer>() };
            if (peerParent != nullptr)
            {
                // ...and if it has a parent, get the corresponding object.
                parent = peerParent.Owner();
            }
        }

        // If the above failed to find a parent...
        if (parent == nullptr)
        {
            // ...use the visual parent.
            parent = VisualTreeHelper::GetParent(child).try_as<UIElement>();
        }

        // Safely pun the value we found to a UIElement reference.
        return parent;
    }

    TimeSpan GazePointer::GetElementStateDelay(UIElement element, DependencyProperty property, TimeSpan defaultValue)
    {
        UIElement walker = element;
        auto valueAtWalker = winrt::unbox_value<TimeSpan>(walker.GetValue(property));

        while (GazeInput::UnsetTimeSpan == valueAtWalker && walker != nullptr)
        {
            walker = GetInheritenceParent(walker);

            if (walker != nullptr)
            {
                valueAtWalker = winrt::unbox_value<TimeSpan>(walker.GetValue(property));
            }
        }

        auto ticks = GazeInput::UnsetTimeSpan == valueAtWalker ? defaultValue : valueAtWalker;

        return ticks;
    }

    TimeSpan GazePointer::GetElementStateDelay(UIElement element, PointerState pointerState)
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
        _activeHitTargetTimes.clear();
        _gazeHistory.clear();

        _maxHistoryTime = DEFAULT_MAX_HISTORY_DURATION;
    }

    winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem GazePointer::GetHitTarget(Point gazePoint)
    {
        winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem invokable{ nullptr };

        switch (Window::Current().CoreWindow().ActivationMode())
        {
        default:
            if (!_isAlwaysActivated)
            {
                invokable = _nonInvokeGazeTargetItem;
                break;
            }

        case CoreWindowActivationMode::ActivatedInForeground:
        case CoreWindowActivationMode::ActivatedNotForeground:
            auto elements = VisualTreeHelper::FindElementsInHostCoordinates(gazePoint, nullptr, false);
            auto first = elements.First();
            auto element = first.HasCurrent() ? first.Current() : nullptr;

            invokable = nullptr;

            if (element != nullptr)
            {
                invokable = GazeTargetItem::GetOrCreate(element);

                while (element != nullptr && !invokable.IsInvokable())
                {
                    element = VisualTreeHelper::GetParent(element).try_as<UIElement>();

                    if (element != nullptr)
                    {
                        invokable = GazeTargetItem::GetOrCreate(element);
                    }
                }
            }

            if (element == nullptr || !invokable.IsInvokable())
            {
                invokable = _nonInvokeGazeTargetItem;
            }
            else
            {
                Interaction interaction;
                do
                {
                    interaction = GazeInput::GetInteraction(element);
                    if (interaction == Interaction::Inherited)
                    {
                        element = GetInheritenceParent(element);
                    }
                } while (interaction == Interaction::Inherited && element != nullptr);

                if (interaction == Interaction::Inherited)
                {
                    interaction = GazeInput::Interaction();
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

    void GazePointer::ActivateGazeTargetItem(winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem target)
    {
        auto index = std::find(_activeHitTargetTimes.begin(), _activeHitTargetTimes.end(), target);
        if (index == _activeHitTargetTimes.end())
        {
            _activeHitTargetTimes.push_back(target);

            // calculate the time that the first DwellRepeat needs to be fired after. this will be updated every time a DwellRepeat is 
            // fired to keep track of when the next one is to be fired after that.
            auto nextStateTime = GetElementStateDelay(target.TargetElement(), PointerState::Enter);

            target.Reset(nextStateTime);
        }
    }

    winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem GazePointer::ResolveHitTarget(Point gazePoint, TimeSpan timestamp)
    {
        // TODO: The existence of a GazeTargetItem should be used to indicate that
        // the target item is invokable. The method of invocation should be stored
        // within the GazeTargetItem when it is created and not recalculated when
        // subsequently needed.

        // create GazeHistoryItem to deal with this sample
        auto target = GetHitTarget(gazePoint);
        auto historyItem = winrt::make<GazeHistoryItem>();
        historyItem.HitTarget(target);
        historyItem.Timestamp(timestamp);
        historyItem.Duration(TimeSpanZero);
        assert(historyItem.HitTarget() != nullptr);

        // create new GazeTargetItem with a (default) total elapsed time of zero if one does not exist already.
        // this ensures that there will always be an entry for target elements in the code below.
        ActivateGazeTargetItem(target);
        target.LastTimestamp(timestamp);

        // find elapsed time since we got the last hit target
        historyItem.Duration(timestamp - _lastTimestamp);
        if (historyItem.Duration() > MAX_SINGLE_SAMPLE_DURATION)
        {
            historyItem.Duration(MAX_SINGLE_SAMPLE_DURATION);
        }
        _gazeHistory.push_back(historyItem);

        // update the time this particular hit target has accumulated
        target.DetailedTime(target.DetailedTime() + historyItem.Duration());

        // drop the oldest samples from the list until we have samples only 
        // within the window we are monitoring
        //
        // historyItem is the last item we just appended a few lines above. 
        for (auto evOldest = _gazeHistory.begin();
            historyItem.Timestamp() - evOldest->Timestamp() > _maxHistoryTime;
            evOldest = _gazeHistory.begin())
        {
            evOldest = _gazeHistory.erase(evOldest);

            // subtract the duration obtained from the oldest sample in _gazeHistory
            auto targetItem = evOldest->HitTarget();
            assert(target.DetailedTime() - evOldest->Duration() >= TimeSpanZero);
            targetItem.DetailedTime(targetItem.DetailedTime() - evOldest->Duration());
            if (targetItem.ElementState() != PointerState::PreEnter)
            {
                targetItem.OverflowTime(targetItem.OverflowTime() + evOldest->Duration());
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

    void GazePointer::OnEyesOff(IInspectable const& sender, IInspectable const& ea)
    {
        _eyesOffTimer.Stop();

        CheckIfExiting(_lastTimestamp + EyesOffDelay());
        RaiseGazePointerEvent(nullptr, PointerState::Enter, EyesOffDelay());
    }

    void GazePointer::CheckIfExiting(TimeSpan curTimestamp)
    {
        for (auto targetItem = _activeHitTargetTimes.begin(); targetItem != _activeHitTargetTimes.end(); ++targetItem)
        {
            auto targetElement = targetItem->TargetElement();
            auto exitDelay = GetElementStateDelay(targetElement, PointerState::Exit);

            auto idleDuration = curTimestamp - targetItem->LastTimestamp();
            if (targetItem->ElementState() != PointerState::PreEnter && idleDuration > exitDelay)
            {
                targetItem->ElementState(PointerState::PreEnter);

                // Transitioning to exit - clear the cached fixated element
                _currentlyFixatedElement = nullptr;

                RaiseGazePointerEvent(*targetItem, PointerState::Exit, targetItem->ElapsedTime());
                targetItem->GiveFeedback();

                _activeHitTargetTimes.erase(targetItem);

                // remove all history samples referring to deleted hit target
                for (auto item = _gazeHistory.begin(); item != _gazeHistory.end(); )
                {
                    auto hitTarget = item->HitTarget();
                    if (hitTarget.TargetElement() == targetElement)
                    {
                        item = _gazeHistory.erase(item);
                    }
                    else
                    {
                        ++item;
                    }
                }

                // return because only one element can be exited at a time and at this point
                // we have done everything that we can do
                return;
            }
        }
    }

    wchar_t* PointerStates[] = {
        L"Exit",
        L"PreEnter",
        L"Enter",
        L"Fixation",
        L"Dwell",
        L"DwellRepeat"
    };

    void GazePointer::RaiseGazePointerEvent(winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem target, PointerState state, TimeSpan elapsedTime)
    {
        auto control = target != nullptr ? target.TargetElement() : nullptr;
        //assert(target != _rootElement);
        auto gpea = winrt::make<StateChangedEventArgs>(control, state, elapsedTime);
        //auto buttonObj = dynamic_cast<Button >(target);
        //if (buttonObj && buttonObj.Content)
        //{
        //    String buttonText = dynamic_cast<String>(buttonObj.Content);
        //    Debug::WriteLine(L"GPE: %s . %s, %d", buttonText, PointerStates[(int)state], elapsedTime);
        //}
        //else
        //{
        //    Debug::WriteLine(L"GPE: 0x%08x . %s, %d", target != nullptr ? target.GetHashCode() : 0, PointerStates[(int)state], elapsedTime);
        //}

        auto gazeElement = target != nullptr ? GazeInput::GetGazeElement(control) : nullptr;

        if (gazeElement != nullptr)
        {
            gazeElement.RaiseStateChanged(control, gpea);
        }

        if (state == PointerState::Dwell)
        {
            auto handled = false;

            if (gazeElement != nullptr)
            {
                auto args = winrt::make<DwellInvokedRoutedEventArgs>();
                gazeElement.RaiseInvoked(control, args);
                handled = args.Handled();
            }

            if (!handled)
            {
                target.Invoke();
            }
        }
    }

    void GazePointer::OnGazeEntered(GazeInputSourcePreview provider, GazeEnteredPreviewEventArgs args)
    {
        //Debug::WriteLine(L"Entered at %ld", args.CurrentPoint().Timestamp());
        _gazeCursor.IsGazeEntered(true);
    }

    void GazePointer::OnGazeMoved(GazeInputSourcePreview provider, GazeMovedPreviewEventArgs args)
    {
        if (!_isShuttingDown)
        {
            auto intermediatePoints = args.GetIntermediatePoints();
            for (auto point : intermediatePoints)
            {
                auto position = point.EyeGazePosition();
                if (position != nullptr)
                {
                    _gazeCursor.IsGazeEntered(true);
                    ProcessGazePoint(TimeSpanFromMicroseconds(point.Timestamp()), position.Value());
                }
                else
                {
                    //Debug::WriteLine(L"Null position eaten at %ld", point.Timestamp());
                }
            }
        }
    }

    void GazePointer::OnGazeExited(GazeInputSourcePreview provider, GazeExitedPreviewEventArgs args)
    {
        //Debug::WriteLine(L"Exited at %ld", args.CurrentPoint().Timestamp());
        _gazeCursor.IsGazeEntered(false);
    }

    void GazePointer::ProcessGazePoint(TimeSpan timestamp, Point position)
    {
        auto ea = GazeFilterArgs(position, timestamp);

        auto fa = Filter->Update(ea);
        _gazeCursor.Position(fa.Location());

        if (_gazeEventCount != 0)
        {
            auto gazeEventArgs = winrt::make<GazeEventArgs>();
            gazeEventArgs.Set(fa.Location(), timestamp);
            m_gazeEvent(*this, gazeEventArgs);
            if (gazeEventArgs.Handled())
            {
                return;
            }
        }

        auto targetItem = ResolveHitTarget(fa.Location(), fa.Timestamp());
        assert(targetItem != nullptr);

        //Debug::WriteLine(L"ProcessGazePoint. [%d, %d], %llu", (int)fa.Location().X, (int)fa.Location().Y, fa.Timestamp());

        // check to see if any element in _hitTargetTimes needs an exit event fired.
        // this ensures that all exit events are fired before enter event
        CheckIfExiting(fa.Timestamp());

        PointerState nextState = static_cast<PointerState>(static_cast<int>(targetItem.ElementState()) + 1);

        //Debug::WriteLine(L"%llu", targetItem.TargetElement());
        //Debug::WriteLine(L"\tState=%d, Elapsed=%d, NextStateTime=%d", targetItem.ElementState(), targetItem.ElapsedTime(), targetItem.NextStateTime());

        if (targetItem.ElapsedTime() > targetItem.NextStateTime())
        {
            auto prevStateTime = targetItem.NextStateTime();

            // prevent targetItem from ever actually transitioning into the DwellRepeat state so as
            // to continuously emit the DwellRepeat event
            if (nextState != PointerState::DwellRepeat)
            {
                targetItem.ElementState(nextState);
                nextState = static_cast<PointerState>(static_cast<int>(nextState) + 1);     // nextState++
                targetItem.NextStateTime(targetItem.NextStateTime() + GetElementStateDelay(targetItem.TargetElement(), nextState));

                if (targetItem.ElementState() == PointerState::Dwell)
                {
                    targetItem.NextStateTime(targetItem.NextStateTime() + GetElementStateDelay(targetItem.TargetElement(), GazeInput::RepeatDelayDurationProperty(), _defaultDwellRepeatDelay));
                }
            }
            else
            {
                // move the NextStateTime by one dwell period, while continuing to stay in Dwell state
                targetItem.NextStateTime(targetItem.NextStateTime() + GetElementStateDelay(targetItem.TargetElement(), PointerState::DwellRepeat));
            }

            if (targetItem.ElementState() == PointerState::Dwell)
            {
                targetItem.RepeatCount(targetItem.RepeatCount() + 1);
                if (targetItem.MaxDwellRepeatCount() < targetItem.RepeatCount())
                {
                    targetItem.NextStateTime(TimeSpan{ MAXINT64 });
                }
            }

            if (targetItem.ElementState() == PointerState::Fixation)
            {
                // Cache the fixated item
                _currentlyFixatedElement = targetItem;

                // We are about to transition into the Dwell state
                // If switch input is enabled, make sure dwell never completes
                // via eye gaze
                if (_isSwitchEnabled)
                {
                    // Don't allow the next state (Dwell) to progress
                    targetItem.NextStateTime(TimeSpan{ MAXINT64 });
                }
            }

            RaiseGazePointerEvent(targetItem, targetItem.ElementState(), targetItem.ElapsedTime());
        }

        targetItem.GiveFeedback();

        _eyesOffTimer.Start();
        _lastTimestamp = fa.Timestamp();
    }

    /// <summary>
    /// When in switch mode, will issue a click on the currently fixated element
    /// </summary>
    void GazePointer::Click()
    {
        if (_isSwitchEnabled &&
            _currentlyFixatedElement != nullptr)
        {
            _currentlyFixatedElement.Invoke();
        }
    }

    /// <summary>
    /// Run device calibration.
    /// </summary>
    Windows::Foundation::IAsyncOperation<bool> GazePointer::RequestCalibrationAsync()
    {
        if (_devices.size() == 1)
        {
            return co_await _devices[0].RequestCalibrationAsync();
        }
        else
        {
            co_return false;
        }
    }

    bool GazePointer::IsCursorVisible()
    {
        return _gazeCursor.IsCursorVisible();
    }

    void GazePointer::IsCursorVisible(bool const& value)
    {
        _gazeCursor.IsCursorVisible(value);
    }

    int GazePointer::CursorRadius()
    {
        return _gazeCursor.CursorRadius();
    }

    void GazePointer::CursorRadius(int value)
    {
        _gazeCursor.CursorRadius(value);
    }

    bool GazePointer::IsSwitchEnabled()
    {
        return _isSwitchEnabled;
    }

    void GazePointer::IsSwitchEnabled(bool value)
    {
        _isSwitchEnabled = value;
    }
}