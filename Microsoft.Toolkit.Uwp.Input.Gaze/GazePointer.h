//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "IGazeFilter.h"
#include "GazeCursor.h"
#include "GazePointerState.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Devices::Enumeration;
using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::UI::Core;
using namespace Windows::Devices::Input::Preview;

namespace Shapes = Windows::UI::Xaml::Shapes;

BEGIN_NAMESPACE_GAZE_INPUT

// units in microseconds
const int DEFAULT_FIXATION_DELAY = 400000;
const int DEFAULT_DWELL_DELAY = 800000;
const int DEFAULT_REPEAT_DELAY = 1600000;
const int DEFAULT_ENTER_EXIT_DELAY = 50000;
const int DEFAULT_MAX_HISTORY_DURATION = 3000000;
const int MAX_SINGLE_SAMPLE_DURATION = 100000;

const int GAZE_IDLE_TIME = 2500000;

ref struct GazeTargetItem;
ref struct GazeHistoryItem;

public ref class GazePointer sealed
{
public:
    virtual ~GazePointer();

    void LoadSettings(ValueSet^ settings);

    void Reset();
    void SetElementStateDelay(UIElement ^element, GazePointerState pointerState, int stateDelay);
    int GetElementStateDelay(UIElement^ element, GazePointerState pointerState);

    // Provide a configurable delay for when the EyesOffDelay event is fired
    // GOTCHA: this value requires that _eyesOffTimer is instantiated so that it
    // can update the timer interval 
    property int64 EyesOffDelay
    {
        int64 get() { return _eyesOffDelay; }
        void set(int64 value)
        {
            _eyesOffDelay = value;

            // convert GAZE_IDLE_TIME units (microseconds) to 100-nanosecond units used
            // by TimeSpan struct
            _eyesOffTimer->Interval = TimeSpan{ EyesOffDelay * 10 };
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

    static property GazePointer^ Instance{ GazePointer^ get(); }
    void OnPageUnloaded(Object^ sender, RoutedEventArgs^ args);
    EventRegistrationToken _unloadedToken;

    void AddRoot(FrameworkElement^ element);
    void RemoveRoot(FrameworkElement^ element);

private:

    GazePointer();

private:

    bool _isShuttingDown;

	TimeSpan GetDefaultPropertyValue(GazePointerState state);

    void    InitializeHistogram();
    void    InitializeGazeInputSource();
    void    DeinitializeGazeInputSource();

    void ActivateGazeTargetItem(GazeTargetItem^ target);
    GazeTargetItem^          GetHitTarget(Point gazePoint);
    GazeTargetItem^          ResolveHitTarget(Point gazePoint, long long timestamp);

    void    CheckIfExiting(long long curTimestamp);
    void    GotoState(UIElement^ control, GazePointerState state);
    void    RaiseGazePointerEvent(GazeTargetItem^ target, GazePointerState state, int64 elapsedTime);

    void OnGazeEntered(
        GazeInputSourcePreview^ provider,
        GazeEnteredPreviewEventArgs^ args);
    void OnGazeMoved(
        GazeInputSourcePreview^ provider,
        GazeMovedPreviewEventArgs^ args);
    void OnGazeExited(
        GazeInputSourcePreview^ provider,
        GazeExitedPreviewEventArgs^ args);

    void ProcessGazePoint(long long timestamp, Point position);

    void    OnEyesOff(Object ^sender, Object ^ea);

private:
    Vector<FrameworkElement^>^ _roots = ref new Vector<FrameworkElement^>();

    int64                               _eyesOffDelay;

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
    int64                               _maxHistoryTime;

    // Used to determine if exit events need to be fired by adding GAZE_IDLE_TIME to the last 
    // saved timestamp
    long long                           _lastTimestamp;

    GazeInputSourcePreview^             _gazeInputSource;
    EventRegistrationToken              _gazeEnteredToken;
    EventRegistrationToken              _gazeMovedToken;
    EventRegistrationToken              _gazeExitedToken;
    CoreDispatcher^                     _coreDispatcher;

	int _defaultFixation = DEFAULT_FIXATION_DELAY;
	int _defaultDwell = DEFAULT_DWELL_DELAY;
	int _defaultRepeat = DEFAULT_REPEAT_DELAY;
	int _defaultEnter = DEFAULT_ENTER_EXIT_DELAY;
	int _defaultExit = DEFAULT_ENTER_EXIT_DELAY;
};

END_NAMESPACE_GAZE_INPUT