//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "GazeCursor.h"
#include "GazeFeedbackPopupFactory.h"
#include "IGazeFilter.h"
#include "Interaction.h"
#include "PointerState.h"

using namespace Platform::Collections;
using namespace Windows::Devices::Input::Preview;
using namespace Windows::Foundation;

BEGIN_NAMESPACE_GAZE_INPUT

ref class GazeTargetItem;
ref struct GazeHistoryItem;

/// <summary>
/// Class of singleton object coordinating gaze input.
/// </summary>
public ref class GazePointer sealed
{
private:

    // units in microseconds
    const TimeSpan DEFAULT_FIXATION_DELAY = TimeSpanFromMicroseconds(350000);
    const TimeSpan DEFAULT_DWELL_DELAY = TimeSpanFromMicroseconds(400000);
    const TimeSpan DEFAULT_DWELL_REPEAT_DELAY = TimeSpanFromMicroseconds(400000);
    const TimeSpan DEFAULT_REPEAT_DELAY = TimeSpanFromMicroseconds(400000);
    const TimeSpan DEFAULT_THRESHOLD_DELAY = TimeSpanFromMicroseconds(50000);
    const TimeSpan DEFAULT_MAX_HISTORY_DURATION = TimeSpanFromMicroseconds(3000000);
    const TimeSpan MAX_SINGLE_SAMPLE_DURATION = TimeSpanFromMicroseconds(100000);

    const TimeSpan GAZE_IDLE_TIME{ 25000000 };

public:
    virtual ~GazePointer();

    /// <summary>
    /// Loads a settings collection into GazePointer.
    /// </summary>
    void LoadSettings(ValueSet^ settings);

internal:
    Brush^ _enterBrush = nullptr;

    Brush^ _progressBrush = ref new SolidColorBrush(Colors::Green);

    Brush^ _completeBrush = ref new SolidColorBrush(Colors::Red);

    Interaction _interaction = Interaction::Disabled;

    GazeTargetItem^ _nonInvokeGazeTargetItem;

    GazeFeedbackPopupFactory^ _gazeFeedbackPopupFactory = ref new GazeFeedbackPopupFactory();

internal:
    void Reset();
    void SetElementStateDelay(UIElement ^element, PointerState pointerState, TimeSpan stateDelay);
    TimeSpan GetElementStateDelay(UIElement ^element, DependencyProperty^ property, TimeSpan defaultValue);
    TimeSpan GetElementStateDelay(UIElement^ element, PointerState pointerState);

    // Provide a configurable delay for when the EyesOffDelay event is fired
    // GOTCHA: this value requires that _eyesOffTimer is instantiated so that it
    // can update the timer interval 
    property TimeSpan EyesOffDelay
    {
        TimeSpan get() { return _eyesOffDelay; }
        void set(TimeSpan value)
        {
            _eyesOffDelay = value;

            // convert GAZE_IDLE_TIME units (microseconds) to 100-nanosecond units used
            // by TimeSpan struct
            _eyesOffTimer->Interval = EyesOffDelay;
        }
    }

    // Pluggable filter for eye tracking sample data. This defaults to being set to the
    // NullFilter which performs no filtering of input samples.
    property IGazeFilter^ Filter;

    property bool IsCursorVisible
    {
        bool get() { return _gazeCursor->IsCursorVisible; }
        void set(bool value) { _gazeCursor->IsCursorVisible = value; }
    }

    property int CursorRadius
    {
        int get() { return _gazeCursor->CursorRadius; }
        void set(int value) { _gazeCursor->CursorRadius = value; }
    }

internal:

    static property GazePointer^ Instance { GazePointer^ get(); }
    void OnPageUnloaded(Object^ sender, RoutedEventArgs^ args);
    EventRegistrationToken _unloadedToken;

    void AddRoot(FrameworkElement^ element);
    void RemoveRoot(FrameworkElement^ element);


    property bool IsDeviceAvailable { bool get() { return _deviceCount != 0; }}
    event EventHandler<Object^>^ IsDeviceAvailableChanged;

private:

    GazePointer();

private:

    bool _isShuttingDown;

    TimeSpan GetDefaultPropertyValue(PointerState state);

    void    InitializeHistogram();
    void    InitializeGazeInputSource();
    void    DeinitializeGazeInputSource();

    void ActivateGazeTargetItem(GazeTargetItem^ target);
    GazeTargetItem^          GetHitTarget(Point gazePoint);
    GazeTargetItem^          ResolveHitTarget(Point gazePoint, TimeSpan timestamp);

    void    CheckIfExiting(TimeSpan curTimestamp);
    void    GotoState(UIElement^ control, PointerState state);
    void    RaiseGazePointerEvent(GazeTargetItem^ target, PointerState state, TimeSpan elapsedTime);

    void OnGazeEntered(
        GazeInputSourcePreview^ provider,
        GazeEnteredPreviewEventArgs^ args);
    void OnGazeMoved(
        GazeInputSourcePreview^ provider,
        GazeMovedPreviewEventArgs^ args);
    void OnGazeExited(
        GazeInputSourcePreview^ provider,
        GazeExitedPreviewEventArgs^ args);

    void ProcessGazePoint(TimeSpan timestamp, Point position);

    void    OnEyesOff(Object ^sender, Object ^ea);

    void OnDeviceAdded(GazeDeviceWatcherPreview^ sender, GazeDeviceWatcherAddedPreviewEventArgs^ args);
    void OnDeviceRemoved(GazeDeviceWatcherPreview^ sender, GazeDeviceWatcherRemovedPreviewEventArgs^ args);

private:
    Vector<FrameworkElement^>^ _roots = ref new Vector<FrameworkElement^>();

    TimeSpan                               _eyesOffDelay;

    GazeCursor^                         _gazeCursor;
    DispatcherTimer^                    _eyesOffTimer;

    // _offScreenElement is a pseudo-element that represents the area outside
    // the screen so we can track how long the user has been looking outside
    // the screen and appropriately trigger the EyesOff event
    Control^                            _offScreenElement;

    // The value is the total time that FrameworkElement has been gazed at
    Vector<GazeTargetItem^>^            _activeHitTargetTimes;

    // A vector to track the history of observed gaze targets
    Vector<GazeHistoryItem^>^           _gazeHistory;
    TimeSpan                               _maxHistoryTime;

    // Used to determine if exit events need to be fired by adding GAZE_IDLE_TIME to the last 
    // saved timestamp
    TimeSpan                           _lastTimestamp;

    GazeInputSourcePreview^             _gazeInputSource;
    EventRegistrationToken              _gazeEnteredToken;
    EventRegistrationToken              _gazeMovedToken;
    EventRegistrationToken              _gazeExitedToken;

    GazeDeviceWatcherPreview^ _watcher;
    int _deviceCount;
    EventRegistrationToken _deviceAddedToken;
    EventRegistrationToken _deviceRemovedToken;

    TimeSpan _defaultFixation = DEFAULT_FIXATION_DELAY;
    TimeSpan _defaultDwell = DEFAULT_DWELL_DELAY;
    TimeSpan _defaultDwellRepeatDelay = DEFAULT_DWELL_REPEAT_DELAY;
    TimeSpan _defaultRepeat = DEFAULT_REPEAT_DELAY;
    TimeSpan _defaultThreshold = DEFAULT_THRESHOLD_DELAY;
};

END_NAMESPACE_GAZE_INPUT